namespace Sales.Interfaces
{
    using System.Globalization;

    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void setLocale(CultureInfo ci);
    }
}
