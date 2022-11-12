using ScoreboardApp.Application.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    internal class RefreshUsersControllerTests
    {
        private const string EndpointUnderTest = "api/Users/authenticate";
        private const string ValidPassword = "Pa@@word123";

        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<AuthenticateCommand> _authenticateCommandGenerator = new Faker<AuthenticateCommand>()
            .RuleFor(x => x.UserName, faker => faker.Internet.UserName())
            .RuleFor(x => x.Password, faker => ValidPassword);

        private readonly Faker<RegisterCommand> _registerCommandGenerator = new Faker<RegisterCommand>()
            .RuleFor(x => x.UserName, faker => faker.Internet.UserName())
            .RuleFor(x => x.Email, faker => faker.Internet.Email())
            .RuleFor(x => x.Password, faker => ValidPassword);

        public RefreshUsersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }
    }
}
