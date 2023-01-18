namespace Sales.Helpers
{
    using System.Net.Mail;
    using System;

    public static class RegexHelper
    {
        public static bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                var email = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
