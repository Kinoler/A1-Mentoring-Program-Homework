using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        // GET: /Home/
        public async Task<ActionResult> Index()
        {
            logger.Info("Access to GET: /Home/");
            return View(await _storeContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(6)
                .ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}