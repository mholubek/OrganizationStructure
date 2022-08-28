using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Models;

namespace OrganizationStructure.Data
{
    public class OrganizationStructureDbContext : DbContext
    {

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }


        public OrganizationStructureDbContext()
        {

        }

        public OrganizationStructureDbContext(DbContextOptions<DbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CompanyOrganizationStructureDb;Integrated Security=true");
            }
        }

    }
}
