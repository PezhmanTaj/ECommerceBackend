using NUnit.Framework;
using Moq;
using AutoMapper;
using FluentValidation;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;
using FluentValidation.Results;
using MongoDB.Driver;
using ECommerceSolution.DAL.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    private readonly Mock<IMongoDBContext> _mockContext;
    private readonly Mock<IMongoCollection<User>> _mockCollection;
    private readonly UserRepository _repository;
    private readonly Mock<IAsyncCursor<User>> _mockCursor;


    public UserRepositoryTests()
    {
        _mockContext = new Mock<IMongoDBContext>();
        _mockCollection = new Mock<IMongoCollection<User>>();
        _mockCursor = new Mock<IAsyncCursor<User>>();
        _mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>()))
            .Returns(_mockCollection.Object);

        _repository = new UserRepository(_mockContext.Object);
    }

    [Test]
    public async Task AddUserAsync_CallsInsertOneAsync()
    {
        // Arrange
        User testUser = new User();
        _mockCollection.Setup(x => x.InsertOneAsync(testUser, null, default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _repository.AddAsync(testUser);

        // Assert
        _mockCollection.Verify();
    }

    [Test]
    public async Task DeleteUserAsync_ValidId_DeletesUser()
    {
        // Arrange
        var testId = "testId";
        var deleteResultMock = new Mock<DeleteResult>();

        deleteResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
        deleteResultMock.SetupGet(r => r.DeletedCount).Returns(1);

        _mockCollection.Setup(x =>
            x.DeleteOneAsync(It.IsAny<FilterDefinition<User>>(), default))
            .ReturnsAsync(deleteResultMock.Object);

        // Act
        var result = await _repository.DeleteAsync(testId);

        // Assert
        Assert.True(result);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var expectedUsers = new List<User> { new User(), new User() };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(expectedUsers);
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                               It.IsAny<FindOptions<User, User>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var users = await _repository.GetAllAsync();

        // Assert
        Assert.That(users.Count(), Is.EqualTo(expectedUsers.Count));
        CollectionAssert.AreEquivalent(expectedUsers, users);
    }

    [Test]
    public async Task GetUsersByIdAsync_ReturnsUser()
    {
        // Arrange
        var expectedUser = new User { UserId = "testId" };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(new List<User> { expectedUser });
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                               It.IsAny<FindOptions<User, User>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var user = await _repository.GetByIdAsync("testId");

        // Assert
        Assert.That(user, Is.EqualTo(expectedUser));
    }

    [Test]
    public async Task UpdateUserAsync_UserExists_ReturnsTrue()
    {
        // Arrange
        var testId = "testId";
        var testUser = new User { UserId = testId };
        var replaceResultMock = new Mock<ReplaceOneResult>();

        // Setting up the ReplaceOneAsync method to return a successful result
        replaceResultMock.SetupGet(r => r.IsAcknowledged).Returns(true);
        replaceResultMock.SetupGet(r => r.ModifiedCount).Returns(1);

        _mockCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<User>>(),
                                                     It.IsAny<User>(),
                                                     It.IsAny<ReplaceOptions>(),
                                                     default))
                       .ReturnsAsync(replaceResultMock.Object);

        // Act
        var result = await _repository.UpdateAsync(testId, testUser);

        // Assert
        Assert.IsTrue(result);
    }


    [Test]
    public async Task GetUsersByUsernameAsync_ReturnsUser()
    {
        // Arrange
        var expectedUser = new User { Username = "testUsername" };
        _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                   .ReturnsAsync(true)
                   .ReturnsAsync(false);
        _mockCursor.Setup(_ => _.Current).Returns(new List<User> { expectedUser });
        _mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                               It.IsAny<FindOptions<User, User>>(),
                                               default))
                       .ReturnsAsync(_mockCursor.Object);

        // Act
        var user = await _repository.GetByUsernameAsync("testUsername");

        // Assert
        Assert.That(user, Is.EqualTo(expectedUser));
    }


}






