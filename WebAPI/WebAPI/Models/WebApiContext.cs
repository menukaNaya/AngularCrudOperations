using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Entities;

namespace WebAPI.Models
{

    public class WebApiContext : DbContext
    {
        //public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee> Employee { get; set; }

        public WebApiContext(DbContextOptions<WebApiContext> options) : base(options)
        {

        }
    }
}
