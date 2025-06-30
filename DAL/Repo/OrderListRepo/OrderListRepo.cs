using DAL.AppContext;
using DAL.Models;
using DAL.Repo.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.OrderListRepo
{
   public class OrderListRepo : GenericRepo<OrderList>, IGenericRepo<OrderList>
   {
       private readonly ApplicationDbContext _dbContext;
       public OrderListRepo(ApplicationDbContext dbContext) : base(dbContext)
       {
           _dbContext = dbContext;
       }
   }
}
