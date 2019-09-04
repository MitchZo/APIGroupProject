using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiProject.Models;
using System.Net.Http;

namespace ApiProject.Controllers
{
    public class HomeController : Controller
    {
        public string baseAddress = "http://www.omdbapi.com";
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<ActionResult<Movie>> SearchResults(string Title, string ReleaseYear, string Rated, string Language, string Runtime, string Genre)
        {

            return View();
        }
    }
}
