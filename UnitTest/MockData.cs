using ASP_FinalExam_Net6.Data;
using ASP_FinalExam_Net6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class MockData
    {
        public MockData()
        {
        }

        public void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Department.AddRange(
                new Department() { Name = "Department 1" },
                new Department() { Name = "Department 2" },
                new Department() { Name = "Department 3" }
            );
            context.SaveChanges();
        }
    }
}
