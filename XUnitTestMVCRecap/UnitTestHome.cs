using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing; //NuGet
using System;
using WebAppMVCRecap.Controllers;
using WebAppMVCRecap.Models;
using Xunit;

namespace XUnitTestMVCRecap
{
    public class UnitTestHome
    {
        [Fact]
        public void HomeIndexWorks()
        {
            //Arrange
            var homeController = new HomeController();

            //Act
            var result = homeController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
    }
}
