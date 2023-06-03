using System.ComponentModel.DataAnnotations;

namespace DutchTreat.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        [MinLength(4)]
        public string ? OrderNumber { get; set; }
        public ICollection<OrderItemDTO> ? Items { get; set; }

    }
}
