
namespace AssignmentManagement.Core
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public AssignmentPriority Priority { get; private set; }
        public bool IsCompleted { get; private set; }
        public string? Notes { get; private set; }

        public Assignment(string title, string description, DateTime dueDate, AssignmentPriority priority, bool iscompleted, string notes = "")
        {
			if (title.Length == 0)
				throw new ArgumentException(" Title cannot be empty! ");
			if (description.Length == 0)
				throw new ArgumentException(" Description cannot be empty! ");
			if (dueDate == default)
				throw new ArgumentException("DueDate must be provided.");
			DueDate = dueDate;
            if (Enum.IsDefined(typeof(AssignmentPriority), priority) == false)
				throw new ArgumentException("Invalid priority value.");
            
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
			Notes = notes;
            IsCompleted = iscompleted;
        }

        public void Update(string? newTitle, string? newDescription, DateTime? newDueDate, AssignmentPriority newPriority, bool newIsCompleted, string newNotes)
        {

			if (string.IsNullOrWhiteSpace(newTitle))
				throw new ArgumentException(" Title cannot be empty! ");
			if (string.IsNullOrWhiteSpace(newDescription))
				throw new ArgumentException(" Description cannot be empty! ");
			if (newDueDate.HasValue == false)
				throw new ArgumentException("DueDate must be provided.");
			if (Enum.IsDefined(typeof(AssignmentPriority), newPriority) == false)
				throw new ArgumentException("Invalid priority value.");
			Title = newTitle;
			Description = newDescription;
			DueDate = newDueDate;
            Priority = newPriority;
            Notes = newNotes;
            IsCompleted = newIsCompleted;
        }

        public void MarkComplete()
        {
            IsCompleted = true;
        }

		public bool IsOverdue()
		{
			return DueDate.HasValue && !IsCompleted && DueDate.Value < DateTime.Now;
		}

		public override string ToString()
        {
			var notePart = string.IsNullOrWhiteSpace(Notes) ? "" : $"\nNotes: {Notes}";

			return $"- {Title} ({Priority}) due {DueDate?.ToShortDateString() ?? "N/A"}\n{Description}{notePart}";
		}
		// BUG: Notes not included in output
	}
    }

    public enum AssignmentPriority
    {
        Low,
        Medium,
        High
    }

