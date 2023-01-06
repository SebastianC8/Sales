namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributtes
        private ObservableCollection<Product> products;
        private ApiService apiService;
        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
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
        #endregion

        private async void LoadProducts()
        {
            try
            {
                this.IsRefreshing = true;
                var connection = await this.apiService.CheckConnection();

                if (!connection.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                    return;
                }
                
                /** Endpoint request*/
                var url = Application.Current.Resources["UrlAPI"].ToString();
                var prefix = Application.Current.Resources["UrlPrefix"].ToString();
                var controller = Application.Current.Resources["UrlProductsController"].ToString();

                var response = await this.apiService.GetList<Product>(url, prefix, controller);

                if (!response.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                    return;
                }

                var list = (List<Product>)response.Result;
                this.Products = new ObservableCollection<Product>(list);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, ex.Message, Languages.Accept); ;
            }

        }

    }
}
