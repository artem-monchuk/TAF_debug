using Framework.Core.ApiClients;
using Framework.Business.Api;
using RestSharp;
using System.Net;
using Framework.Core.Utilities;

namespace Framework.Tests.TestCases
{
    [TestFixture, Parallelizable]
    [Category("API")]
    public class ApiTests : BaseApiClient
    {
        //status code assertion is added to all tests 
        //assertion for 'no error' in response is added to all tests
        //all assertions are grouped in Assert.Multiple() to avoid test failure on 1st assertion
        //predefined constraint-based NUnit assertions are introduced (like Has.All, Does.Not.Contain, and Is.EqualTo) 
        //3 A's comment are added to all the test methods

        [Test]
        public async Task ValidateUsersListIsReceivedSuccessfully()
        {
            //Arrange
            var request = CreateRequest(Constants.Endpoints.Users, Method.Get);

            //Act
            var response = await ExecuteAsync(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code.");
            var users = await ExecuteWithDeserializationAsync<List<User>>(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Content, Does.Not.Contain("error"), "Response contains error messages.");

                Assert.That(users, Is.Not.Null, "Users list is null!");
                Assert.That(users, Has.All.Matches<User>(u => u.Id > 0), "One or more users has an invalid ID.");
                Assert.That(users, Has.All.Matches<User>(u => !string.IsNullOrEmpty(u.Name)), "One or more users has an empty name.");
                Assert.That(users, Has.All.Matches<User>(u => !string.IsNullOrEmpty(u.Username)), "One or more users has an empty username.");
                Assert.That(users, Has.All.Matches<User>(u => u.Address != null), "One or more users has a null address.");
                Assert.That(users, Has.All.Matches<User>(u => !string.IsNullOrEmpty(u.Phone)), "One or more users has an empty phone.");
                Assert.That(users, Has.All.Matches<User>(u => !string.IsNullOrEmpty(u.Website)), "One or more users has an empty website.");
                Assert.That(users, Has.All.Matches<User>(u => u.Company != null), "One or more users has a null company.");
            });
        }
        //RestSharp is used, Content-Type header is retrieved from response.Content.Headers
        [Test]
        public async Task ValidateResponseHeader()
        {
            //Arrange
            var request = new ApiRequestBuilder(Constants.Endpoints.Users, Method.Get).Build();

            //Act
            var response = await ExecuteAsync(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code.");
                Assert.That(response.Content, Does.Not.Contain("error"), "Response contains error messages.");

                //retrieve 'Content-Type' from ContentHeaders using LINQ
                var contentTypeHeader = response.ContentHeaders?.FirstOrDefault(h => h.Name == "Content-Type")?.Value;
                Assert.That(contentTypeHeader, Is.Not.Null, "Content-Type header is missing.");
                Assert.That(contentTypeHeader, Is.EqualTo("application/json; charset=utf-8"), "Unexpected Content-Type header value.");

                Logger.LogInfo($"Content-Type: {contentTypeHeader}");
            });
        }

        [Test]
        public async Task ValidateResponseBodyForUsers()
        {
            //Arrange
            var request = CreateRequest(Constants.Endpoints.Users, Method.Get);

            //Act
            var response = await ExecuteAsync(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code.");
            var users = await ExecuteWithDeserializationAsync<List<User>>(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Content, Does.Not.Contain("error"), "Response contains error messages.");

                Assert.That(users.Count, Is.EqualTo(10), "Users list does not contain 10 elements!");

                //each user is with different Id
                Assert.That(users.Select(u => u.Id), Is.Unique, "User IDs are not unique.");

                //each user contains non-empty name/username and company name
                users.ForEach(user =>
                {
                    Assert.That(user.Name, Is.Not.Null.And.Not.Empty, $"User ID {user.Id} has an empty name.");
                    Assert.That(user.Username, Is.Not.Null.And.Not.Empty, $"User ID {user.Id} has an empty username.");
                    Assert.That(user.Company.Name, Is.Not.Null.And.Not.Empty, $"User ID {user.Id} has an empty company name.");
                });
            });
        }

        [Test]
        public async Task ValidateUserCanBeCreated()
        {
            //Arrange
            var newUser = new UserBuilder()
                            .WithName("Test User")
                            .WithUsername("testuser")
                            .Build();

            var request = CreateRequest(Constants.Endpoints.Users, Method.Post)
                          .AddJsonBody(newUser);

            //Act
            var response = await ExecuteAsync(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Unexpected status code.");
                Assert.That(response.Content, Does.Not.Contain("error"), "Response contains error messages.");
                Assert.That(response.Content.Contains("id"), "Response does not contain ID!");
            });
        }

        [Test]
        public async Task ValidateInvalidEndpointReturnsNotFound()
        {
            //Arrange
            var request = CreateRequest(Constants.Endpoints.InvalidEndpoint, Method.Get);

            //Act
            var response = await ExecuteAsync(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected response for invalid endpoint!");
                //might be unnecessary returned, as {} is only returned
                Assert.That(response.Content, Does.Not.Contain("error"), "Response contains error messages.");
            });
        }
    }
}