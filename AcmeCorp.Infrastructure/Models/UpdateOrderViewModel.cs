using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Infrastructure.Models
{
    public class UpdateOrderViewModel
    {
        public int Id { get; set; }
        public long CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
