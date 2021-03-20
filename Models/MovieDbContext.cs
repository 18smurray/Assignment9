using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment9.Models
{
    public class MovieDbContext : DbContext
    {
        //Inherit base options from the DbContext class
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        { }

        //Creating table in the database storing MovieResponse model objects
        public DbSet<MovieResponse> Movies { get; set; }
    }
}
