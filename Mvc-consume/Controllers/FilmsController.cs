using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mvc_consume.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;


namespace Mvc_consume.Controllers
{
	
	public class FilmsController : Controller
	{

		public static string GetAllurl = "https://localhost:7289/api/Films/GetAllFilms/Get-All-Films";
		public static string GetIdurl = "https://localhost:7289/api/Films/GetFilmById/Get-Film-By-Id/";
		public static string CreateUrl = "https://localhost:7289/api/Films/AddFilm/Add-Film";
		public static string EditUrl = "https://localhost:7289/api/Films/UpdateFilmById/Update-Film-By-Id/{id}";
		public static string DeleteUrl = "https://localhost:7289/api/Films/DeleteFilmById/Delete-Film-By-Id/";

		private readonly IHttpClientFactory _httpclienttFactory;

		public FilmsController(IHttpClientFactory httpclienttFactory)
		{
			_httpclienttFactory = httpclienttFactory;
		}

		public async Task<IActionResult> Index()
		{
			var Films = await GetFilms();
			return View(Films);
		}

		[HttpGet]
		public async Task<List<FilmViewModel>> GetFilms()
		{
			var accessToken = HttpContext.Session.GetString("JWToken");
			var url = GetAllurl;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			string jsonStr = await client.GetStringAsync(url);

			var res = JsonConvert.DeserializeObject<List<FilmViewModel>>(jsonStr).ToList();

			return res;
		}


		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Description,Date,Genre,Directors,Actors")] AddFilmVM addFilmVM)
		{
			var accessToken = HttpContext.Session.GetString("JWToken");
			var url = CreateUrl;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var StringContent = new StringContent(JsonConvert.SerializeObject(addFilmVM), Encoding.UTF8, "application/json");
			await client.PostAsync(url, StringContent);

			return RedirectToAction(nameof(Index));
		}

		/*	public async Task<IActionResult> Edit(int? Id)
			{
				if (Id == null)
				{
					return NotFound();
				}
				var accessToken = HttpContext.Session.GetString("JWToken");
				var url = EditUrl + Id;
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

				string jsonStr = await client.GetStringAsync(url);
				var res = JsonConvert.DeserializeObject<EditFilmVM>(jsonStr);

				if (res == null)
				{
					return NotFound();
				}
				return View(res);
			}

		[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Edit(int Id , [Bind("Id,Name,Description,Date,Genre,Directors,Actors")] EditFilmVM editFilmVM )
			{
				if(Id != editFilmVM.Id)
				{
					return NotFound();
				}
				var accessToken = HttpContext.Session.GetString("JWToken");
				var url = EditUrl + Id;
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

				var StringContent = new StringContent(JsonConvert.SerializeObject(editFilmVM),Encoding.UTF8, "application/json");
				await client.PutAsync(url, StringContent);

				return RedirectToAction(nameof(Index));
			}*/

		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			try
			{
				var client = new HttpClient();
				var httpreponseMess = await client.DeleteAsync("https://localhost:7289/api/Films/DeleteFilmById/Delete-Film-By-Id" + id);
				httpreponseMess.EnsureSuccessStatusCode();
				return RedirectToAction("Index", "Films");
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
			}
			return View("Index");
		}

		/*public async Task<IActionResult> Delete(int? id)
		{
			if(id == null)
			{
				return NotFound();
			}
			var accessToken = HttpContext.Session.GetString("JWToken");
			var url = DeleteUrl + id;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			string jsonStr = await client.GetStringAsync(url);
			var res = JsonConvert.DeserializeObject<FilmViewModel>(jsonStr);

			if (res == null)
			{
				return NotFound();
			}
			return View(res);
		}

		[HttpPost , ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteComfirmed(int id)
		{
			var accessToken = HttpContext.Session.GetString("JWToken");
			var url = DeleteUrl + id;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			await client.DeleteAsync(url);
			return RedirectToAction(nameof (Index));
		}*/

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
			var film =	JsonConvert.DeserializeObject<FilmViewModel>(jsonStr);

			if (film == null)
			{
				return NotFound();
			}
			return View(film);
		}

		/*    public async Task<IActionResult> ListBook(int id)
			{
				BookViewModel reponse = new BookViewModel();
				try
				{
					var client = _httpclienttFactory.CreateClient();
					var HttpreponseMess = await client.GetAsync("https://localhost:7292/api/Book/GetBookById/get-book-by-id/" + id);
					HttpreponseMess.EnsureSuccessStatusCode();
					var stringReponseBody = await HttpreponseMess.Content.ReadAsStringAsync();
					reponse = await HttpreponseMess.Content.ReadFromJsonAsync<BookViewModel>();
				}
				catch (Exception ex)
				{
					ViewBag.Error = ex.Message;
				}
				return View(reponse);
			}*/

		[HttpGet]
		public async Task<IActionResult> Edit(int Id)
		{
			FilmViewModel reponseFilm = new FilmViewModel();
			var client = _httpclienttFactory.CreateClient();
			var httpreponseMess = await client.GetAsync("https://localhost:7289/api/Films/GetFilmById/Get-Film-By-Id/" + Id);
			httpreponseMess.EnsureSuccessStatusCode();
			reponseFilm = await httpreponseMess.Content.ReadFromJsonAsync<FilmViewModel>();
			ViewBag.Films = reponseFilm;

			List<ActorViewModel> reponseAc = new List<ActorViewModel>();
			var httpReponseAu = await client.GetAsync("https://localhost:7289/api/Actors/Get-All-Actors");
			httpReponseAu.EnsureSuccessStatusCode();
			reponseAc.AddRange(await httpReponseAu.Content.ReadFromJsonAsync<IEnumerable<ActorViewModel>>());
			ViewBag.Actors = reponseAc;

			List<DirectorViewModel> reponseDi = new List<DirectorViewModel>();
			var httpReponsePu = await client.GetAsync("https://localhost:7289/api/Directors/Get-All-Directors");
			httpReponsePu.EnsureSuccessStatusCode();
			reponseDi.AddRange(await httpReponsePu.Content.ReadFromJsonAsync<IEnumerable<DirectorViewModel>>());
			ViewBag.Directors = reponseDi;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Edit([FromRoute] int Id, EditFilmVM editFilmVM)
		{
			try
			{
				var client = _httpclienttFactory.CreateClient();
				var httpRequestMess = new HttpRequestMessage()
				{
					Method = HttpMethod.Put,
					RequestUri = new Uri("https://localhost:7289/api/Films/GetFilmById/Get-Film-By-Id/" + Id),
					Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(editFilmVM), Encoding.UTF8,
						MediaTypeNames.Application.Json)
				};

				var httpReponseMess = await client.SendAsync(httpRequestMess);
				httpReponseMess.EnsureSuccessStatusCode();
				var reponse = await httpReponseMess.Content.ReadFromJsonAsync<AddFilmVM>();
				if (reponse != null)
				{
					return RedirectToAction("Index", "Films");
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
			}
			return View();
		}


	}
}
