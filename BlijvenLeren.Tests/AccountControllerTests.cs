using BlijvenLeren.Controllers;
using BlijvenLeren.Data;
using BlijvenLeren.Services;
using BlijvenLeren.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BlijvenLeren.Tests
{
    public class AccountControllerTests
    {
        private const string testUserName = "TestCommentText";
        private BlijvenLerenContext blijvenLerenContext;

        Mock<IUserService> userServiceMock;
        AccountController sut;


        [SetUp]
        public void Setup()
        {
            userServiceMock = new Mock<IUserService>();
        }

        [Test]
        public void Register_ReturnViewIfModelUnknown()
        {
            sut = new AccountController(userServiceMock.Object);
            var actual = sut.Register();

            Assert.IsInstanceOf(typeof(ActionResult), actual);
        }

        [Test]
        public void Register_RedirectsWhenSuccesful()
        {
            userServiceMock.Setup(x => x.CreateUser(It.IsAny<RegisterViewModel>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            sut = new AccountController(userServiceMock.Object);

            var model = new RegisterViewModel
            {
                Email = "a@b.net",
                Password = "Foo",
                ConfirmPassword = "Foo"
            };

            var result = sut.Register(model);
            var expected = nameof(RedirectToActionResult);
            var actual = result.Result.GetType().Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Register_RedirectsToHomeIndexWhenSuccesful()
        {
            userServiceMock.Setup(x => x.CreateUser(It.IsAny<RegisterViewModel>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            sut = new AccountController(userServiceMock.Object);

            var model = new RegisterViewModel
            {
                Email = "a@b.net",
                Password = "Foo",
                ConfirmPassword = "Foo"
            };

            var result = sut.Register(model);
            var expectedController = "Home";
            var expectedAction = "index";

            var actualController = (result.Result as RedirectToActionResult).ControllerName;
            var actualAction = (result.Result as RedirectToActionResult).ActionName;

            Assert.AreEqual(expectedController, actualController);
            Assert.AreEqual(expectedAction, actualAction);
        }

        [Test]
        public void Login_ReturnViewIfUserUnknown()
        {
            sut = new AccountController(userServiceMock.Object);
            var actual = sut.Login();

            Assert.IsInstanceOf(typeof(ActionResult), actual);
        }

        [Test]
        public void Login_RedirectsWhenLoginSucceed()
        {
            userServiceMock.Setup(x => x.Login(It.IsAny<LoginViewModel>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            sut = new AccountController(userServiceMock.Object);

            var result = sut.Login(new LoginViewModel());
            var expected = nameof(RedirectToActionResult);
            var actual = result.Result.GetType().Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_RedirectsToHomeIndexWhenLoginSucceed()
        {
            userServiceMock.Setup(x => x.Login(It.IsAny<LoginViewModel>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            sut = new AccountController(userServiceMock.Object);

            var result = sut.Login(new LoginViewModel());
            var expectedController = "Home";
            var expectedAction = "Index";

            var actualController = (result.Result as RedirectToActionResult).ControllerName;
            var actualAction = (result.Result as RedirectToActionResult).ActionName;

            Assert.AreEqual(expectedController, actualController);
            Assert.AreEqual(expectedAction, actualAction);
        }

        [Test]
        public void Logout_RedirectsToLogin()
        {
            sut = new AccountController(userServiceMock.Object);

            var result = sut.Logout();

            var expectedAction = "Login";
            var actualAction = (result.Result as RedirectToActionResult).ActionName;

            Assert.AreEqual(expectedAction, actualAction);
        }
    }
}
