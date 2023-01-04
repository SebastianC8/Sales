using Sales.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sales
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            /* Se establece página de inicio para la aplicación */
            MainPage = new ProductsPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
