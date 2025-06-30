using BLL.Dtos.OrderListDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.OrderDto
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public int UserId { get; set; }

        public List<CreateOrderlistDto> OrderList { get; set; } = new List<CreateOrderlistDto>();

    }
}
