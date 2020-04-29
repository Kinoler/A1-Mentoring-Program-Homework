using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MvcMusicStore.Models;
using MvcMusicStore.Service;
using NLog;
using PerformanceCounterHelper;

namespace MvcMusicStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private const string XsrfKey = "XsrfId";

        private UserManager<ApplicationUser> _userManager;

        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private async Task MigrateShoppingCart(string userName)
        {
            using (var storeContext = new MusicStoreEntities())
            {
                var cart = ShoppingCart.GetCart(storeContext, this);

                await cart.MigrateCart(userName);

                Session[ShoppingCart.CartSessionKey] = userName;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            logger.Info("Access to GET: /Account/Login");
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            logger.Info("Access to POST: /Account/Login");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    logger.Debug("User successfuly found.");
                    await SignInAsync(user, model.RememberMe);

                    using (var counterhelper =
                        PerformanceHelper.CreateCounterHelper<Counters>("App counters"))
                    {
                        counterhelper.Increment(Counters.LogIn);
                    }

                    return RedirectToLocal(returnUrl);
                }
                logger.Error("Invalid username or password.");
                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            logger.Info("Access to GET: /Account/Register");
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            logger.Info("Access to POST: /Account/Register");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    logger.Debug("User successfuly created.");

                    await SignInAsync(user, false);

                    using (var counterhelper =
                        PerformanceHelper.CreateCounterHelper<Counters>("App counters"))
                    {
                        counterhelper.Increment(Counters.Register);
                    }

                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            return View(model);
        }

        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            logger.Info("Access to POST: /Account/Disassociate");
            var result = await _userManager.RemoveLoginAsync(
                User.Identity.GetUserId(),
                new UserLoginInfo(loginProvider, providerKey));

            return RedirectToAction(
                "Manage",
                new { Message = result.Succeeded ? ManageMessageId.RemoveLoginSuccess : ManageMessageId.Error });
        }

        // GET: /Account/Manage
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            logger.Info("Access to GET: /Account/Manage");
            switch (message)
            {
                case ManageMessageId.ChangePasswordSuccess:
                    ViewBag.StatusMessage = "Your password has been changed.";
                    break;
                case ManageMessageId.SetPasswordSuccess:
                    ViewBag.StatusMessage = "Your password has been set.";
                    break;
                case ManageMessageId.RemoveLoginSuccess:
                    ViewBag.StatusMessage = "The external login was removed.";
                    break;
                case ManageMessageId.Error:
                    ViewBag.StatusMessage = "An error has occurred.";
                    break;
                default:
                    ViewBag.StatusMessage = "";
                    break;
            }
            logger.Debug(ViewBag.StatusMessage);
            ViewBag.HasLocalPassword = await HasPasswordAsync();
            ViewBag.ReturnUrl = Url.Action("Manage");

            return View();
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            logger.Info("Access to POST: /Account/Manage");
            bool hasPassword = await HasPasswordAsync();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    var result = await _userManager.ChangePasswordAsync(
                        User.Identity.GetUserId(),
                        model.OldPassword,
                        model.NewPassword);

                    if (result.Succeeded)
                    {
                        logger.Debug("The password successfuly changed.");
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }

                    AddErrors(result);
                }
            }
            else
            {
                var state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    var result = await _userManager.AddPasswordAsync(
                        User.Identity.GetUserId(),
                        model.NewPassword);

                    if (result.Succeeded)
                    {
                        logger.Debug("The password successfully added.");
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }

                    AddErrors(result);
                }
            }

            return View(model);
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            logger.Info("Access to POST: /Account/ExternalLogin");
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            logger.Info("Access to GET: /Account/ExternalLoginCallback");
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                logger.Debug("External login info is null.");
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                logger.Debug("User was successfully found.");
                await SignInAsync(user, false);

                return RedirectToLocal(returnUrl);
            }
            logger.Debug("User is null.");

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

            return View("ExternalLoginConfirmation",
                new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
        }

        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            logger.Info("Access to POST: /Account/LinkLogin");
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            logger.Info("Access to GET: /Account/LinkLoginCallback");
            var loginInfo =
                await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());

            if (loginInfo == null)
            {
                logger.Debug("External login info is null.");
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }

            var result = await _userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                logger.Debug("Login successfully.");
                return RedirectToAction("Manage");
            }

            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(
            ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            logger.Info("Access to POST: /Account/ExternalLoginConfirmation");
            if (User.Identity.IsAuthenticated)
            {
                logger.Debug("User is already authenticated.");
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.UserName };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);

                    if (result.Succeeded)
                    {
                        await SignInAsync(user, false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            logger.Info("Access to POST: /Account/LogOff");

            AuthenticationManager.SignOut();

            using (var counterhelper = 
                PerformanceHelper.CreateCounterHelper<Counters>("App counters"))
            {
                counterhelper.Increment(Counters.LogOut);
            }
            
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            logger.Info("Access to GET: /Account/ExternalLoginFailure");
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            logger.Info("Access to ");
            var linkedAccounts = _userManager.GetLogins(User.Identity.GetUserId());

            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;

            return PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity =
                await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);

            await MigrateShoppingCart(user.UserName);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                logger.Error(error);
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());

            return user != null && user.PasswordHash != null;
        }

        private async Task<bool> HasPasswordAsync()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());

            return user != null && user.PasswordHash != null;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                ? (ActionResult)Redirect(returnUrl)
                : RedirectToAction("Index", "Home");
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            private readonly string _loginProvider;
            private readonly string _redirectUri;
            private readonly string _userId;

            public ChallengeResult(string provider, string redirectUri, string userId = null)
            {
                _loginProvider = provider;
                _redirectUri = redirectUri;
                _userId = userId;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = _redirectUri };
                if (_userId != null)
                {
                    properties.Dictionary[XsrfKey] = _userId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, _loginProvider);
            }
        }
    }
}