using Assignment9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment9.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //Variable for the DbContext class
        private MovieDbContext context { get; set; }

        public HomeController(ILogger<HomeController> logger, MovieDbContext con)
        {
            _logger = logger;
            context = con;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Action for first navigating to the enter movie page
        [HttpGet]
        public IActionResult EnterMovie()
        {
            return View();
        }


        //Action for receiving movie inputs from the user to create new database entry
        [HttpPost]
        public IActionResult EnterMovie(MovieResponse mr)
        {
            //Check to ensure there have been no validation errors
            if (ModelState.IsValid)
            {
                //Use table name Movies - add model information to database
                context.Movies.Add(mr);

                //Save changes to the DB
                context.SaveChanges();

                //Route to view movies page; exclude Independence Day from being displayed
                return View("ViewMovies", context.Movies.Where(m => m.Title != "Independence Day"));
            }

            //If model is invalid, return to same page - show validation violations
            return View();
        }

        //Page for returning all movies in database (except those with the title of independence day)
        public IActionResult ViewMovies()
        {
            //Returns the view and passes the enumerated MovieList of MovieResponse model objects to the view
            return View(context.Movies.Where(m => m.Title != "Independence Day"));
        }

        //Action for returning the podcast page
        public IActionResult Podcast()
        {
            return View();
        }


        //Action for getting to the edit movie page and passing the correct movie data
        [HttpPost]
        public IActionResult EditMovie(int movieid)
        {
            //Gets the movierepsonse model object that corresponds with the id passed in
            var MovieToEdit = context.Movies.Where(mv => mv.MovieId == movieid).FirstOrDefault();

            //Returns the editmovie page and passes the movie object - to be used for populating the form default values
            //with the current values 
            return View(MovieToEdit);
        }

        //Action for receiving user changes from form and updating the appropriate movie 
        [HttpPost]
        public IActionResult SaveChanges(MovieResponse mr, int movieid)
        {
            //Identify which movie corresponds to the id passed in 
            var MovieToOverwrite = context.Movies.Where(mr => mr.MovieId == movieid).FirstOrDefault();

            //Ensure form inputs are valid
            if (ModelState.IsValid)
            {
                //Update the values of the movie corresponding with the movieid passed in
                MovieToOverwrite.Title = mr.Title;
                MovieToOverwrite.Category = mr.Category;
                MovieToOverwrite.Year = mr.Year;
                MovieToOverwrite.Director = mr.Director;
                MovieToOverwrite.Rating = mr.Rating;
                MovieToOverwrite.Edited = mr.Edited;
                MovieToOverwrite.LentTo = mr.LentTo;
                MovieToOverwrite.Notes = mr.Notes;

                //Save changes to the DB
                context.SaveChanges();

                //Route to view movies page; exclude Independence Day from being displayed
                return View("ViewMovies", context.Movies.Where(m => m.Title != "Independence Day"));
            }

            //If model is invalid, return to same page - show validation violations
            //Not sure how to repass original movie data...
            return View("EditMovie", mr);
        }

        //Action for deleting movie from the database
        [HttpPost]
        public IActionResult DeleteMovie(int movieid)
        {
            //identify the movie in the database that corresponds to the id passed in 
            var MovieToDelete = context.Movies.Where(mv => mv.MovieId == movieid).FirstOrDefault();

            //Remove the identified movie and save the change
            context.Remove(MovieToDelete);
            context.SaveChanges();

            //Go back to the ViewMovies page
            return View("ViewMovies", context.Movies.Where(m => m.Title != "Independence Day"));
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
