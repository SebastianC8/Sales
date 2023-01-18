namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributtes
        private ObservableCollection<ProductItemViewModel> products;
        private ApiService apiService;
        private DataService dataService;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties

        public List<Product> MyProducts { get; set; }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadProducts();
        }
        #endregion

        #region Singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            try
            {
                this.IsRefreshing = true;
                var connection = await this.apiService.CheckConnection();

                if (connection.IsSuccess)
                {
                    var answer = await this.LoadProductsFromAPI();
                    if (answer)
                    {
                        await this.SaveProductsToDB();
                    }
                }
                else
                {
                    await this.LoadProductFromDB();
                }

                if (this.MyProducts == null || this.MyProducts.Count == 0)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(Languages.Error, "No hay productos disponibles", Languages.Accept);
                    return;
                }

                /** Endpoint request*/
                
                this.RefreshList();
                this.IsRefreshing = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, ex.Message, Languages.Accept); ;
            }

        }

        private async Task<bool> LoadProductsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.GetList<Product>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyProducts = (List<Product>)response.Result;
            return true;
        }

        private async Task LoadProductFromDB()
        {
            this.MyProducts = await this.dataService.GetAllProducts();
        }

        private async Task SaveProductsToDB()
        {
            await this.dataService.DeleteAllProducts();
            this.dataService.Insert(this.MyProducts);
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var listProductItemViewModel = this.MyProducts.Select(product => new ProductItemViewModel
                {
                    Description = product.Description,
                    ImageArray = product.ImageArray,
                    ImagePath = product.ImagePath,
                    IsAvailable = product.IsAvailable,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    PublishOn = product.PublishOn,
                    Remarks = product.Remarks
                });

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    listProductItemViewModel.OrderBy(product => product.Description)
                );
            }
            else
            {
                var listProductItemViewModel = this.MyProducts.Select(product => new ProductItemViewModel
                {
                    Description = product.Description,
                    ImageArray = product.ImageArray,
                    ImagePath = product.ImagePath,
                    IsAvailable = product.IsAvailable,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    PublishOn = product.PublishOn,
                    Remarks = product.Remarks
                }).Where(product => product.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    listProductItemViewModel.OrderBy(product => product.Description)
                );
            }

        }

        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }
        #endregion

    }
}
