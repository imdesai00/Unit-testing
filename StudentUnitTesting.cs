using adotoaspcorewebapi.Controllers;
using adotoaspcorewebapi.Data;
using adotoaspcorewebapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crudapitesting
{
    public class StudentUnitTesting
    {
        private readonly Mock<IStudentDataAccess> _mockRepo;
        private readonly StudentController _controller;

        public StudentUnitTesting()
        {
            _mockRepo = new Mock<IStudentDataAccess>();
            _controller = new StudentController(_mockRepo.Object);
        }

        //GET ALL STUDENT 
        [Fact]
        public void Task_GetAllStudent_ReturnOkResult()
        {
            // Arrange
            var mocStudent = new List<Student>()
                    {
                        new Student() { StudentID = 1 , FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 },
                        new Student() { StudentID = 2 , FirstName = "Test", LastName = "Test", Age = 30, Gender = "FeMale", Grade = 90.54 }
                    };
            _mockRepo.Setup(repo => repo.GetAllStudents()).Returns(mocStudent);

            // Act
            var result = _controller.GetAllStudents();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ResponceModel<List<Student>>>(actionResult.Value);
            Assert.Equal(2, model.Data.Count());
        }
        [Fact]
        public void Task_GetAllStudent_ReturnBadRequestResultOn_Exception()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetAllStudents()).Throws(new Exception("Test exception"));

            //Act
            var result = _controller.GetAllStudents();

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<object>>(actionResult.Value); // Ensure this matches your actual response model class name and namespace
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Error while Fetching data", responseModel.Description);
        }

        // GET STUDENT BY ID
        [Fact]
        public void Task_GetStudentByID_ReturnOkResult()
        {
            // Arrange
            var mocStudent = new Student() { StudentID = 1 , FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.GetstudentById(1)).Returns(mocStudent);

            // Act
            var result = _controller.GetstudentById(1);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ResponceModel<Student>>(actionResult.Value);
            Assert.Equal(mocStudent.StudentID, model.Data.StudentID);
            Assert.Equal(mocStudent.FirstName, model.Data.FirstName);
            Assert.Equal(mocStudent.LastName, model.Data.LastName);
            Assert.Equal(mocStudent.Age, model.Data.Age);
            Assert.Equal(mocStudent.Gender, model.Data.Gender);
            Assert.Equal(mocStudent.Grade, model.Data.Grade);
        }
        [Fact]
        public void Task_GetStudentByID_NotFoundResult()
        {
            // Arrange
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.GetstudentById(4)).Returns( () => null);

            // Act
            var result = _controller.GetstudentById(4);


            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
            var response = Assert.IsType <ResponceModel<object>> (notFoundResult.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, response.Status);
            Assert.Null(response.Data);
            Assert.Equal("Data Not Exist", response.Description);
        }
        [Fact]
        public void Task_GetStudentbyID_ReturnBadRequestResultOn_Exception()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetstudentById(4)).Throws(new Exception("Test exception"));

            //Act
            var result = _controller.GetstudentById(4);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<object>>(actionResult.Value); // Ensure this matches your actual response model class name and namespace
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Error while Fetching data", responseModel.Description);
        }

        //ADD STUDENT
        [Fact]
        public void Task_AddStudent_ReturnOkResult()
        {
            //Arrange
            var mocStudent = new Student() { StudentID = 1 , FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Addstudent(mocStudent)).Returns(true);

            //Act
            var result = _controller.Addstudent(mocStudent);

            //Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<ResponceModel<Student>>(okresult.Value);
            Assert.Equal(1, response.Data.StudentID);
        }
        [Fact]
        public void Task_AddStudent_ReturnBadRequestResultOn_invalideData()
        {
            //Arrange
            var mocStudent = new Student() { StudentID = 1, FirstName = "", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Addstudent(mocStudent)).Returns(false);

            //Act
            var result = _controller.Addstudent(mocStudent);

            //Assert
            var okresult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<Student>>(okresult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Country data cannot be null or empty.", responseModel.Description);
        }
        [Fact]
        public void Task_AddStudent_ReturnBadRequestResultOn_Exception()
        {
            //Arrange
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Addstudent(mocStudent)).Throws(new Exception("Test exception"));

            //Act
            var result = _controller.Addstudent(mocStudent);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<object>>(actionResult.Value); // Ensure this matches your actual response model class name and namespace
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Error while Fetching data", responseModel.Description);
        }

        //UPDATE STUDENT
        [Fact]
        public void Task_UpdateStudent_ReturnOkResult()
        {
            //Arrange
            int id = 1;
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Updatestudent(mocStudent)).Returns(true);

            //Act
            var result = _controller.Updatestudent(id,mocStudent);

            //Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<ResponceModel<Student>>(okresult.Value);
            Assert.Equal(1, response.Data.StudentID);
        }
        [Fact]
        public void Task_UpdateStudent_ReturnBadRequestResultOn_invalideData()
        {
            //Arrange
            int id = 1;
            var mocStudent = new Student() { StudentID = 1, FirstName = "", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Updatestudent(mocStudent)).Returns(false);

            //Act
            var result = _controller.Updatestudent(id,mocStudent);

            //Assert
            var okresult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<Student>>(okresult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Student data is not valid.", responseModel.Description);
        }
        [Fact]
        public void Task_UpdateStudent_ReturnBadRequestResultOn_changingId()
        {
            //Arrange
            int id = 2;
            var mocStudent = new Student() { StudentID = 1, FirstName = "", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Updatestudent(mocStudent)).Returns(false);

            //Act
            var result = _controller.Updatestudent(id, mocStudent);

            //Assert
            var okresult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<Student>>(okresult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Id has primary key it can't change or add manually", responseModel.Description);
        }
        [Fact]
        public void Task_UpdateStudent_ReturnBadRequestResultOn_Exception()
        {
            //Arrange
            int id = 1;
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Updatestudent(mocStudent)).Throws(new Exception("Test exception"));

            //Act
            var result = _controller.Updatestudent(id, mocStudent);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<object>>(actionResult.Value); // Ensure this matches your actual response model class name and namespace
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Error while Fetching data", responseModel.Description);
        }

        //DELETE STUDENT
        [Fact]
        public void Task_DeleteStudentByID_ReturnOkResult()
        {
            // Arrange
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Deletestudent(1)).Returns(mocStudent);

            // Act
            var result = _controller.Deletestudent(1);

            // Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<ResponceModel<Student>>(okresult.Value);
            Assert.Equal((int)HttpStatusCode.OK, response.Status);
            Assert.Equal(mocStudent, response.Data);
            Assert.Equal("Data Deleted", response.Description);
        }
        [Fact]
        public void Task_DeleteStudentByID_NotFoundResult()
        {
            // Arrange
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Deletestudent(4)).Returns(() => null);

            // Act
            var result = _controller.Deletestudent(4);


            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
            var response = Assert.IsType<ResponceModel<object>>(notFoundResult.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, response.Status);
            Assert.Null(response.Data);
            Assert.Equal("Data Not Exist", response.Description);
        }
        [Fact]
        public void Task_StudentbyID_ReturnBadRequestResultOn_Exception()
        {
            //Arrange
            var mocStudent = new Student() { StudentID = 1, FirstName = "Test", LastName = "Test", Age = 20, Gender = "Male", Grade = 90.54 };
            _mockRepo.Setup(repo => repo.Deletestudent(1)).Throws(new Exception("Test exception"));

            //Act
            var result = _controller.Deletestudent(1);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsAssignableFrom<ResponceModel<object>>(actionResult.Value); // Ensure this matches your actual response model class name and namespace
            Assert.Equal((int)HttpStatusCode.BadRequest, responseModel.Status);
            Assert.Contains("Error while Fetching data", responseModel.Description);
        }
    }
}
