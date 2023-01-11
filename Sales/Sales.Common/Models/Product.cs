namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        /* Definición de clave primaria numérica */
        [Key]
        public int ProductId { get; set; }

        /* Campo obligatorio */
        [Required]
        [StringLength(50)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false )]
        public Decimal Price { get; set; }

        [Display(Name = "Está disponible")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publicación")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }

        [Display(Name ="Imagen")]
        public string ImagePath { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        /* La data annotation NotMapped excluye una propiedad del mapeo del modelo en la base de datos */
        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }

                return $"https://salesbackendcorrales.azurewebsites.net/{this.ImagePath.Substring(1)}";

            }

        }

        public override string ToString()
        {
            return $"Producto: {this.Description.ToString()}, Precio: $ {this.Price.ToString()}";
        }
    }
}
