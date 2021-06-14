using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewMovieMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewMovieMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<MovieViewModel> movieList = new List<MovieViewModel>();

            using (var http = new HttpClient())
            {
                using (var response = await http.GetAsync("https://localhost:7001/api/movie/filmovi"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    movieList = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(apiResponse);
                }
            }

            return View(movieList);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovie(int id)
        {
            MovieViewModel movie = new MovieViewModel();

            using (var http = new HttpClient())
            {
                using (var response = await http.GetAsync("https://localhost:7001/api/movie/film/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    movie = JsonConvert.DeserializeObject<MovieViewModel>(apiResponse);
                }
            }

            return View(movie);
        }

        //[HttpPost]
        //public async Task<string> PostMovie(MovieViewModel model)
        //{
        //    var statusCode = string.Empty;

        //    using (var httpClient = new HttpClient())
        //    {
        //        HttpContent movie = new StringContent(model.ToString(), Encoding.UTF8, "application/json");
        //        using (var response = await httpClient.PostAsync("https://localhost:7001/api/movie/film/", movie))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                statusCode = response.StatusCode.ToString();
        //            }
        //        }
        //    }
        //    return statusCode;
        //}

        [HttpPost]
        public async Task<IActionResult> PostMovie(MovieViewModel model)
        {
            var statusCode = string.Empty;

            using (var httpClient = new HttpClient())
            {
                HttpContent movie = new StringContent(model.ToString(), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7001/api/movie/film/", movie))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        statusCode = response.StatusCode.ToString();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
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

        // Edit Method Get/Post and View

        // Delete Method Get/Post and View

        // Details Method Get and View
    }
}
