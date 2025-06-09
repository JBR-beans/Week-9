
using Xunit;
using Moq;
using AssignmentManagement.Core;
using AssignmentManagement.Console;
using System.Collections.Generic;
using System.IO;
using AssignmentManagement.UI;

namespace AssignmentManagement.Tests
{
    public class ConsoleUITests
    {
		private readonly DateTime laterDate;
		private readonly AssignmentPriority apMed = AssignmentPriority.Medium;

        public ConsoleUITests()
        {
			laterDate = new DateTime(11,11,11);
		}

		[Fact]
		public void Log_ShouldWriteMessageToConsoleWithPrefix()
		{
			var logger = new ConsoleAppLogger();
			var output = new StringWriter();
			System.Console.SetOut(output);

			var message = "Test message";

			logger.Log(message);

			var consoleOutput = output.ToString().Trim();
			Assert.Equal("[LOG] Test message", consoleOutput);
		}

		[Fact]
        public void AddAssignment_Should_Call_Service_Add()
        {
            var mock = new Mock<IAssignmentService>();
            var ui = new ConsoleUI(mock.Object);

            // Correct input: choose menu option 1, enter title, enter description, then exit
            using var input = new StringReader("1\nSample Title\nSample Description\n2025-06-30\nMedium\n");
            System.Console.SetIn(input);

            ui.Run();

            mock.Verify(s => s.AddAssignment(It.Is<Assignment>(a =>
                a.Title == "Sample Title" &&
                a.Description == "Sample Description"
            )), Times.Once);
        }


        [Fact]
        public void SearchAssignmentByTitle_Should_Display_Assignment()
        {
            var mock = new Mock<IAssignmentService>();

			var assignment = new Assignment("Sample", "Details", DateTime.Now.AddDays(1), AssignmentPriority.Medium, false, "");

			mock.Setup(s => s.FindAssignmentByTitle("Sample")).Returns(assignment);

			var ui = new ConsoleUI(mock.Object);

            using var input = new StringReader("5\nSample\n0\n");
            System.Console.SetIn(input);

            ui.Run();

            mock.Verify(s => s.FindAssignmentByTitle("Sample"), Times.Once);
        }

        [Fact]
        public void DeleteAssignment_Should_Call_Service_Delete()
        {
            var mock = new Mock<IAssignmentService>();
			mock.Setup(s => s.ListAll()).Returns(new List<Assignment>
            {
	            new Assignment("ToDelete", "desc", DateTime.Now.AddDays(1), AssignmentPriority.Medium, false, "")
            }); 
            mock.Setup(s => s.DeleteAssignment("ToDelete")).Returns(true);

            var ui = new ConsoleUI(mock.Object);

            using var input = new StringReader("7\nToDelete\n0\n");
            System.Console.SetIn(input);

            ui.Run();

            mock.Verify(s => s.DeleteAssignment("ToDelete"), Times.Once);
        }
    }
}
