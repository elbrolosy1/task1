using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string imgUrl { get; set; }
        public string Description { get; set; }

        public ICollection<OrderList> OrderList { get; set; } = new List<OrderList>();
    }
}
