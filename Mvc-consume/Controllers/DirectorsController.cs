using Microsoft.AspNetCore.Mvc;
using Mvc_consume.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
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


		private readonly IHttpClientFactory _httpclienttFactory;

		public DirectorsController(IHttpClientFactory httpclienttFactory)
		{
			_httpclienttFactory = httpclienttFactory;
		}
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


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            DirectorViewModel reponseDirec = new DirectorViewModel();
            var client = _httpclienttFactory.CreateClient();
            var httpreponseMess = await client.GetAsync("https://localhost:7289/api/Directors/Get-Director-By-Id/" + Id);
            httpreponseMess.EnsureSuccessStatusCode();
            reponseDirec = await httpreponseMess.Content.ReadFromJsonAsync<DirectorViewModel>();
            ViewBag.Directors = reponseDirec;

       /*     FilmViewModel reponseFilm = new FilmViewModel();
            var httpreponseFilm = await client.GetAsync("https://localhost:7289/api/Films/GetFilmById/Get-Film-By-Id/" + Id);
            httpreponseFilm.EnsureSuccessStatusCode();
            reponseFilm = await httpreponseFilm.Content.ReadFromJsonAsync<FilmViewModel>();
            ViewBag.Films = reponseFilm;*/

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int Id, EditDirectorVM direcFilmVM)
        {
            try
            {
                var client = _httpclienttFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7289/api/Directors/Update-Director-By-Id/" + Id),
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(direcFilmVM), Encoding.UTF8,
                        MediaTypeNames.Application.Json)
                };

                var httpReponseMess = await client.SendAsync(httpRequestMess);
                httpReponseMess.EnsureSuccessStatusCode();
                var reponse = await httpReponseMess.Content.ReadFromJsonAsync<AddDirectorVM>();
                if (reponse != null)
                {
                    return RedirectToAction("Index", "Directors");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var client = new HttpClient();
                var httpreponseMess = await client.DeleteAsync("https://localhost:7289/api/Directors/Delete-Director-By-Id/" + id);
                httpreponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Directors");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }
    }
}
