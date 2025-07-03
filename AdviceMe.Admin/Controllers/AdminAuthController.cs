using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace AdviceMe.Admin.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminAuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _clientFactory.CreateClient("Advice_Me_APIs");

            var loginData = new
            {
                Email = email,
                Password = password
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("auth/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JwtToken", token);

                // ✅ بعد نجاح الدخول، نرسل token ونجلب بيانات المستخدم
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userResponse = await client.GetAsync("auth/me"); // API يجب أن يوفر هذا

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    dynamic user = JsonConvert.DeserializeObject(userJson);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    string role = user.role ?? "User";

                    // ✅ فقط Admin يمكنه الدخول للوحة التحكم
                    if (role != "Admin")
                    {
                        return RedirectToAction("AccessDenied");
                    }

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };

                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("Cookies", principal);

                    return RedirectToAction("Index", "AdminProducts");
                }

                ViewBag.Error = "Unable to retrieve user info.";
                return View();
            }

            ViewBag.Error = "Login failed. Please check your email or password.";
            return View();
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            HttpContext.SignOutAsync("Cookies").Wait(); // Ensure the user is signed out
            return RedirectToAction("Login");
        }
    }
}
