using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Infrastructure.Models
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Added OrderId property
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
