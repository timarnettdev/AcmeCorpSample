using AcmeCorp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCore.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderViewModel> GetOrderByIdAsync(long orderId);
        Task<OrderViewModel> CreateOrderAsync(CreateOrderViewModel createOrderViewModel);
        Task UpdateOrderAsync(UpdateOrderViewModel updateOrderViewModel);
        Task DeleteOrderAsync(long orderId);
    }
}
