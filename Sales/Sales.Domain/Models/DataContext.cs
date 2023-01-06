namespace Sales.Domain.Models
{
    using Sales.Common.Models;
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        /* Se instancia constructor de clase padre y se le pasa el nombre del string de conexión */
        public DataContext() : base("DefaultConnection") { }

        public DbSet<Product> Products { get; set; }
    }
}
