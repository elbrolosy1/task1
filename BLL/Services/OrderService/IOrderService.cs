using BLL.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.OrderService
{
    public interface IOrderService
    {
        Task CreateAsync(CreateOrderDto dto);
        Task<ReadOrderDto?> GetByIdAsync(int id);
        Task<List<ReadOrderDto>> GetAllAsync();
        Task<bool> UpdateAsync(UpdateOrderDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
