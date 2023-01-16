﻿namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributtes
        private ObservableCollection<ProductItemViewModel> products;
        private ApiService apiService;
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

                this.MyProducts = (List<Product>)response.Result;
                this.RefreshList();
                this.IsRefreshing = false;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, ex.Message, Languages.Accept); ;
            }

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
