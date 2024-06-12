using Microsoft.AspNetCore.Mvc;
using Mvc_consume.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Mvc_consume.Controllers
{
    public class DirectorsController : Controller
    {

        public static string GetAllurl = "https://localhost:7289/api/Directors/Get-All-Directors";
        public static string GetIdurl = "https://localhost:7289/api/Directors/Get-Director-By-Id/";
        public static string CreateUrl = "https://localhost:7289/api/Directors/Add-Director";
        public static string EditUrl = "https://localhost:7289/api/Directors/Update-Director-By-Id/";
        public static string DeleteUrl = "https://localhost:7289/api/Directors/Delete-Director-By-Id/";

		public async Task<IActionResult> Index()
		{
			var Directors = await GetDirectors();
			return View(Directors);
		}

		[HttpGet]
		public async Task<List<DirectorViewModel>> GetDirectors()
		{
			var accessToken = HttpContext.Session.GetString("JWToken");
			var url = GetAllurl;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			string jsonStr = await client.GetStringAsync(url);

			var res = JsonConvert.DeserializeObject<List<DirectorViewModel>>(jsonStr).ToList();

			return res;
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,Film")] AddDirectorVM addDirectorVM)
		{
			var accessToken = HttpContext.Session.GetString("JWToken");
			var url = CreateUrl;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var StringContent = new StringContent(JsonConvert.SerializeObject(addDirectorVM), Encoding.UTF8, "application/json");
			await client.PostAsync(url, StringContent);

			return RedirectToAction(nameof(Index));
		}



		public async Task<IActionResult> Details(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}
			var accesstoken = HttpContext.Session.GetString("JWToken");
			var url = GetIdurl + id;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
			string jsonStr = await client.GetStringAsync(url);
			var director = JsonConvert.DeserializeObject<DirectorViewModel>(jsonStr);

			if (director == null)
			{
				return NotFound();
			}
			return View(director);
		}
	}
}
