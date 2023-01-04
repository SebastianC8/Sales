namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        /* Definición de clave primaria numérica */
        [Key]
        public int ProductId { get; set; }

        /* Campo obligatorio */
        [Required]
        public string Description { get; set; }

        public Decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime PublishOn { get; set; }

        public override string ToString()
        {
            return this.Description.ToString();
        }
    }
}
