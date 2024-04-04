using AcmeCore.Service.Services.Interfaces;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Models;
using AcmeCorp.Infrastructure.Repositories;
using AutoMapper;

namespace AcmeCore.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderViewModel>>(orders);
        }

        public async Task<OrderViewModel> GetOrderByIdAsync(long orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            var orderViewModel = _mapper.Map<OrderViewModel>(order);

            return orderViewModel;
        }

        public async Task<OrderViewModel> CreateOrderAsync(CreateOrderViewModel createOrderViewModel)
        {
            var order = _mapper.Map<Order>(createOrderViewModel);
            var addedOrder = await _unitOfWork.OrderRepository.AddAsync(order);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrderViewModel>(addedOrder);
        }

        public async Task UpdateOrderAsync(UpdateOrderViewModel updateOrderViewModel)
        {
            var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(updateOrderViewModel.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {updateOrderViewModel.Id} not found.");
            }

            _mapper.Map(updateOrderViewModel, existingOrder);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteOrderAsync(long orderId)
        {
            var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            await _unitOfWork.OrderRepository.DeleteAsync(existingOrder);
            await _unitOfWork.CommitAsync();
        }
    }
}
