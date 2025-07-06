using BLL.Dtos.OrderListDto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.OrderDto
{
    public class CreateOrderDto
    {
        public DateTime OrderDate { get; set; }

        public int UserId { get; set; }

        public List<CreateOrderlistDto> OrderList { get; set; } = new List<CreateOrderlistDto>();

        public IEnumerable<SelectListItem>? Users { get; set; }

    }
}
