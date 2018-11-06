using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportStore.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage ="Prosze podać nazwę produktu.")]
        [Display(Name="Nazwa")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Prosze podać opis.")]
        [Display(Name="Opis")]
        public string Description { get; set; }
        [Required]
        [Range(0.01,double.MaxValue, ErrorMessage ="Prosze podac dodatnią cenę.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage ="Prosze określić kategorię.")]
        public string Category { get; set; }
    }
}
