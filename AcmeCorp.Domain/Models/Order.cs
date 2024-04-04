using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Models
{
    public class Order : BaseEntity<long>
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
