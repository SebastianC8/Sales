﻿namespace Sales.ViewModels
{
    using System.Windows.Input;
    using System.Linq;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;

    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructors
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commands
        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct()
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

            /* Se verifica conexión */
            var connection = await this.apiService.CheckConnection();

            /** Si no hay conexión */
            if (!connection.IsSuccess)
            {
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
            var response = await this.apiService.Delete(url, prefix, controller, this.ProductId);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept
                );

                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productsViewModel.Products.Where(product => product.ProductId == this.ProductId).FirstOrDefault();

            if (deletedProduct != null)
            {
                productsViewModel.Products.Remove(deletedProduct);
            }

        }
        #endregion
    }
}
