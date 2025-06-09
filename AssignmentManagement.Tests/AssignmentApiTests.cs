
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AssignmentManagement.Core;
using AssignmentManagement.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AssignmentManagement.Tests
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
        public async Task CreateAssignment_ReturnsCreated()
        {
            var assignment = new Assignment("Test Task", "This is a test.", laterDate, apMed, false, "");
            var response = await _client.PostAsJsonAsync("/api/assignment", assignment);
            response.EnsureSuccessStatusCode();
			
		}

        [Fact]
        public async Task GetAllAssignments_ReturnsList()
        {
            var response = await _client.GetAsync("/api/assignment");
            response.EnsureSuccessStatusCode();
            var assignments = await response.Content.ReadFromJsonAsync<List<Assignment>>();
            Assert.NotNull(assignments);
        }
    }
}
