using AcmeCore.Service.Services.Interfaces;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Repositories;
using AutoMapper;
using AcmeCore.Infrastructure.Models;

namespace AcmeCore.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
        }

        public async Task<CustomerViewModel> GetCustomerByIdAsync(long customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            return _mapper.Map<CustomerViewModel>(customer);
        }

        public async Task<CustomerViewModel> CreateCustomerAsync(CustomerViewModel customerViewModel)
        {
            var customer = _mapper.Map<Customer>(customerViewModel);
            var addedCustomer = await _unitOfWork.CustomerRepository.AddAsync(customer);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CustomerViewModel>(addedCustomer);
        }

        public async Task UpdateCustomerAsync(CustomerViewModel customerViewModel)
        {
            var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerViewModel.Id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerViewModel.Id} not found.");
            }

            _mapper.Map(customerViewModel, existingCustomer);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCustomerAsync(long customerId)
        {
            var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            await _unitOfWork.CustomerRepository.DeleteAsync(existingCustomer);
            await _unitOfWork.CommitAsync();
        }
    }
}
