using Advice_Me_APIs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AdviceMe.Admin.Controllers
{
    
    public class AdminProductsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminProductsController(IHttpClientFactory clientFactory)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var client = GetClientWithToken();
            var response = await client.GetAsync("products/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                return View(products);
            }

            return View(new List<Product>());
        }

        public async Task<IActionResult> Approve(int id)
        {
            var client = GetClientWithToken();
            await client.PostAsync($"products/{id}/approve", null);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = GetClientWithToken();
            await client.DeleteAsync($"products/{id}");
            return RedirectToAction("Index");
        }
    }
}
