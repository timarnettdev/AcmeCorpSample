using AcmeCorp.Domain;
using AcmeCorp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AcmeCorpDbContext _context;

        public UnitOfWork(AcmeCorpDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IGenericRepository<Customer> CustomerRepository => new GenericRepository<Customer>(_context);
        public IGenericRepository<Product> ProductRepository => new GenericRepository<Product>(_context);
        public IGenericRepository<Order> OrderRepository => new GenericRepository<Order>(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
