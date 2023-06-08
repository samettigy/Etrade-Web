using Etrade.Entity.Models.Identity;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Etrade.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public IActionResult SignUp()
        {
            if (User.Identity.Name == null)
                return View();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var user = new User(model.Username)
            {
                Password = model.Password,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.Phone
            };
            var result = await userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            return View(model);
        }
        public IActionResult SignIn()
        {
            if (User.Identity.Name == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            User user;
            if (model.UsernameOrEmail.Contains("@"))
            {
                user = await userManager.FindByEmailAsync(model.UsernameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(model.UsernameOrEmail);
            }
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile(string name)
        {
            var user = await userManager.FindByNameAsync(name);
            return View(user);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.FindByIdAsync($"{id}");
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.FindByIdAsync($"{id}");
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            var user = await userManager.FindByIdAsync($"{model.UserName}");
            if (user == null)
            {
                user = new User(model.UserName)
                {
                    Id = model.Id,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };


                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
            }
            return View(model);
        }
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoterError = null)
        {
            if (remoterError != null)
            {
                //Hata Olduğunda
                return RedirectToAction("Login");
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //Google ile giriş bilgisi alınamadı.
                return RedirectToAction("Login");
            }
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (signInResult.IsLockedOut)
                return RedirectToAction("SignOut");
            else
            {
                //Kullanıcı google ile kayıt olmadıysa 
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new User { UserName = email, Email = email, Name = "user", Surname = "user",Password="yourpassword" };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: true);
                        return RedirectToLocal(returnUrl);
                    }
                }
                return RedirectToAction("Login");
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
