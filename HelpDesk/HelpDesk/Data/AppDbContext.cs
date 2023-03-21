using System;
using System.Diagnostics;
using System.Reflection.Emit;
using HelpDesk.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Data
{
	public class AppDbContext: IdentityDbContext<User>
	{
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Request> Requests { get; set; } = null!;
        public DbSet<EmployeeRequest> EmployeeHandledRequests { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=HelpDeskDatabase.db");
        }

        
    }



}

