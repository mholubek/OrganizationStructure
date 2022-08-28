﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrganizationStructure.Data;

#nullable disable

namespace OrganizationStructure.Migrations
{
    [DbContext(typeof(OrganizationStructureDbContext))]
    [Migration("20220828212844_AddedNestedNodes")]
    partial class AddedNestedNodes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("OrganizationStructure.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Division", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int?>("DivisionId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DivisionId")
                        .HasColumnType("int");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DivisionId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Department", b =>
                {
                    b.HasOne("OrganizationStructure.Models.Project", null)
                        .WithMany("Departments")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Division", b =>
                {
                    b.HasOne("OrganizationStructure.Models.Company", null)
                        .WithMany("Divisions")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Employee", b =>
                {
                    b.HasOne("OrganizationStructure.Models.Company", null)
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId");

                    b.HasOne("OrganizationStructure.Models.Department", null)
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("OrganizationStructure.Models.Division", null)
                        .WithMany("Employees")
                        .HasForeignKey("DivisionId");

                    b.HasOne("OrganizationStructure.Models.Project", null)
                        .WithMany("Employees")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Project", b =>
                {
                    b.HasOne("OrganizationStructure.Models.Division", null)
                        .WithMany("Projects")
                        .HasForeignKey("DivisionId");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Company", b =>
                {
                    b.Navigation("Divisions");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Division", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Projects");
                });

            modelBuilder.Entity("OrganizationStructure.Models.Project", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}