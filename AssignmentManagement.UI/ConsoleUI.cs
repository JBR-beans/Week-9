using AssignmentManagement.Core;

using System;

namespace AssignmentManagement.UI
{
    public class ConsoleUI
    {
        private readonly IAssignmentService _assignmentService;
        
        public ConsoleUI(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nAssignment Manager Menu:");
                Console.WriteLine("1. Add Assignment");
                Console.WriteLine("2. List All Assignments");
                Console.WriteLine("3. List Incomplete Assignments");
                Console.WriteLine("4. Mark Assignment as Complete");
                Console.WriteLine("5. Search Assignment by Title");
                Console.WriteLine("6. Update Assignment");
                Console.WriteLine("7. Delete Assignment");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddAssignment();
                        break;
                    case "2":
                        ListAllAssignments();
                        break;
                    case "3":
                        ListIncompleteAssignments();
                        break;
                    case "4":
                        MarkAssignmentComplete(); // TODO
                        break;
                    case "5":
                        SearchAssignmentByTitle(); // TODO
                        break;
                    case "6":
                        UpdateAssignment(); // TODO
                        break;
                    case "7":
                        DeleteAssignment(); // TODO
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private void AddAssignment()
        {
            Console.WriteLine("Enter assignment title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter assignment description: ");
            var description = Console.ReadLine();
			Console.WriteLine("Enter a due date: ");
			var newDueDate = Console.ReadLine();
			if (!DateTime.TryParse(newDueDate, out var parsedDueDate))
			{
				Console.WriteLine("Invalid due date format.");
				return;
			}
			Console.WriteLine("Enter a priority level (Low, Medium, High): ");
			var newPriority = Console.ReadLine();
			if (Enum.TryParse<AssignmentPriority>(newPriority, true, out var parsedPriority) == false || Enum.IsDefined(typeof(AssignmentPriority), parsedPriority) == false)
			{
				Console.WriteLine("Invalid priority level.");
				return;
			}
            Console.WriteLine("Enter new note: ");
            var newNotes = Console.ReadLine();

			Console.WriteLine("Is the assignment complete? (true/false):");
			var isCompleteInput = Console.ReadLine()?.Trim().ToLower();

			if (bool.TryParse(isCompleteInput, out bool parsedIsComplete) == false)
			{
				Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
				return;
			}

			try
            {
				var assignment = new Assignment(title, description, parsedDueDate, parsedPriority, parsedIsComplete, newNotes);
				if (_assignmentService.AddAssignment(assignment))
                {
                    Console.WriteLine("Assignment added successfully.");
                }
                else
                {
                    Console.WriteLine("An assignment with this title already exists.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ListAllAssignments()
        {
            var assignments = _assignmentService.ListAll();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"{assignment.Priority} - {assignment.Title}: {assignment.Description} | Due: {assignment.DueDate} - (Completed: {assignment.IsCompleted}) | Notes: {assignment.Notes}");
            }
        }

        private void ListIncompleteAssignments()
        {
            var assignments = _assignmentService.ListIncomplete();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No incomplete assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
				Console.WriteLine($"{assignment.Priority} - {assignment.Title}: {assignment.Description} | Due: {assignment.DueDate} - (Completed: {assignment.IsCompleted}) | Notes: {assignment.Notes}");
			}
        }

        private void MarkAssignmentComplete()
        {
            Console.WriteLine("Enter the title of the assignment to mark as complete:");
            var title = Console.ReadLine();
            if (_assignmentService.MarkAssignmentComplete(title))
            {
                Console.WriteLine("Assignment marked as complete.");
            }
            else
            {
                Console.WriteLine("Assignment not found or already completed.");
            }
        }

        private void SearchAssignmentByTitle()
        {
            Console.WriteLine("Enter the title of the assignment to search:");
            var title = Console.ReadLine();
            var assignment = _assignmentService.FindAssignmentByTitle(title);
            if (assignment != null)
            {
                Console.WriteLine($"Found Assignment: {assignment.Title} - {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }

        private void UpdateAssignment()
        {
            Console.WriteLine("Enter the title of the assignment to update:");
            var oldTitle = Console.ReadLine();
            Console.Write("Enter new title: ");
            var newTitle = Console.ReadLine();
            Console.Write("Enter new description: ");
            var newDescription = Console.ReadLine();
            
            Console.WriteLine("Enter a new due date: ");
            var newDueDate = Console.ReadLine();
			if (!DateTime.TryParse(newDueDate, out var parsedDueDate))
			{
				Console.WriteLine("Invalid due date format.");
				return;
			}

			Console.WriteLine("Enter a new priority level (Low, Medium, High): ");
            var newPriority = Console.ReadLine();
			if (Enum.TryParse<AssignmentPriority>(newPriority, true, out var parsedPriority) == false || Enum.IsDefined(typeof(AssignmentPriority), parsedPriority) == false)
			{
				Console.WriteLine("Invalid priority level.");
				return;
			}
			Console.WriteLine("Enter note: ");
			var newNotes = Console.ReadLine();

			Console.WriteLine("Is the assignment complete? (true/false):");
			var isCompleteInput = Console.ReadLine()?.Trim().ToLower();

			if (bool.TryParse(isCompleteInput, out bool parsedIsComplete) == false)
			{
				Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
				return;
			}
			if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription, parsedDueDate, parsedPriority, parsedIsComplete, newNotes))
            {
                Console.WriteLine("Assignment updated successfully.");
            }
            else
            {
                Console.WriteLine("Assignment not found or update failed.");
            }
        }

        private void DeleteAssignment()
        {
			Console.Write("Enter the title of the assignment to delete: ");
			string title = Console.ReadLine();

			var assignments = _assignmentService.ListAll();

			var match = assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

			if (match != null)
			{
				bool deleted = _assignmentService.DeleteAssignment(title);
				if (deleted)
				{
					Console.WriteLine($"Assignment '{title}' deleted.");
				}
				else
				{
					Console.WriteLine($"Failed to delete assignment '{title}'.");
				}
			}
			else
			{
				Console.WriteLine($"No assignment found with the title '{title}'.");
			}
		}
    }
}
