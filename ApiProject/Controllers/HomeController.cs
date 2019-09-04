using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiProject.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace ApiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string baseAddress = "http://www.omdbapi.com";
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<ActionResult<Movie>> SearchResults(string Title, string ReleaseYear, string Rated, string Language, string Runtime, string Genre)
        {
            var ApiKey = _configuration.GetSection("AppConfiguration")["APIKeyValue"];
            var client = new HttpClient();
            string searchAddress = "";
            if (Title != null)
            {
                searchAddress = $"?s={Title}&apikey={ApiKey}";
            }
            else if (ReleaseYear != null)
            {
                searchAddress = $"?y={ReleaseYear}&apikey={ApiKey}";
            }
            client.BaseAddress = new Uri("http://www.omdbapi.com");
            var response = await client.GetAsync(searchAddress);
            var something = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<Movies>(something);
            return View(movies);
        }
        public async Task<ActionResult<Movie>> AddToFavs()
        {
            
        }
    }
}
