namespace Sales.Helpers
{

    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string TOKEN_TYPE = "TokenType";
        private const string ACCESS_TOKEN = "AccessToken";
        private const string IS_REMEMBERED = "IsRemembered";

        private static readonly string stringDefault = string.Empty;
        private static readonly bool booleanDefault = false;

        #endregion


        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(TOKEN_TYPE, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TOKEN_TYPE, value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(ACCESS_TOKEN, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ACCESS_TOKEN, value);
            }
        }

        public static bool IsRemembered
        {
            get
            {
                return AppSettings.GetValueOrDefault(IS_REMEMBERED, booleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IS_REMEMBERED, value);
            }
        }

    }
}
