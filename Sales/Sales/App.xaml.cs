using Xamarin.Forms;

namespace Sales
{
    using Sales.ViewModels;
    using Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /* Se establece página de inicio para la aplicación */
            MainViewModel.GetInstance().Login = new LoginViewModel();
            MainPage = new LoginPage();
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
