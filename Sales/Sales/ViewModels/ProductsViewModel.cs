namespace Sales.ViewModels
{
    using Sales.ViewModels;
    using Sales.Common.Models;
    using Sales.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class ProductsViewModel: BaseViewModel
    {

        #region Atributtes
        private ObservableCollection<Product> products;
        private ApiService apiService;
        #endregion

        #region Properties
        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        #endregion

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this.apiService.GetList<Product>("https://salesbackendcorrales.azurewebsites.net", "/api/", "Products");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var list = (List<Product>) response.Result;
            this.Products = new ObservableCollection<Product>(list);

        }
    }
}
