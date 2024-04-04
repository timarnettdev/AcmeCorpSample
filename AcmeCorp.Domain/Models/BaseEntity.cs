using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Models
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }

        // Constructor to initialize CreatedOn with current date/time
        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }
}
