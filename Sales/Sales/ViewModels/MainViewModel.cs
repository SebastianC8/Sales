namespace Sales.ViewModels
{
    public class MainViewModel
    {

        public ProductsViewModel Products;

        public MainViewModel()
        {
            this.Products = new ProductsViewModel();
        }
    }
}
