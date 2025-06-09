
using AssignmentManagement.Core;
using Xunit;

namespace AssignmentManagement.Tests
{
    public class AssignmentServiceTests
    {
		private readonly DateTime laterDate;
		private readonly AssignmentPriority apMed = AssignmentPriority.Medium;

        public AssignmentServiceTests()
        {
			laterDate = DateTime.Now.AddDays(1);
		}
		[Fact]
        public void ListIncomplete_ReturnsOnlyIncompleteAssignments()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a1 = new Assignment("Title 1", "Desc 1", laterDate, apMed, false, "");
            var a2 = new Assignment("Title 2", "Desc 2", laterDate, apMed, false, "");
            a2.MarkComplete();

            service.AddAssignment(a1);
            service.AddAssignment(a2);

            var result = service.ListIncomplete();

            Assert.Single(result);
            Assert.Contains(a1, result);
            Assert.DoesNotContain(a2, result);
        }

        [Fact]
        public void ListIncomplete_ReturnsEmptyList_WhenNoAssignments()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var result = service.ListIncomplete();
            Assert.Empty(result);
        }

        [Fact]
        public void ListIncomplete_ReturnsAll_WhenAllAreIncomplete()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a1 = new Assignment("Title 1", "Desc 1", laterDate, apMed, false, "");
            var a2 = new Assignment("Title 2", "Desc 2", laterDate, apMed, false, "");

            service.AddAssignment(a1);
            service.AddAssignment(a2);

            var result = service.ListIncomplete();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void AddAssignment_StoresAssignmentCorrectly()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a = new Assignment("Title", "Desc", laterDate, apMed, false, "");
            service.AddAssignment(a);
            Assert.Contains(a, service.ListAll());
        }
    }
}
