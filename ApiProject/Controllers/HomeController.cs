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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly APIMovieDbContext _context;
        private readonly IConfiguration _configuration;
        public HomeController(APIMovieDbContext context, IConfiguration configuration)
        {
            _context = context;
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
        public async Task<ActionResult<Movie>> AddToFavs(string imdbID)
        {
            bool found = false;
            List<Favorites> favoritesList = _context.Favorites.ToList();
            int i = 0;
            //id is the current user's id
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            while (!found)
            {
                //if we find the user's id in the favorites list
                if (favoritesList[i].UserId == id)
                {
                    //pull the movie id string from the found favorite
                    string favoriteMovies = favoritesList[i].MovieId;
                    //break out of the loop
                    found = true;
                }
                else
                //otherwise, move to the next favorite
                { i++; }
            }
        }
        public async Task<ActionResult<Movie>> GetMovieById(string imdbID)
        {
            var ApiKey = _configuration.GetSection("AppConfiguration")["APIKeyValue"];
            var client = new HttpClient();
            var response = await client.GetAsync("http://www.omdbapi.com/?i={imdbID}&apikey={ApiKey}");
            var jsonObject = await response.Content.ReadAsStringAsync();
            Movie returnedMovie = JsonConvert.DeserializeObject<Movie>(jsonObject);
            return returnedMovie;
        }
    }
}
