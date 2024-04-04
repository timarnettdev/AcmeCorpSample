using AcmeCore.Service.Services;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Models;
using AcmeCorp.Infrastructure.Repositories;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Test.Unit
{

    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsListOfProductViewModels()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10 },
                new Product { Id = 2, Name = "Product 2", Price = 20 }
            };

            var expectedViewModels = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Name = "Product 1", Price = 10 },
                new ProductViewModel { Id = 2, Name = "Product 2", Price = 20 }
            };

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetAllAsync()).ReturnsAsync(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ProductViewModel>>(products)).Returns(expectedViewModels);

            var result = await _productService.GetAllProductsAsync();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<ProductViewModel>>(result);
            CollectionAssert.AreEqual(expectedViewModels, (List<ProductViewModel>)result);
        }

        [Test]
        public async Task GetProductByIdAsync_WithValidId_ReturnsProductViewModel()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product 1", Price = 10 };
            var expectedViewModel = new ProductViewModel { Id = productId, Name = "Product 1", Price = 10 };

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductViewModel>(product)).Returns(expectedViewModel);

            var result = await _productService.GetProductByIdAsync(productId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [Test]
        public async Task CreateProductAsync_WithValidViewModel_ReturnsCreatedProductViewModel()
        {
            var productViewModel = new ProductViewModel { Name = "New Product", Price = 20 };
            var product = new Product { Id = 1, Name = "New Product", Price = 20 };
            var expectedViewModel = new ProductViewModel { Id = 1, Name = "New Product", Price = 20 };

            _mockMapper.Setup(mapper => mapper.Map<Product>(productViewModel)).Returns(product);
            _mockUnitOfWork.Setup(uow => uow.ProductRepository.AddAsync(product)).ReturnsAsync(product);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync());

            var result = await _productService.CreateProductAsync(productViewModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [Test]
        public async Task UpdateProductAsync_WithValidViewModel_ReturnsTask()
        {
            var productViewModel = new ProductViewModel { Id = 1, Name = "Updated Product", Price = 30 };
            var existingProduct = new Product { Id = 1, Name = "Old Product", Price = 20 };

            _mockMapper.Setup(mapper => mapper.Map<Product>(productViewModel)).Returns(existingProduct);
            _mockUnitOfWork.Setup(uow => uow.ProductRepository.UpdateAsync(existingProduct));
            _mockUnitOfWork.Setup(uow => uow.CommitAsync());

            Assert.DoesNotThrowAsync(async () => await _productService.UpdateProductAsync(productViewModel));
        }

        [Test]
        public async Task DeleteProductAsync_WithValidId_ReturnsTask()
        {
            var productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Product 1", Price = 10 };

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
            _mockUnitOfWork.Setup(uow => uow.ProductRepository.DeleteAsync(existingProduct));
            _mockUnitOfWork.Setup(uow => uow.CommitAsync());

            Assert.DoesNotThrowAsync(async () => await _productService.DeleteProductAsync(productId));
        }
    }
}
