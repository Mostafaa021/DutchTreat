using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderNumber { get; set; }
        public ICollection<OrderItem> ? Items { get; set; }
        public StoreUser  User { get; set; }
    }
}
