using AcmeCore.Service.Services.Interfaces;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Models;
using AcmeCorp.Infrastructure.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCore.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetProductByIdAsync(long productId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> CreateProductAsync(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);
            var addedProduct = await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProductViewModel>(addedProduct);
        }

        public async Task UpdateProductAsync(ProductViewModel productViewModel)
        {
            var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productViewModel.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productViewModel.Id} not found.");
            }

            _mapper.Map(productViewModel, existingProduct);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProductAsync(long productId)
        {
            var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            await _unitOfWork.ProductRepository.DeleteAsync(existingProduct);
            await _unitOfWork.CommitAsync();
        }
    }
}
