using Microsoft.AspNetCore.Mvc;
using Mvc_consume.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Mvc_consume.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> LoginUser(UserInfo user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8, "application/json");
                using  (var reponse = await httpClient.PostAsync("https://localhost:7289/api/User/Login", stringContent))
                {
                    string token = await reponse.Content.ReadAsStringAsync();
                    if(token == "Invalid credentials")
                    {
                        ViewBag.Message = "Incorrect USerName or PassWord";
                        return Redirect("~/Home/index");
                    }
                    HttpContext.Session.SetString("JWToken", token);
                }
                return Redirect("~/Dashboard/Index");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Home/Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
