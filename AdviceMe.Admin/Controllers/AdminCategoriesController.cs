using Advice_Me_APIs.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AdviceMe.Admin.Controllers
{

    public class AdminCategoriesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminCategoriesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private HttpClient GetClientWithToken()
        {
            var client = _clientFactory.CreateClient("Advice_Me_APIs");
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<IActionResult> Index()
        {
            var client = GetClientWithToken();
            var response = await client.GetAsync("categories");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(json);
                return View(categories);
            }

            return View(new List<Category>());
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            var client = GetClientWithToken();
            var content = new StringContent(JsonConvert.SerializeObject(new { Name = name }), Encoding.UTF8, "application/json");
            await client.PostAsync("categories", content);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = GetClientWithToken();
            await client.DeleteAsync($"categories/{id}");
            return RedirectToAction("Index");
        }
    }
}
