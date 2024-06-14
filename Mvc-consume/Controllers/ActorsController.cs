using Microsoft.AspNetCore.Mvc;
using Mvc_consume.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Mvc_consume.Controllers
{
    public class ActorsController : Controller
    {

        public static string GetAllurl = "https://localhost:7289/api/Actors/Get-All-Actors";
        public static string GetIdurl = "https://localhost:7289/api/Actors/Get-Actor-By-Id/";
        public static string CreateUrl = "https://localhost:7289/api/Actors/Add-Actor";
        public static string EditUrl = "https://localhost:7289/api/Actors/Update-Actor-By-Id/";
        public static string DeleteUrl = "https://localhost:7289/api/Actors/Delete-Actor-By-Id/";


        private readonly IHttpClientFactory _httpclienttFactory;

        public ActorsController(IHttpClientFactory httpclienttFactory)
        {
            _httpclienttFactory = httpclienttFactory;
        }

        public async Task<IActionResult> Index()
        {
            var Actors = await GetActors();
            return View(Actors);
        }

        [HttpGet]
        public async Task<List<ActorViewModel>> GetActors()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = GetAllurl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<List<ActorViewModel>>(jsonStr).ToList();

            return res;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,Film")] AddActorVm addActorVM)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = CreateUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var StringContent = new StringContent(JsonConvert.SerializeObject(addActorVM), Encoding.UTF8, "application/json");
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
            var actor = JsonConvert.DeserializeObject<ActorViewModel>(jsonStr);

            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ActorViewModel reponseActor = new ActorViewModel();
            var client = _httpclienttFactory.CreateClient();
            var httpreponseMess = await client.GetAsync("https://localhost:7289/api/Actors/Get-Actor-By-Id/" + Id);
            httpreponseMess.EnsureSuccessStatusCode();
            reponseActor = await httpreponseMess.Content.ReadFromJsonAsync<ActorViewModel>();
            ViewBag.Actors = reponseActor;

        /*    FilmViewModel reponseFilm = new FilmViewModel();
            var httpreponseFilm = await client.GetAsync("https://localhost:7289/api/Films/GetFilmById/Get-Film-By-Id/" + Id);
            httpreponseFilm.EnsureSuccessStatusCode();
            reponseFilm = await httpreponseFilm.Content.ReadFromJsonAsync<FilmViewModel>();
            ViewBag.Films = reponseFilm;*/

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int Id, EditActorVM actorFilmVM)
        {
            try
            {
                var client = _httpclienttFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7289/api/Actors/Update-Actor-By-Id/" + Id),
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(actorFilmVM), Encoding.UTF8,
                        MediaTypeNames.Application.Json)
                };

                var httpReponseMess = await client.SendAsync(httpRequestMess);
                httpReponseMess.EnsureSuccessStatusCode();
                var reponse = await httpReponseMess.Content.ReadFromJsonAsync<AddActorVm>();
                if (reponse != null)
                {
                    return RedirectToAction("Index", "Actors");
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
                var httpreponseMess = await client.DeleteAsync("https://localhost:7289/api/Actors/Delete-Actor-By-Id/" + id);
                httpreponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Actors");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }
    }
}
