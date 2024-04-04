using AcmeCore.Infrastructure.Models;
using AcmeCore.Service.Services;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Repositories;
using AutoMapper;
using Moq;

namespace AcmeCorp.Test.Unit
{
    [TestFixture]
    public class Tests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private Mock<IGenericRepository<Customer>> mockCustomerRepository;
        private CustomerService customerService;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();
            mockCustomerRepository = new Mock<IGenericRepository<Customer>>();

            mockUnitOfWork.Setup(uow => uow.CustomerRepository).Returns(mockCustomerRepository.Object);

            customerService = new CustomerService(mockUnitOfWork.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetAllCustomersAsync()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Customer 1", Email = "customer1@example.com" },
                new Customer { Id = 2, Name = "Customer 2", Email = "customer2@example.com" }
            };

            mockCustomerRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            var expectedViewModels = new List<CustomerViewModel>
            {
                new CustomerViewModel { Id = 1, Name = "Customer 1", Email = "customer1@example.com" },
                new CustomerViewModel { Id = 2, Name = "Customer 2", Email = "customer2@example.com" }
            };
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<CustomerViewModel>>(customers)).Returns(expectedViewModels);

            var result = await customerService.GetAllCustomersAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<CustomerViewModel>>(result);
            CollectionAssert.AreEquivalent(expectedViewModels, (List<CustomerViewModel>)result);
        }

        [Test]
        public async Task GetCustomerByIdAsync_ReturnsCustomerViewModel()
        {
            var customer = new Customer { Id = 1, Name = "Customer 1", Email = "customer1@example.com" };

            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            var expectedViewModel = new CustomerViewModel { Id = customer.Id, Name = customer.Name, Email = customer.Email };
            mockMapper.Setup(mapper => mapper.Map<CustomerViewModel>(customer)).Returns(expectedViewModel);

            var result = await customerService.GetCustomerByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [Test]
        public async Task CreateCustomerAsync()
        {
            var createCustomerViewModel = new CustomerViewModel { Name = "New Customer", Email = "newcustomer@example.com" };
            var customer = new Customer { Id = 1, Name = createCustomerViewModel.Name, Email = createCustomerViewModel.Email };

            mockCustomerRepository.Setup(repo => repo.AddAsync(It.IsAny<Customer>())).ReturnsAsync(customer);

            mockMapper.Setup(mapper => mapper.Map<Customer>(createCustomerViewModel)).Returns(customer);

            var expectedViewModel = new CustomerViewModel { Id = customer.Id, Name = customer.Name, Email = customer.Email };
            mockMapper.Setup(mapper => mapper.Map<CustomerViewModel>(customer)).Returns(expectedViewModel);

            var result = await customerService.CreateCustomerAsync(createCustomerViewModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [Test]
        public async Task UpdateCustomerAsync()
        {
            var updateCustomerViewModel = new CustomerViewModel { Id = 1, Name = "Updated Customer", Email = "updatedcustomer@example.com" };
            var existingCustomer = new Customer { Id = 1, Name = "Customer", Email = "customer@example.com" };

            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCustomer);

            mockMapper.Setup(mapper => mapper.Map(updateCustomerViewModel, existingCustomer)).Returns(existingCustomer);

            await customerService.UpdateCustomerAsync(updateCustomerViewModel);

            mockCustomerRepository.Verify(repo => repo.UpdateAsync(existingCustomer), Times.Never);
        }
    }
}