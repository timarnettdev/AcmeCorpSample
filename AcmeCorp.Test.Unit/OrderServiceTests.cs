using AcmeCore.Service.Services;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Models;
using AcmeCorp.Infrastructure.Repositories;
using AutoMapper;
using Castle.Core.Resource;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Test.Unit
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _orderService = new OrderService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllOrdersAsync_ReturnsListOfOrderViewModels()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, OrderDate = DateTime.Now, TotalAmount = 100 },
                new Order { Id = 2, OrderDate = DateTime.Now, TotalAmount = 200 }
            };

            var expectedViewModels = new List<OrderViewModel>
            {
                new OrderViewModel { Id = 1, OrderDate = DateTime.Now, TotalAmount = 100 },
                new OrderViewModel { Id = 2, OrderDate = DateTime.Now, TotalAmount = 200 }
            };

            _mockUnitOfWork.Setup(uow => uow.OrderRepository.GetAllAsync()).ReturnsAsync(orders);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderViewModel>>(orders)).Returns(expectedViewModels);

            var result = await _orderService.GetAllOrdersAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<OrderViewModel>>(result);
            CollectionAssert.AreEqual(expectedViewModels, (List<OrderViewModel>)result);
        }

        [Test]
        public async Task GetOrderByIdAsync_WithValidId_ReturnsOrderViewModel()
        {
            var orderId = 1;
            var order = new Order { Id = orderId, OrderDate = DateTime.Now, TotalAmount = 100 };
            var expectedViewModel = new OrderViewModel { Id = orderId, OrderDate = DateTime.Now, TotalAmount = 100 };

            _mockUnitOfWork.Setup(uow => uow.OrderRepository.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockMapper.Setup(mapper => mapper.Map<OrderViewModel>(order)).Returns(expectedViewModel);

            var result = await _orderService.GetOrderByIdAsync(orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [Test]
        public async Task CreateOrderAsync_WithValidViewModel_ReturnsCreatedOrderViewModel()
        {
            // Arrange
            var createOrderViewModel = new CreateOrderViewModel { CustomerId = 1, OrderDate = DateTime.Now.AddDays(-25), TotalAmount = 215 };
            var order = new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-25), TotalAmount = 215 };
            var expectedViewModel = new OrderViewModel { Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-35), TotalAmount = 124 };

            _mockMapper.Setup(mapper => mapper.Map<Order>(createOrderViewModel)).Returns(order);
            _mockUnitOfWork.Setup(uow => uow.OrderRepository.AddAsync(order)).ReturnsAsync(order);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync());

            // Act
            var result = await _orderService.CreateOrderAsync(createOrderViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [Test]
        public async Task UpdateOrderAsync_WithValidViewModel_ReturnsTask()
        {
            // Arrange
            var updateOrderViewModel = new UpdateOrderViewModel { Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-35), TotalAmount = 124 };
            var existingOrder = new Order { Id = 1, CustomerId=1, OrderDate=DateTime.Now.AddDays(-25), TotalAmount=215 };

            _mockMapper.Setup(mapper => mapper.Map<Order>(updateOrderViewModel)).Returns(existingOrder);
            _mockUnitOfWork.Setup(uow => uow.OrderRepository.UpdateAsync(existingOrder));
            _mockUnitOfWork.Setup(uow => uow.CommitAsync());

            Assert.DoesNotThrowAsync(async () => await _orderService.UpdateOrderAsync(updateOrderViewModel));
        }

        [Test]
        public async Task DeleteOrderAsync_WithValidId_ReturnsTask()
        {
            var orderId = 1;
            var existingOrder = new Order { Id = 1, CustomerId=1, OrderDate=DateTime.Now.AddDays(-25), TotalAmount=215 };

            _mockUnitOfWork.Setup(uow => uow.OrderRepository.GetByIdAsync(orderId)).ReturnsAsync(existingOrder);
            _mockUnitOfWork.Setup(uow => uow.OrderRepository.DeleteAsync(existingOrder));
            _mockUnitOfWork.Setup(uow => uow.CommitAsync());

            Assert.DoesNotThrowAsync(async () => await _orderService.DeleteOrderAsync(orderId));
        }
    }
}
