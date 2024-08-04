using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace ProductManagementAppAngular.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
