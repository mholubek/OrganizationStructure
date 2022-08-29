using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationStructure.Migrations
{
    public partial class PopulateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //populate employees
            migrationBuilder.Sql(@"insert into employees (title, firstName, lastName, phoneNumber, email)
                                   values 
                                   ('Ing', 'Anton', 'Mrkvicka', '0922123456', 'amrkvicka@email.sk'),
                                   ('Ing', 'Jana', 'Simkova', '0923123456', 'jsimkova@email.sk'),
                                   ('', 'Frantisek', 'Hlinka', '0924123456', 'fhlinka@email.sk'),
                                   ('Ing', 'Petronela', 'Hradska', '0925123456', 'phradska@email.sk'),
                                   ('Mgr', 'Peter', 'Krajci', '0926123456', 'pkrajci@email.sk'),
                                   ('Ing', 'Adam', 'Sandler', '0927123456', 'asandler@email.sk')");
            //populate companies
            migrationBuilder.Sql(@"insert into companies (name, code, leaderId) 
                                   values ('Super firma s.r.o.', 'SF1', 1)");

            ////populate divisions
            migrationBuilder.Sql(@"insert into divisions (name, code, leaderId, companyId) 
                                   values 
                                   ('Divizia 1', 'DIV1', 1, 1),
                                   ('Divizia 2', 'DIV2', 3, 1),
                                   ('Divizia 3', 'DIV3', 5, 1)");

            ////populate divisions
            migrationBuilder.Sql(@"insert into projects (name, code, leaderId, divisionId) 
                                   values 
                                   ('Project 1', 'PRO1', 1, 1),
                                   ('Project 2', 'PRO2', 3, 2),
                                   ('Project 3', 'PRO3', 5, 3)");

            ////populate departments
            migrationBuilder.Sql(@"insert into departments (name, code, leaderId, projectId) 
                                   values 
                                   ('Department 1', 'DEP1', 1, 1),
                                   ('Department 2', 'DEP2', 3, 2),
                                   ('Department 3', 'DEP3', 5, 3)");

            migrationBuilder.Sql(@"update employees set companyId = 1, divisionId = 1, projectId = 1, departmentId = 1 where id = 1");
            migrationBuilder.Sql(@"update employees set companyId = 1, divisionId = 1, projectId = 1, departmentId = 1 where id = 2");
            migrationBuilder.Sql(@"update employees set companyId = 1, divisionId = 2, projectId = 2, departmentId = 2 where id = 3");
            migrationBuilder.Sql(@"update employees set companyId = 1, divisionId = 2, projectId = 2, departmentId = 2 where id = 4");
            migrationBuilder.Sql(@"update employees set companyId = 1, divisionId = 3, projectId = 3, departmentId = 3 where id = 5");
            migrationBuilder.Sql(@"update employees set companyId = 1, divisionId = 3, projectId = 3, departmentId = 3 where id = 6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from employees where id between 1 and 6");
            migrationBuilder.Sql("delete from companies where id = 1");
            migrationBuilder.Sql("delete from divisions where id between 1 and 3");
            migrationBuilder.Sql("delete from projects where id between 1 and 3");
            migrationBuilder.Sql("delete from departments where id between 1 and 3");

        }
    }
}
