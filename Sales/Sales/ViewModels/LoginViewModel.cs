using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemembered { get; set; }

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
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.IsEnabled = true;
            this.IsRemembered = true;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        #endregion

        #region Methods
        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    "You must enter a email",
                    Languages.Accept
                );

                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    "You must enter a password",
                    Languages.Accept
                );

                return;
            }

            return;
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new ProductsPage()));
        }
        #endregion

    }
}
