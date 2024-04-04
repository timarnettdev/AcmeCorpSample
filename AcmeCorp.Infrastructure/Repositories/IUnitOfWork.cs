using AcmeCorp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Customer> CustomerRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }

        Task<int> CommitAsync();
    }
}
