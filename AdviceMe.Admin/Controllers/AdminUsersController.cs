using Advice_Me_APIs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdviceMe.Admin.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminUsersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("Advice_Me_APIs");
            var response = await client.GetAsync("Users/GetAllUsers");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(json);
                return View(users);
            }
            return View(new List<User>());
        }
    }

}
