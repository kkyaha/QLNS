using Microsoft.AspNetCore.Mvc;
using QLNS.Models;

namespace QLNS.Controllers
{
    public class LuongController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public LuongController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
