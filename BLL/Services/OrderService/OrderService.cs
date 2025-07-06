using BLL.Dtos.OrderDto;
using BLL.Dtos.OrderListDto;
using DAL.AppContext;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.OrderService
{
    public class OrderServices : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateOrderDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");

            var order = new Order
            {
                CustomerName = user.UserName,
                OrderDate = dto.OrderDate,
                UserId = dto.UserId,
                OrderList = new List<OrderList>()
            };

            var productIds = dto.OrderList.Select(x => x.ProductId).ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var item in dto.OrderList)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");

                order.OrderList.Add(new OrderList
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * item.Quantity
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadOrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderList)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new ReadOrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                OrderList = order.OrderList.Select(oi => new ReadOrderListDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };
        }

        public async Task<List<ReadOrderDto>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderList)
                .ThenInclude(oi => oi.Product)
                .Select(order => new ReadOrderDto
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    OrderDate = order.OrderDate,
                    UserId = order.UserId,
                    OrderList = order.OrderList.Select(oi => new ReadOrderListDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<bool> UpdateAsync(UpdateOrderDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderList)
                .FirstOrDefaultAsync(o => o.Id == dto.Id);

            if (order == null) return false;

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");

            order.CustomerName = user.UserName;
            order.OrderDate = dto.OrderDate;
            order.UserId = dto.UserId;

            _context.OrderLists.RemoveRange(order.OrderList);

            order.OrderList = new List<OrderList>();
            foreach (var item in dto.OrderList)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product {item.ProductId} not found");

                order.OrderList.Add(new OrderList
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * item.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
