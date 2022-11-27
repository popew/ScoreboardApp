using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class UpdateHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;


        private readonly Faker<CreateHabitTrackerCommand> _createCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.Utf16String(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        private readonly Faker<UpdateHabitTrackerCommand> _updateCommandGenerator = new Faker<UpdateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.Utf16String(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.Important);



        public UpdateHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.NormalTestUser.Token);
        }

        [Fact]
        public async Task Update_UpdatesHabitTracker_WhenDataIsValid()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            var updateCommand = _updateCommandGenerator.Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            // Assert
            var updatedObject = await updateHttpResponse.Content.ReadFromJsonAsync<UpdateHabitTrackerCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_ReturnsError_WhenTitleIsTooLong()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            var updateCommand = _updateCommandGenerator.Clone()
                                                       .RuleFor(x => x.Title, faker => faker.Random.Utf16String(201))
                                                       .Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var updatedObject = await updateHttpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The title is too long.");
        }

        [Fact]
        public async Task Update_ReturnsError_WhenTitleIsEmpty()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            var updateCommand = _updateCommandGenerator.Clone()
                                                       .RuleFor(x => x.Title, faker => string.Empty)
                                                       .Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var updatedObject = await updateHttpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The title cannot be null or empty.");
        }

        [Fact]
        public async Task Update_ReturnsError_WhenPriorityIsNotInEnum()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            var updateCommand = new { Title = "Title", Priority = "PriorityNotInEnum" };

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            // Assert
            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenHabitTrackerDoesntExist()
        {
            // Arrange
            var updateCommand = _updateCommandGenerator.Generate();

            var randomId = Guid.NewGuid();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{randomId}", updateCommand);


            // Assert

            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }
    }
}
