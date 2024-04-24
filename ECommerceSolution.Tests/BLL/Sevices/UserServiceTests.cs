using NUnit.Framework;
using System.Security;
using Moq;
using AutoMapper;
using FluentValidation;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;
using FluentValidation.Results;
using ECommerceSolution.DAL.Repositories;
using ECommerceSolution.BLL.Interfaces;
using System.Security.Authentication;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IValidator<UserRegistrationDTO>> _mockValidator;
    private UserService _userService;
    private Mock<IPasswordHasher> _mockPasswordHasher;
    private Mock<ITokenService> _mockTokenService;


    [SetUp]
    public void SetUp()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<UserRegistrationDTO>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockTokenService = new Mock<ITokenService>();
        _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object, _mockValidator.Object, _mockPasswordHasher.Object, _mockTokenService.Object);
    }

    [Test]
    public void AuthenticateAsyncInvalidCredentialsThrowsAuthenticationException()
    {
        // Arrange
        var loginDto = new UserLoginDTO { Username = "user1", Password = "pass" };
        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("user1")).ReturnsAsync(new User { PasswordHash = "hashedpass" });
        _mockPasswordHasher.Setup(hasher => hasher.VerifyPassword("hashedpass", "pass")).Returns(false);

        // Act & Assert
        Assert.ThrowsAsync<AuthenticationException>(() => _userService.AuthenticateAsync(loginDto));
    }


    [Test]
    public void RegisterUserAsyncInvalidDataThrowsValidationException()
    {
        // Arrange
        var registrationDto = new UserRegistrationDTO { Username = "newuser", Password = "newpass" };
        var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Username", "Error") });
        _mockValidator.Setup(v => v.Validate(registrationDto)).Returns(validationResult);

        // Act & Assert
        Assert.ThrowsAsync<ValidationException>(() => _userService.RegisterUserAsync(registrationDto));
    }

    [Test]
    public async Task GetUserByIdAsync_ValidUserId_ReturnsUserDTO()
    {
        // Arrange
        var userId = "123";
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(new User { UserId = userId });
        _mockMapper.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(new UserDTO { UserId = userId });

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.UserId, Is.EqualTo(userId));
    }


    [Test]
    public void ChangePasswordAsyncOldPasswordIncorrectThrowsArgumentException()
    {
        // Arrange
        string userId = "123";
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(new User { UserId = userId, PasswordHash = "oldHash" });
        _mockPasswordHasher.Setup(hasher => hasher.VerifyPassword("oldHash", "oldPassword")).Returns(false);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _userService.ChangePasswordAsync(userId, "oldPassword", "newPassword"));
    }

    [Test]
    public async Task ChangePasswordAsync_ValidCredentials_PasswordUpdated()
    {
        // Arrange
        string userId = "123";
        string newHashedPassword = "newHashed";
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(new User { UserId = userId, PasswordHash = "oldHash" });
        _mockPasswordHasher.Setup(hasher => hasher.VerifyPassword("oldHash", "oldPassword")).Returns(true);
        _mockPasswordHasher.Setup(hasher => hasher.HashPassword("newPassword")).Returns(newHashedPassword);
        _mockUserRepository.Setup(repo => repo.UpdateAsync(userId, It.IsAny<User>())).ReturnsAsync(true);

        // Act
        bool result = await _userService.ChangePasswordAsync(userId, "oldPassword", "newPassword");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteUserAsync_UserExists_ReturnsTrue()
    {
        // Arrange
        string userId = "123";
        _mockUserRepository.Setup(repo => repo.DeleteAsync(userId)).ReturnsAsync(true);

        // Act
        bool result = await _userService.DeleteUserAsync(userId);

        // Assert
        Assert.IsTrue(result);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once());
    }

    [Test]
    public async Task GetUserByUsernameAsync_UsernameExists_ReturnsUserDTO()
    {
        // Arrange
        string username = "user123";
        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(new User { Username = username });
        _mockMapper.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(new UserDTO { Username = username });

        // Act
        var result = await _userService.GetUserByUsernameAsync(username);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Username, Is.EqualTo(username));
    }

    [Test]
    public async Task UpdateUserAsync_UserExists_ReturnsTrue()
    {
        // Arrange
        string username = "existingUser";
        var updateDto = new UserUpdateDTO { Username = username };
        _mockMapper.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpdateDTO>())).Returns(new User { Username = username });
        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(new User { UserId = "123", Username = username });
        _mockUserRepository.Setup(repo => repo.UpdateAsync("123", It.IsAny<User>())).ReturnsAsync(true);

        // Act
        bool result = await _userService.UpdateUserAsync(updateDto);

        // Assert
        Assert.IsTrue(result);
        _mockUserRepository.Verify(repo => repo.UpdateAsync("123", It.IsAny<User>()), Times.Once());
    }

    [Test]
    public void UpdateUserAsyncUserNotFoundThrowsArgumentException()
    {
        // Arrange
        var updateDto = new UserUpdateDTO { Username = "nonExistingUser" };
        _mockMapper.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpdateDTO>())).Returns(new User { Username = "nonExistingUser" });
        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("nonExistingUser")).ReturnsAsync((User)null);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateUserAsync(updateDto));
    }


}

