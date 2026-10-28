
using Moq;
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Models;
using TeleHealthLite.Web.Services;

namespace TeleHealthLite.Tests;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _repositoryMock;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _repositoryMock = new Mock<IPatientRepository>();
        _service = new PatientService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPatients()
    {
        var patients = new List<Patient>
        {
            new Patient { Id = 1 },
            new Patient { Id = 2 }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(patients);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoPatientsExist()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Patient>());

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPatient_WhenPatientExists()
    {
        var patient = new Patient { Id = 1 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(patient);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Patient?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_CallsRepositoryWithPatient()
    {
        var patient = new Patient { Id = 1 };

        await _service.AddAsync(patient);

        _repositoryMock.Verify(
            r => r.AddAsync(patient),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_CallsRepositoryWithPatient()
    {
        var patient = new Patient { Id = 1 };

        await _service.UpdateAsync(patient);

        _repositoryMock.Verify(
            r => r.UpdateAsync(patient),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepositoryWithCorrectId()
    {
        await _service.DeleteAsync(5);

        _repositoryMock.Verify(
            r => r.DeleteAsync(5),
            Times.Once);
    }

    [Fact]
    public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
    {
        _repositoryMock
            .Setup(r => r.EmailExistsAsync("patient@example.com"))
            .ReturnsAsync(true);

        var result = await _service.EmailExistsAsync(
            "patient@example.com");

        Assert.True(result);
    }

    [Fact]
    public async Task EmailExistsAsync_ReturnsFalse_WhenEmailDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.EmailExistsAsync("newpatient@example.com"))
            .ReturnsAsync(false);

        var result = await _service.EmailExistsAsync(
            "newpatient@example.com");

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepositoryOnlyOnce()
    {
        await _service.DeleteAsync(3);

        _repositoryMock.Verify(
            r => r.DeleteAsync(3),
            Times.Once);
    }

}
