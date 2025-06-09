namespace AssignmentManagement.Tests
{
    using AssignmentManagement.Core;
	using System.Diagnostics.CodeAnalysis;

	public class AssignmentTests
    {
		private readonly DateTime laterDate;
		private readonly AssignmentPriority apMed = AssignmentPriority.Medium;

        public AssignmentTests()
        {
            laterDate = DateTime.Now.AddDays(1);
        }
		[Fact]
        public void Constructor_ValidInput_ShouldCreateAssignment()
        {
            var assignment = new Assignment("Read Chapter 2", "Summarize key points", laterDate, apMed, false, "");
            Assert.Equal("Read Chapter 2", assignment.Title);
            Assert.Equal("Summarize key points", assignment.Description);
            Assert.False(assignment.IsCompleted);
        }

        [Fact]
        public void Constructor_BlankTitle_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Assignment("", "Valid description", laterDate, apMed, false, ""));
        }

        [Fact]
        public void Update_BlankDescription_ShouldThrowException()
        {
            var assignment = new Assignment("Read Chapter 2", "Summarize key points", laterDate, apMed, false, "");

			Assert.Throws<ArgumentException>(() => assignment.Update("Valid title", "", laterDate, apMed, false, ""));
        }

        [Fact]
        public void MarkComplete_SetsIsCompletedToTrue()
        {
            var assignment = new Assignment("Task", "Complete the lab", laterDate, apMed, false, "");
            assignment.MarkComplete();
            Assert.True(assignment.IsCompleted);
        }
    }
}
