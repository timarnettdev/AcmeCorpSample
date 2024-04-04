using AcmeCore.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCore.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
        Task<CustomerViewModel> GetCustomerByIdAsync(long customerId);
        Task<CustomerViewModel> CreateCustomerAsync(CustomerViewModel customerViewModel);
        Task UpdateCustomerAsync(CustomerViewModel customerViewModel);
        Task DeleteCustomerAsync(long customerId);
    }
}
