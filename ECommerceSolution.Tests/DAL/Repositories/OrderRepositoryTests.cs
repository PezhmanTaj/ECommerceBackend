using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using ECommerceSolution.DAL.Repositories;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

  [TestFixture]
    public class OrderRepositoryTests
    {
        private Mock<IMongoDBContext> _mockDbContext;
        private Mock<IMongoCollection<Order>> _mockOrderCollection;
        private Mock<IAsyncCursor<Order>> _mockCursor;
        private OrderRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockCursor = new Mock<IAsyncCursor<Order>>();
            _mockDbContext = new Mock<IMongoDBContext>();
            _mockOrderCollection = new Mock<IMongoCollection<Order>>();
            _mockDbContext.Setup(db => db.GetCollection<Order>("Orders")).Returns(_mockOrderCollection.Object);
            _repository = new OrderRepository(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = "1" }, new Order { Id = "2" } };
            var mockCursor = new Mock<IAsyncCursor<Order>>();
            mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
            mockCursor.Setup(_ => _.Current).Returns(orders);
            _mockOrderCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Order>>(),
                                               It.IsAny<FindOptions<Order, Order>>(),
                                               default))
                       .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateAsync_ReturnsTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var order = new Order { Id = "order1" };
            var replaceResultMock = new Mock<ReplaceOneResult>();
            replaceResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
            replaceResultMock.SetupGet(r => r.ModifiedCount).Returns(1);

            _mockOrderCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Order>>(),
                                                     It.IsAny<Order>(),
                                                     It.IsAny<ReplaceOptions>(),
                                                     default))
                       .ReturnsAsync(replaceResultMock.Object);

            // Act
            var result = await _repository.UpdateAsync(order.Id, order);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetFilteredOrdersAsync_FiltersBySellerId_ReturnsFilteredOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderOwnershipId = "seller1", Status = OrderStatus.Delivered, OrderDate = DateTime.Now.AddDays(-1) },
                new Order { OrderOwnershipId = "seller2", Status = OrderStatus.Pending, OrderDate = DateTime.Now.AddDays(-2) },
                new Order { OrderOwnershipId = "seller1", Status = OrderStatus.Delivered, OrderDate = DateTime.Now.AddDays(-3) }
            };

            var filterDefinition = Builders<Order>.Filter.Eq(order => order.OrderOwnershipId, "seller1");

            _mockCursor.Setup(_ => _.Current).Returns(orders.Where(o => o.OrderOwnershipId == "seller1").ToList());
            _mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                            .Returns(true)
                            .Returns(false);
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                            .Returns(Task.FromResult(true))
                            .Returns(Task.FromResult(false));

            _mockOrderCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Order>>(),
                                                       It.IsAny<FindOptions<Order, Order>>(),
                                                       It.IsAny<CancellationToken>()))
                                .ReturnsAsync(_mockCursor.Object);

            // Act
            var result = await _repository.GetFilteredOrdersAsync(sellerId: "seller1");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().OrderOwnershipId, Is.EqualTo("seller1"));
        }

        [Test]
        public async Task GetOrdersBySellerId_ReturnsOrdersForGivenSellerId()
        {
            // Arrange
            string sellerId = "sellerId";
            var expectedOrder = new List<Order> { new Order() { OrderOwnershipId = sellerId }, new Order() { OrderOwnershipId = sellerId } };
            _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                       .ReturnsAsync(true)
                       .ReturnsAsync(false);
            _mockCursor.Setup(_ => _.Current).Returns(expectedOrder);
            _mockOrderCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Order>>(),
                                                   It.IsAny<FindOptions<Order, Order>>(),
                                                   default))
                           .ReturnsAsync(_mockCursor.Object);

            // Act
            var orders = await _repository.GetOrdersBySellerId(sellerId);

            // Assert
            Assert.IsNotNull(orders);
            Assert.That(orders.Count(), Is.EqualTo(2));
            Assert.IsTrue(orders.All(o => o.OrderOwnershipId == sellerId));
        }
    }

