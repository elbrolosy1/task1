using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public int UserId { get; set; } 
        public AppUser AppUser { get; set; }

        public ICollection<OrderList> OrderList { get; set; } = new List<OrderList>();

    }
}
