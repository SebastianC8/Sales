using Xamarin.Forms;

namespace Sales
{
    using Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /* Se establece página de inicio para la aplicación */
            MainPage = new NavigationPage(new ProductsPage());
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
