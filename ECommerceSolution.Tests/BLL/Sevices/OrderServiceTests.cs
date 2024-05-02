using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using Moq;
using NUnit.Framework;

    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IUserContext> _mockUserContext;
        private OrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUserContext = new Mock<IUserContext>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mockMapper.Object, _mockUserContext.Object);
        }

        [Test]
        public async Task GetOrderByIdAsync_ReturnsOrderDetailDTO()
        {
            var orderId = "testId";
            var order = new Order();
            var orderDetailDTO = new OrderDetailDTO();

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderDetailDTO>(order)).Returns(orderDetailDTO);

            var result = await _orderService.GetOrderByIdAsync(orderId);

            Assert.That(result, Is.EqualTo(orderDetailDTO));
        }

        [Test]
        public async Task GetAllOrdersAsync_WhenUserIsSeller_ReturnsOrderListDTOs()
        {
            var orders = new List<Order> { new Order(), new Order() };
            var orderListDTOs = new List<OrderListDTO> { new OrderListDTO(), new OrderListDTO() };

            _mockUserContext.SetupGet(uc => uc.CurrentUserRole).Returns("Seller");
            _mockOrderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<OrderListDTO>(It.IsAny<Order>())).Returns<Order>(o => orderListDTOs[orders.IndexOf(o)]);

            var result = await _orderService.GetAllOrdersAsync();

            Assert.That(result, Is.EquivalentTo(orderListDTOs));
        }

        [Test]
        public async Task CreateOrderAsync_ReturnsNewOrderDetailDTO()
        {
            var orderDTO = new OrderDTO();
            var order = new Order();
            var orderDetailDTO = new OrderDetailDTO();

            _mockMapper.Setup(m => m.Map<Order>(orderDTO)).Returns(order);
            _mockOrderRepository.Setup(repo => repo.CreateAsync(order)).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<OrderDetailDTO>(order)).Returns(orderDetailDTO);

            var result = await _orderService.CreateOrderAsync(orderDTO);

            Assert.That(result, Is.EqualTo(orderDetailDTO));
        }

        [Test]
        public async Task UpdateOrderAsync_WhenOrderExists_ReturnsTrue()
        {
            var id = "orderId";
            var orderDTO = new OrderDTO();
            var existingOrder = new Order();

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingOrder);
            _mockMapper.Setup(m => m.Map(orderDTO, existingOrder)).Returns(existingOrder);
            _mockOrderRepository.Setup(repo => repo.UpdateAsync(id, existingOrder)).ReturnsAsync(true);

            var result = await _orderService.UpdateOrderAsync(id, orderDTO);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteOrderAsync_WhenOrderExists_ReturnsTrue()
        {
            var id = "orderId";

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(new Order());
            _mockOrderRepository.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _orderService.DeleteOrderAsync(id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetFilteredOrdersAsync_ReturnsFilteredOrders()
        {
            var sellerId = "sellerId";
            var status = "status";
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(1);
            var orders = new List<Order> { new Order(), new Order() };

            _mockOrderRepository.Setup(repo => repo.GetFilteredOrdersAsync(sellerId, status, startDate, endDate)).ReturnsAsync(orders);

            var result = await _orderService.GetFilteredOrdersAsync(sellerId, status, startDate, endDate);

            Assert.That(result, Is.EquivalentTo(orders));
        }
    }

