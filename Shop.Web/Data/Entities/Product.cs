namespace Shop.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Sockets;

    public class Product : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")] // Personalización de mensaje
        [Required]
        public string Name { get; set; }

        // Currency 2 -> establece el formato que tenga configurado en la maquina
        // le pone separadores de miles, dos decimales .00
        // formato de moneda
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastPurchase { get; set; } // ? -> significa que permite almacenar valores nulos

        [Display(Name = "Last Sale")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Availabe?")]
        public bool IsAvailabe { get; set; }

        // N2 -> Esto es númerico de 2, aquí no le pone simbolo de pesos y le marca 2 decimales
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public User User { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }

                // Obtención de ip
                /* var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipAdress = string.Empty;

                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAdress = ip.ToString();
                    }
                }

                return $"http://{ipAdress}/shop/{this.ImageUrl.Substring(1)}";*/

                return $"https://shopoz.azurewebsites.net/{this.ImageUrl.Substring(1)}";
            }
        }
    }

}
