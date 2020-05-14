using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string PromoCode = "FREE";

        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        // GET: /Checkout/
        public ActionResult AddressAndPayment()
        {
            logger.Info("Access to GET: /Checkout/");
            return View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            logger.Info("Access to POST: /Checkout/AddressAndPayment");
            var order = new Order();
            TryUpdateModel(order);

            if (ModelState.IsValid 
                && string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase))
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                _storeContext.Orders.Add(order);

                await ShoppingCart.GetCart(_storeContext, this).CreateOrder(order);

                await _storeContext.SaveChangesAsync();

                logger.Debug("Card found");

                return RedirectToAction("Complete", new { id = order.OrderId });
            }

            return View(order);
        }

        // GET: /Checkout/Complete
        public async Task<ActionResult> Complete(int id)
        {
            logger.Info("Access to GET: /Checkout/Complete");
            return await _storeContext.Orders.AnyAsync(o => o.OrderId == id && o.Username == User.Identity.Name)
                ? View(id)
                : View("Error");
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