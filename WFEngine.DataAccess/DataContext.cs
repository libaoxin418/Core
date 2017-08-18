using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using WFEngine.DataAccess.Models;

namespace WFEngine.DataAccess
{
    /// <summary>
    /// The entity framework context with a DataContext DbSet
    /// </summary>
    public class DataContext : DbContext
    {
        public DbSet<Workflow> Workflows { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("MySqlConnection");

            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
