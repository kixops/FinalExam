using ASP_FinalExam_Net6.Controllers;
using ASP_FinalExam_Net6.Data;
using ASP_FinalExam_Net6.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class UnitTestController
    {
        public static DbContextOptions<ApplicationDbContext> dbContextOptions { get; }

        //Replace connection string with yours
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-ASP_FinalExam_Net6-C00B432D-AD85-4FC7-BEB4-79B78C41E2BF;Trusted_Connection=True;MultipleActiveResultSets=true";
        ApplicationDbContext context = new ApplicationDbContext(dbContextOptions);

        static UnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public UnitTestController()
        {
            context = new ApplicationDbContext(dbContextOptions);
            MockData db = new MockData();
            db.Seed(context);
        }

        [Fact]
        public void Task_GetDepartmentDetailById_Return_OkResult()
        {
            var controller = new DepartmentController(context);
            var postId = 2;
            var data = controller.DepartmentDetail(postId);

            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        public void Task_GetDepartmentDetailById_NoContentResult()
        {
            var controller = new DepartmentController(context);
            var postId = 5;

            var data = controller.DepartmentDetail(postId);
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public void Task_GetDepartmentDetailById_MatchResult()
        {
            var controller = new DepartmentController(context);
            int postId = 1;

            var data = controller.DepartmentDetail(postId);

            Assert.IsType<ViewResult>(data);
            var okResult = (Department)data.Should().BeOfType<ViewResult>().Subject.Model;

            Assert.Equal("Department 1", okResult.Name);
        }

        [Fact]
        public void Task_AddDepartment_InvalidModelState_Return_View()
        {
            var controller = new DepartmentController(context);

            controller.ModelState.AddModelError("Name", "The Name field is required.");

            var department = new Department { Name = "" };

            var result = controller.SaveData(department);

            var viewResult = Assert.IsType<ViewResult>(result);
            var testDepartment = Assert.IsType<Department>(viewResult.Model);

            Assert.Equal(department.Name, testDepartment.Name);
        }


        [Fact]
        public void Task_AddDepartment_RedirectIndex()
        {
            var controller = new DepartmentController(context);
            var department = new Department { Name = "Department 2" };

            var result = controller.SaveData(department);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
