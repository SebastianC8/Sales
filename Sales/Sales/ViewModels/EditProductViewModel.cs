
namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {

        #region Attributes
        private Product product;
        private ApiService apiService;
        private ImageSource imageSource;
        private MediaFile file;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public Product Product
        {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }

            set
            {
                this.SetValue(ref this.isRunning, value);
            }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }

            set
            {
                this.SetValue(ref this.isEnabled, value);
            }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }

            set
            {
                this.SetValue(ref this.imageSource, value);
            }
        }
        #endregion

        #region Constructors
        public EditProductViewModel(Product product)
        {
            this.apiService = new ApiService();
            this.product = product;
            this.IsEnabled = true;
            this.ImageSource = product.ImageFullPath;
        }
        #endregion

        #region Methods
        private async void Save()
        {
            /* Validación de campos */

            if (string.IsNullOrEmpty(this.Product.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept
                );

                return;
            }

            if (this.Product.Price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept
                );

                return;
            }

            /* Valores para loading y botón */
            this.IsRunning = true;
            this.IsEnabled = false;

            /* Se verifica conexión */
            var connection = await this.apiService.CheckConnection();

            /** Si no hay conexión */
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept
                );

                return;
            }

            byte[] imageArray = null;

            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }
            
            /** Endpoint request*/
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            /* Se realiza petición HTTP */
            var response = await this.apiService.Put(url, prefix, controller, this.Product, this.Product.ProductId, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept
                );

                return;
            }

            /* Se adiciona el nuevo producto en la lista de productos de ProductsViewModel */
            var newProduct = (Product)response.Result;
            var productsViewModel = ProductsViewModel.GetInstance();
            var oldProduct = productsViewModel.MyProducts.Where(product => product.ProductId == this.Product.ProductId).FirstOrDefault();

            if (oldProduct != null)
            {
                productsViewModel.MyProducts.Remove(oldProduct);
            }

            productsViewModel.MyProducts.Add(newProduct);

            /* Se refresca la lista de productos en ProductsViewModel */
            productsViewModel.RefreshList();

            /* Se desapila la vista actual y regresamos a ProductsPage */
            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();

        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture
            );

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        private async void Delete()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No
            );

            if (!answer)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            /* Se verifica conexión */
            var connection = await this.apiService.CheckConnection();

            /** Si no hay conexión */
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept
                );

                return;
            }

            /** Endpoint request*/
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            /* Se realiza petición HTTP */
            var response = await this.apiService.Delete(url, prefix, controller, this.Product.ProductId, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept
                );

                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productsViewModel.MyProducts.Where(product => product.ProductId == this.Product.ProductId).FirstOrDefault();

            if (deletedProduct != null)
            {
                productsViewModel.MyProducts.Remove(deletedProduct);
            }

            productsViewModel.RefreshList();

            this.IsRunning = true;
            this.IsEnabled = false;

            await App.Navigator.PopAsync();
        }
        #endregion

        #region Commands

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        #endregion
    }
}
