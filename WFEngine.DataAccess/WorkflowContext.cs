using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WFEngine.DataAccess.Models;

namespace WFEngine.DataAccess
{
    /// <summary>
    /// The entity framework context with a DataContext DbSet
    /// </summary>
    public class WorkflowContext : DbContext
    {
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<WorkflowAssociation> WorkflowAssociations { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; }

        public WorkflowContext(DbContextOptions<WorkflowContext> options)
                : base(options)
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=PEKWNCOWO03;Initial Catalog=Core_WorkflowDB;Persist Security Info=True;User ID=sa;Password=abcd-1234");
        //}
    }
}
