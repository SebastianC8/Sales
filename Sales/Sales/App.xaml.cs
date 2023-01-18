using Xamarin.Forms;

namespace Sales
{
    using Sales.Helpers;
    using Sales.ViewModels;
    using Views;

    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public App()
        {
            InitializeComponent();

            if (Settings.IsRemembered && !string.IsNullOrEmpty(Settings.AccessToken))
            {
                /* Se establece página de inicio para la aplicación */
                MainViewModel.GetInstance().Products = new ProductsViewModel();
                MainPage = new MasterPage();
            }
            else
            {
                /* Se establece página de inicio para la aplicación */
                MainViewModel.GetInstance().Login = new LoginViewModel();
                MainPage = new NavigationPage(new LoginPage());
            }

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
