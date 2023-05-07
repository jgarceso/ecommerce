using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts)).Options;
            var productsDbContext = new ProductsDbContext(options);
            CreateProducts(productsDbContext);

            var productsProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg=>cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(productsDbContext, null, mapper);

            var productResult = await productsProvider.GetProductsAsync();
            Assert.Multiple(() =>
            {
                Assert.That(productResult.IsSuccess, Is.True);
                Assert.That(productResult.products.Any(), Is.True);
                Assert.That(productResult.ErrorMessage, Is.Null);
            });
        }
        [Test]
        public async Task GetProductsReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsProductUsingValidId)).Options;
            var productsDbContext = new ProductsDbContext(options);
            CreateProducts(productsDbContext);

            var productsProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(productsDbContext, null, mapper);

            var productResult = await productsProvider.GetProductAsync(1);
            Assert.Multiple(() =>
            {
                Assert.That(productResult.IsSuccess, Is.True);
                Assert.That(productResult.Product, Is.Not.Null);
                Assert.That(productResult.Product.Id == 1, Is.True);
                Assert.That(productResult.ErrorMessage, Is.Null);
            });
        }
        
        [Test]
        public async Task GetProductsReturnsProductUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsProductUsingValidId)).Options;
            var productsDbContext = new ProductsDbContext(options);
            CreateProducts(productsDbContext);

            var productsProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(productsDbContext, null, mapper);

            var productResult = await productsProvider.GetProductAsync(-1);
            Assert.Multiple(() =>
            {
                Assert.That(productResult.IsSuccess, Is.False);
                Assert.That(productResult.Product, Is.Null);
                Assert.That(productResult.ErrorMessage, Is.Not.Null);
            });
        }
        private void CreateProducts(ProductsDbContext productsDbContext)
        {
            for(int i = 1; i < 11; i++)
            {
                productsDbContext.Products.Add(new Product
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            productsDbContext.SaveChanges();

        }
    }
}