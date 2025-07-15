using Advice_Me_APIs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AdviceMe.Admin.Controllers
{
    public class AdminReviewsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AdminReviewsController(IHttpClientFactory clientFactory)
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
            var response = await client.GetAsync("reviews/pending");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var reviews = JsonConvert.DeserializeObject<List<Review>>(json);
                return View(reviews);
            }

            return View(new List<Review>());
        }

        public async Task<IActionResult> GetReviewByProductId(int productId)
        {
            var client = GetClientWithToken();
            var response = await client.GetAsync($"products/{productId}/reviews");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var reviews = JsonConvert.DeserializeObject<List<Review>>(json);
                return View(reviews);
            }

            return View(new List<Review>());
        }

        public async Task<IActionResult> Approve(int id)
        {
            var client = GetClientWithToken();
            await client.PostAsync($"reviews/{id}/approve", null);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var client = GetClientWithToken();
            await client.PostAsync($"reviews/{id}/reject", null);
            return RedirectToAction("Index");
        }
    }
}
