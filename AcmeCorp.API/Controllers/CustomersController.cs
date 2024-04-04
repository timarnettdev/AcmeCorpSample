using AcmeCore.Infrastructure.Models;
using AcmeCore.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetAllCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }
        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomerByIdAsync(long customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerViewModel>> CreateCustomerAsync(CustomerViewModel customerViewModel)
        {
            var createdCustomer = await _customerService.CreateCustomerAsync(customerViewModel);
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { customerId = createdCustomer.Id }, createdCustomer);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomerAsync(long customerId, CustomerViewModel customerViewModel)
        {
            if (customerId != customerViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                await _customerService.UpdateCustomerAsync(customerViewModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomerAsync(long customerId)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(customerId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
