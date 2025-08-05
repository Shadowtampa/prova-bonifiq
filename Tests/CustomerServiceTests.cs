using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Requests;
using System;
using System.Collections.Generic;
using ProvaPub.Repository;
using ProvaPub.Validators;

namespace ProvaPub.Tests
{
    [TestClass]
    public class CanPurchaseRequestValidatorTest
    {
        private TestDbContext _dbContext;
        private CanPurchaseRequestValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            _dbContext = new TestDbContext(options);

            _dbContext.Customers.RemoveRange(_dbContext.Customers);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.SaveChanges();

            var customerWithOrders = new Customer
            {
                Id = 1,
                Name = "ClienteComPedido",
                Orders = new List<Order>
                {
                    new Order {
                        Id = 1,
                        CustomerId = 1,
                         OrderDate = DateTime.UtcNow.AddDays(-10),
                         Value = 10
                          }
                }
            };

            var customerWithoutOrders = new Customer
            {
                Id = 2,
                Name = "ClienteSemPedido",
                Orders = new List<Order>() 
            };

            var customerWithOrdersThisMonth = new Customer
            {
                Id = 3,
                Name = "ClienteComPedido",
                Orders = new List<Order>
                {
                    new Order {
                        Id = 2,
                        CustomerId = 3,
                        OrderDate = DateTime.UtcNow,
                        Value = 50
                        }
                }
            };

            _dbContext.Customers.AddRange(customerWithOrders, customerWithoutOrders, customerWithOrdersThisMonth);
            _dbContext.SaveChanges();

            _validator = new Validators.CanPurchaseRequestValidator(_dbContext);
        }

        [TestMethod]
        public void ValidaRequest_ClienteID_MaiorQueZero()
        {
            var request = new CanPurchaseRequest
            {
                CustomerId = 0, // menor que zero
                PurchaseValue = 50
            };

            var result = _validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("O valor do ID deve ser maior que zero")));
        }

        [TestMethod]
        public void ValidaRequest_ClienteNaoExiste_DeveFalhar()
        {
            var request = new CanPurchaseRequest
            {
                CustomerId = 999, // não existe
                PurchaseValue = 50
            };

            var result = _validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("Cliente com ID 999 não existe.")));
        }

        [TestMethod]
        public void ValidaRequest_CompraMaiorQueZero()
        {
            var request = new CanPurchaseRequest
            {
                CustomerId = 1, // Cliente qualquer
                PurchaseValue = 0 // Compra não é maior que 0
            };

            var result = _validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("O valor da compra deve ser maior que zero.")));
        }

        [TestMethod]
        public void ValidaRequest_SomenteUmaCompraPorMes()
        {
            var request = new CanPurchaseRequest
            {
                CustomerId = 3, // Cliente com pedidos
                PurchaseValue = 150 // maior que 100 na primeira compra
            };

            var result = _validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("O cliente já efetuou uma compra este mês.")));
        }

        [TestMethod]
        public void ValidaRequest_CompraPrimeiraMaiorQue100_DeveFalhar()
        {
            var request = new CanPurchaseRequest
            {
                CustomerId = 2, // Cliente sem pedidos
                PurchaseValue = 150 // maior que 100 na primeira compra
            };

            var result = _validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("primeira compra")));
        }

        [TestMethod]
        public void ValidaRequest_CompraForaDoHorarioPermitido_DeveFalhar()
        {
            var fakeNow = new DateTime(2025, 8, 4, 22, 0, 0); // Segunda-feira, 22h

            // Criei um validator específico pra esse cara            
            var validator = new CustomTimeCanPurchaseRequestValidator(_dbContext, fakeNow);

            var request = new CanPurchaseRequest
            {
                CustomerId = 1,
                PurchaseValue = 50
            };

            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage.Contains("As compras só são permitidas durante dias da semana e hora útil")));
        }

    }

    public class CustomTimeCanPurchaseRequestValidator : CanPurchaseRequestValidator
    {
        private readonly DateTime _customNow;

        public CustomTimeCanPurchaseRequestValidator(TestDbContext dbContext, DateTime customNow)
            : base(dbContext)
        {
            _customNow = customNow;
        }

        protected override DateTime GetNow()
        {
            return _customNow;
        }
    }
}
