using DAL.AppContext;
using DAL.Models;
using DAL.Repo.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.AppUserRepo
{
    public class AppUserRepo:GenericRepo<AppUser>,IGenericRepo<AppUser>
    {
        private readonly ApplicationDbContext _dbContext;
        public AppUserRepo(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
