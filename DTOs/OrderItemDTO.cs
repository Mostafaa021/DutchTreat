using DutchTreat.Models;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.DTOs
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        [Required]
        public int OrderItemQuantity { get; set; }
        [Required]
        public decimal OrderItemUnitPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductSize { get; set; }
        public string? ProductTitle { get; set; }
        public string? ProductArtist { get; set; }
        public string? ProductArtId { get; set; }

    }
}