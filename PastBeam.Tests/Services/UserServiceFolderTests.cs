

using Microsoft.AspNetCore.Identity;
using Moq;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Logger;
using Xunit;

namespace PastBeam.Tests.Services
{
    public class UserServiceFolderTests
    {

        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IFavoriteRepository> favoriteRepositoryMock;
        private readonly Mock<IBookmarkRepository> bookmarkRepositoryMock;
        private readonly Mock<IFolderRepository> folderRepositoryMock;
        private readonly Mock<IUserCourseRepository> userCourseRepositoryMock;
        private readonly UserService userService;
        private readonly Mock<ILogger> loggerMock;
        private readonly Mock<UserManager<User>> userManagerMock;

        public UserServiceFolderTests()
        {
            loggerMock = new Mock<ILogger>();
            userRepositoryMock = new Mock<IUserRepository>();
            favoriteRepositoryMock = new Mock<IFavoriteRepository>();
            bookmarkRepositoryMock = new Mock<IBookmarkRepository>();
            folderRepositoryMock = new Mock<IFolderRepository>();
            userCourseRepositoryMock = new Mock<IUserCourseRepository>();
            userManagerMock = new Mock<UserManager<User>>();

            userService = new UserService(
                userRepositoryMock.Object,
                favoriteRepositoryMock.Object,
                bookmarkRepositoryMock.Object,
                folderRepositoryMock.Object,
                userCourseRepositoryMock.Object,
                loggerMock.Object,
                userManagerMock.Object);
        }

        [Fact]
        public async Task CreateFolderAsync_ShouldCreateFolder()
        {
            var userId = "testid";
            var folderName = "New Folder";
            var expectedFolder = new Folder
            {
                UserId = userId,
                Name = folderName
            };

            userRepositoryMock.Setup(repo => repo.CreateFolderAsync(It.IsAny<Folder>())).Returns(Task.CompletedTask);

            var result = await userService.CreateFolderAsync(userId, folderName);

            Assert.NotNull(result);
            Assert.Equal(expectedFolder.UserId, result?.UserId);
            Assert.Equal(expectedFolder.Name, result?.Name);

            userRepositoryMock.Verify(repo => repo.CreateFolderAsync(It.Is<Folder>(f => f.Name == folderName && f.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task GetUserFoldersAsync_WhenFoldersExist()
        {
            List<Folder> folders = new List<Folder> { new Folder { Id = 1, Name = "folder1", UserId = "test_id_1" } };
            userRepositoryMock.Setup(repo => repo.GetUserFoldersAsync("test_id_1")).ReturnsAsync(folders);

            var result = await userService.GetUserFoldersAsync("test_id_1");
            List<Folder> resultList = result.ToList();

            Assert.NotNull(result);
            Assert.Single(resultList);
            Assert.Equal("folder1", resultList[0].Name);
        }

        [Fact]
        public async Task DeleteFolderAsync_ShouldReturnFolder_WhenFolderExists()
        {
            var folderId = 1;
            var folder = new Folder { Id = folderId, Name = "Folder 1", UserId = "test_id_1" };

            userRepositoryMock.Setup(repo => repo.DeleteFolderAsync(folderId)).ReturnsAsync(folder);

            var result = await userService.DeleteFolderAsync(folderId);

            Assert.NotNull(result);
            Assert.Equal(folderId, result?.Id);

            userRepositoryMock.Verify(repo => repo.DeleteFolderAsync(folderId), Times.Once);
        }
    }
}
