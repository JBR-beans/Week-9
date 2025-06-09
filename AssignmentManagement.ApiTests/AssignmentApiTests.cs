
using System.Net;
using System.Net.Http.Json;
using AssignmentManagement.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Xunit;

namespace AssignmentManagement.ApiTests
{
    public class AssignmentApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private readonly DateTime laterDate;
        private readonly AssignmentPriority apMed = AssignmentPriority.Medium;
        public AssignmentApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

			laterDate = DateTime.Now.AddDays(1);
		}
        [Fact]
        public void PrintCurrentDirectory()
        {
            var directory = ($"Current Directory: {Environment.CurrentDirectory}");
        }

        [Fact]
        public async Task CanCreateAssignment()
        {
            var assignment = new Assignment("Test Assignment", "This is a test assignment.", laterDate, apMed, false, "");
            var response = await _client.PostAsJsonAsync("/api/assignment", assignment);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CanGetAllAssignments()
        {
            //create new assignment
            var assignment = new Assignment("Test2 Assignment", "This is a test assignment.", laterDate, apMed, false, "");
            await _client.PostAsJsonAsync("/api/assignment", assignment);
            var response = await _client.GetAsync("/api/assignment");
            response.EnsureSuccessStatusCode();

            var assignments = await response.Content.ReadFromJsonAsync<List<Assignment>>();
            Assert.Contains(assignments, a => a.Title == "Test2 Assignment");
        }
    }
}
