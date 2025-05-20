using Moq;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class CourseMaterialsServiceTests
{
    private readonly Mock<ICourseMaterialsRepository> _mockRepo;
    private readonly CourseMaterialsService _service;

    public CourseMaterialsServiceTests()
    {
        _mockRepo = new Mock<ICourseMaterialsRepository>();
        _service = new CourseMaterialsService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetMaterialsByCourseAsync_ReturnsMaterials()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var expectedMaterials = new List<CourseMaterials>
        {
            new CourseMaterials { Id = Guid.NewGuid(), CourseId = courseId, Title = "Intro" },
            new CourseMaterials { Id = Guid.NewGuid(), CourseId = courseId, Title = "Chapter 1" }
        };

        _mockRepo.Setup(r => r.GetByCourseIdAsync(courseId))
                 .ReturnsAsync(expectedMaterials);

        // Act
        var result = await _service.GetMaterialsByCourseAsync(courseId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMaterials.Count, (result as List<CourseMaterials>)?.Count);
        _mockRepo.Verify(r => r.GetByCourseIdAsync(courseId), Times.Once);
    }

    [Fact]
    public async Task AddMaterialAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var material = new CourseMaterials { Id = Guid.NewGuid(), Title = "New Material" };

        // Act
        await _service.AddMaterialAsync(material);

        // Assert
        _mockRepo.Verify(r => r.AddAsync(material), Times.Once);
    }

    [Fact]
    public async Task DeleteMaterialAsync_CallsRepositoryDeleteAsync()
    {
        // Arrange
        var materialId = Guid.NewGuid();

        // Act
        await _service.DeleteMaterialAsync(materialId);

        // Assert
        _mockRepo.Verify(r => r.DeleteAsync(materialId), Times.Once);
    }

    [Fact]
    public async Task UpdateMaterialAsync_CallsRepositoryUpdateAsync()
    {
        // Arrange
        var updatedMaterial = new CourseMaterials { Id = Guid.NewGuid(), Title = "Updated" };

        // Act
        await _service.UpdateMaterialAsync(updatedMaterial);

        // Assert
        _mockRepo.Verify(r => r.UpdateAsync(updatedMaterial), Times.Once);
    }
}
