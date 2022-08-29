﻿using Microsoft.EntityFrameworkCore;
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
                //optionsBuilder.UseSqlServer("Data Source=192.168.1.52\\SQLEXPRESS;Initial Catalog=CompanyOrganizationStructureDb;User Id=sa; Password=Localadmin");
            }
        }

        public void AddNode(IOrganizationNode node)
        {
            if (node.GetType() == typeof(Company))
                Companies.Add((Company)node);
            if (node.GetType() == typeof(Division))
                Divisions.Add((Division)node);
            if (node.GetType() == typeof(Project))
                Projects.Add((Project)node);
            if (node.GetType() == typeof(Department))
                Departments.Add((Department)node);
        }

        public void RemoveNode(IOrganizationNode node)
        {
            if (node.GetType() == typeof(Company))
                Companies.Remove((Company)node);
            if (node.GetType() == typeof(Division))
                Divisions.Remove((Division)node);
            if (node.GetType() == typeof(Project))
                Projects.Remove((Project)node);
            if (node.GetType() == typeof(Department))
                Departments.Remove((Department)node);
        }


    }
}
