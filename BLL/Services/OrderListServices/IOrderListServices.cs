using BLL.Dtos.OrderListDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.OrderListServices
{
        public interface IOrderListService
        {
            Task AddItemsAsync(int orderId, List<CreateOrderlistDto> items);
            Task<List<ReadOrderListDto>> GetItemsByOrderIdAsync(int orderId);
            Task<bool> UpdateItemAsync(int orderId, UpdateOrderListDto dto);
            Task<bool> DeleteItemAsync(int orderId, int productId);
        }

}
