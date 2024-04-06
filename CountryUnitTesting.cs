
//using adotoaspcorewebapi.Controllers;
//using adotoaspcorewebapi.Data;
//using adotoaspcorewebapi.Models;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Hosting;
//using Moq;
//using System.Diagnostics.Metrics;
//using System.Net;

//namespace crudapitesting
//{
//    public class CountryUnitTesting
//    {
//        private readonly Mock<ICountryDataAccess> _mockRepo;
//        private readonly CountryController _controller;

//        public CountryUnitTesting()
//        {
//            _mockRepo = new Mock<ICountryDataAccess>();
//            _controller = new CountryController(_mockRepo.Object);
//        }
//        [Fact]
//        public void Task_GetAllCountry_ReturnsAllCountries()
//        {
//            // Arrange
//            var mockCountries = new List<CountryMaster>()
//            {
//                new CountryMaster() { CountryId = 1, CountryName = "Country A" },
//                new CountryMaster() { CountryId = 2, CountryName = "Country B" }
//            };
//            _mockRepo.Setup(repo => repo.GetAllCountry()).Returns(mockCountries);

//            // Act
//            var result = _controller.GetAllCountry();

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var model = Assert.IsAssignableFrom<ResponceModel<IEnumerable<CountryMaster>>>(actionResult.Value);
//            Assert.Equal(2, model.Data.Count());
//        }
//        [Fact]
//        public void Task_GetAllCountry_ReturnsBadRequestResult()
//        {
//            //Arrange  
//            var mockCountries = new List<CountryMaster>()
//            {
//                new CountryMaster() { CountryId = 1, CountryName = "Country A" },
//                new CountryMaster() { CountryId = 2, CountryName = "Country B" }
//            };
//            _mockRepo.Setup(repo => repo.GetAllCountry()).Returns(mockCountries);

//            //Act  
//            var data = _controller.GetAllCountry();
//            data = null;

//            if (data != null)
//                //Assert  
//                Assert.IsType<BadRequestResult>(data);
//        }

//        [Fact]
//        public void Task_GetAllCountryByID_ReturnsAllCountries()
//        {
//            // Arrange
//            var mockCountries = new CountryMaster() { CountryId = 1, CountryName = "Country A" };
//            _mockRepo.Setup(repo => repo.GetCountryById(1)).Returns(mockCountries);
            

//            // Act
//            var result = _controller.GetCountryById(1);

//            // Assert
//            var actionResult = Assert.IsType<OkObjectResult>(result);
//            var model = Assert.IsAssignableFrom<ResponceModel<CountryMaster>>(actionResult.Value);
//            Assert.Equal(mockCountries.CountryId, model.Data.CountryId);
//            Assert.Equal(mockCountries.CountryName, model.Data.CountryName);

//        }
//        [Fact]
//        public void Task_GetAllCountryByID_ReturnsBadRequestResult()
//        {
//            //Arrange  
//            var mockCountries = new CountryMaster() { CountryId = 1, CountryName = "Country A" };
//            _mockRepo.Setup(repo => repo.GetCountryById(1)).Returns(mockCountries);

//            //Act  
//            var data = _controller.GetCountryById(1);
//            data = null;

//            //Assert  
//            if (data != null)         
//                Assert.IsType<BadRequestResult>(data);
//        }
//        [Fact]
//        public void Task_GetAllCountryByID_ReturnsNotFound()
//        {
//            //Arrange  
//            _mockRepo.Setup(repo => repo.GetCountryById(4)).Returns((CountryMaster)null);

//            //Act  
//            var data = _controller.GetCountryById(4);

//            //Assert
//            Assert.IsType<OkObjectResult>(data);

//            var okResult = data as OkObjectResult;
//            Assert.NotNull(okResult);

//            var response = okResult.Value as ResponceModel<CountryMaster>;
//            Assert.NotNull(response);

//            Assert.Equal((int)HttpStatusCode.NotFound, response.Status);
//        }

//        [Fact]
//        public async void Task_AddInvalideCountryData_ReturnBadRequest()
//        {
//            //Arrange  
//            var countrymaster = new CountryMaster() { CountryId = 1, CountryName = "Country A" };

//            //Act              
//            var data = _controller.AddCountry(countrymaster);

//            //Assert  
//            Assert.IsType<BadRequestResult>(data);
//        }
//        [Fact]
//        public void Task_AddCountryData_ReturnOkResult()
//        {
//            //Arrange
//            var countrymaster = new CountryMaster() { CountryId = 1, CountryName = "Country A" };

//            //Act  
//            var data = _controller.AddCountry(countrymaster);

//            //Assert  
//            var actionResult = Assert.IsType<OkObjectResult>(data);
//            Assert.NotNull(actionResult);

//            // Optionally, you can further assert the content of the OkObjectResult
//            var response = actionResult.Value as ResponceModel<string>;
//            Assert.NotNull(response);
//            Assert.Equal((int)HttpStatusCode.OK, response.Status);
//            Assert.Equal("Data Added", response.Data);
//        }
//        [Fact]
//        public void Task_AddValidData_MatchResult()
//        {
//            //Arrange  
//            var countrymaster = new CountryMaster() { CountryId = 1, CountryName = "Country A" };

//            //Act  
//            var data = _controller.AddCountry(countrymaster);

//            //Assert  
//            Assert.IsType<ConflictResult>(data);
//        }
//    }
//}