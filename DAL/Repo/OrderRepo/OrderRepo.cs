using DAL.AppContext;
using DAL.Models;
using DAL.Repo.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.OrderRepo
{
    public class OrderRepo : GenericRepo<Order>, IGenericRepo<Order>
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
