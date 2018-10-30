using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [Required(ErrorMessage = "Prosze podać imie i nazwisko.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Prosze podać pierwszy wiersz adresu.")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Prosze podać nazwę miasta.")]
        public string City { get; set; }

        [Required(ErrorMessage ="Prosze podać nazwę województwa")]
        public string State { get; set; }
        public string Zip { get; set; }

        [Required(ErrorMessage = "Prosze podać nazwę kraju.")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}
