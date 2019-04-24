using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing; //NuGet
using System;
using System.Collections.Generic;
using WebAppMVCRecap.Controllers;
using WebAppMVCRecap.Models;
using Xunit;

namespace XUnitTestMVCRecap
{
    public class UnitTestCars
    {
        [Fact]
        public void ZeroCarsIndexWorks()
        {
            //Arrange
            var carsController = new CarsController(new MockCarsRepository());

            //Act
            var result = carsController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Car>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void TwoCarsIndexWorks()
        {
            //Arrange
            List<Car> cars = new List<Car> {
                new Car() { Id = 1, Brand = "test1", Name = "tester1"},
                new Car() { Id = 2, Brand = "test2", Name = "tester2"}
            };
            var carsController = new CarsController(new MockCarsRepository(cars));

            //Act
            var result = carsController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Car>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void CreateCar()
        {
            //Arrange
            var carsController = new CarsController(new MockCarsRepository());
            Car aCar = new Car() { Id = 9, Brand = "test1", Name = "tester1" };

            //Act
            var result = carsController.Create(aCar);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void CreateCarFailBrand()
        {
            //Arrange
            var carsController = new CarsController(new MockCarsRepository());
            carsController.ModelState.AddModelError("Brand", "Req Brand");
            Car aCar = new Car();

            //Act
            var result = carsController.Create(aCar);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Car>(viewResult.ViewData.Model);
            Assert.IsType<Car>(model);
        }
    }
}
