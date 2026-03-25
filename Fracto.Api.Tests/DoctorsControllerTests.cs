using Fracto.Api.Controllers;
using Fracto.Api.Data;
using Fracto.Api.DTOs.Doctors;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fracto.Api.Tests
{
    public class DoctorsControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetAll_ReturnsActiveDoctors()
        {
            // Arrange
            var dbContext = GetDbContext();
            dbContext.Specializations.Add(new Specialization { Id = 1, Name = "Cardio" });
            dbContext.Doctors.Add(new Doctor { Id = 1, FullName = "Dr. Active", IsActive = true, City = "Delhi", SpecializationId = 1 });
            dbContext.Doctors.Add(new Doctor { Id = 2, FullName = "Dr. Inactive", IsActive = false, City = "Delhi", SpecializationId = 1 });
            await dbContext.SaveChangesAsync();

            var controller = new DoctorsController(dbContext);

            // Act
            var result = await controller.GetAll(null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var doctors = Assert.IsAssignableFrom<IEnumerable<DoctorListItemDto>>(okResult.Value);
            Assert.Single(doctors);
        }
    }
}
