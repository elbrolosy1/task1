using BLL.Dtos.OrderListDto;
using DAL.AppContext;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.OrderListServices
{
    public class OrderListServices : IOrderListService
    {
        private readonly ApplicationDbContext _context;

        public OrderListServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddItemsAsync(int orderId, List<CreateOrderlistDto> items)
        {
            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product {item.ProductId} not found");

                var orderItem = new OrderList
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * item.Quantity
                };

                _context.Add(orderItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ReadOrderListDto>> GetItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderLists
                .Where(x => x.OrderId == orderId)
                .Include(x => x.Product)
                .Select(x => new ReadOrderListDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateItemAsync(int orderId, UpdateOrderListDto dto)
        {
            var item = await _context.OrderLists
                .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == dto.ProductId);

            if (item == null) return false;

            item.Quantity = dto.Quantity;
            item.TotalPrice = item.UnitPrice * dto.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(int orderId, int productId)
        {
            var item = await _context.OrderLists
                .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId);

            if (item == null) return false;

            _context.OrderLists.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
