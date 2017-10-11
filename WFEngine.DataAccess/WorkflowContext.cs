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
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<WorkflowAssociation> WorkflowAssociations { get; set; }
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
        public DbSet<DBConnection> DBConnections { get; set; }

        public WorkflowContext(DbContextOptions<WorkflowContext> options)
                : base(options)
        { }
    }
}
