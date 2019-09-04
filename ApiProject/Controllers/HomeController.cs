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
            List<Favorites> favoritesList = _context.Favorites.ToList();
            int i = 0;
            //id is the current user's id
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Favorites fav = new Favorites();
            fav.MovieId = imdbID;
            fav.UserId = id;
            _context.Favorites.Add(fav);
            _context.SaveChanges();
            var results = await GetMovieById(imdbID);
            return View(results);
        }

        public async Task<ActionResult<List<Movie>>> FavoritesList()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var somethingelse = _context.Favorites.Where(u => u.UserId == id).ToList();

            List<Movie> userMovieList = new List<Movie>();
            foreach (Favorites movie in somethingelse)
            {
                var currentMovie = await GetMovieById(movie.MovieId);
                userMovieList.Add(currentMovie);
            }

            return View(userMovieList);

        }
        public async Task<Movie> GetMovieById(string imdbID)
        {
            var ApiKey = _configuration.GetSection("AppConfiguration")["APIKeyValue"];
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.omdbapi.com");
            var response = await client.GetAsync($"?i={imdbID}&apikey={ApiKey}");
            var result = await response.Content.ReadAsAsync<Movie>();
            return result;
        }
    }
}
