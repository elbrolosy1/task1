﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.OrderListDto
{
    public class ReadOrderListDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
