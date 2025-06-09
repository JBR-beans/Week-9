using System;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentManagement.Core
{
    public class AssignmentService : IAssignmentService
    {
        private readonly List<Assignment> _assignments = new();
        private readonly IAssignmentFormatter _formatter;
        private readonly IAppLogger _logger;

        public AssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
        {
            _formatter = formatter;
            _logger = logger;
        }

        public bool AddAssignment(Assignment assignment)
        {
            try
            {
                _assignments.Add(assignment);
                _logger.Log($"Added Assignment [{assignment.Id}]: {assignment.Title}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error adding assignment: {ex.Message}");
                return false;
            }
        }

        public bool DeleteAssignment(string title)
        {
            var toRemove = _assignments.FirstOrDefault(a => a.Title == title);
            if (toRemove != null)
            {
                _assignments.Remove(toRemove);
                _logger.Log($"Deleted Assignment [{toRemove.Id}]: {toRemove.Title}");
                return true;
            }
			_logger.Log($"Assignment not found");
			return false;
        }

        public List<Assignment> ListAll()
        {
            if (_assignments.Count == 0)
            {
				_logger.Log($"Assignments not found");
				return _assignments;
            }
			_logger.Log($"Assignments found: {_assignments.Count}");
			return _assignments;  
        }

        public List<Assignment> ListIncomplete()
        {
			var assignment = _assignments.Where(a => !a.IsCompleted).ToList();

			if (assignment != null)
			{
				_logger.Log($"Assignments found: {assignment.Count}");
				return assignment;
			}
			_logger.Log($"Assignments not found");
			return assignment;
		}

        public List<string> ListFormatted() => _assignments.Select(a => _formatter.Format(a)).ToList();

        public Assignment FindByTitle(string title)
        {
			var assignment = _assignments.FirstOrDefault(a => a.Title == title); 
            if (assignment  != null)
            {
				_logger.Log($"Assignment found: {assignment.Title}");
				return assignment;
			}
			_logger.Log($"Assignment not found");
			return assignment;
            
        }

        public bool UpdateAssignment(string title, string newTitle, string newDescription, DateTime newDueDate, AssignmentPriority newPriority, bool newIsCompleted, string newNotes)
        {
            var assignment = FindByTitle(title);
            
            if (assignment != null)
            {
                assignment.Update(newTitle, newDescription, newDueDate, newPriority, newIsCompleted, newNotes);
				_logger.Log($"Assignment updated: {assignment.Title}");
				return true;
            }
			_logger.Log($"Assignment not found: {assignment.Title}");
			return false;
        }

        public bool MarkComplete(string title)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
				_logger.Log($"Assignment marked complete: {assignment.Title}");
				return true;
            }
			_logger.Log($"Assignment not found: {assignment.Title}");
			return false;
        }

        public Assignment FindAssignmentByTitle(string title)
        {	
			var assignment =_assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
			if (assignment != null)
            {
				_logger.Log($"Assignments found: {assignment.Title}");
				return assignment;
			}
			_logger.Log($"Assignment not found: {assignment.Title}");
            return assignment;
           
        }

        public bool MarkAssignmentComplete(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
				_logger.Log($"Assignment marked complete: {assignment.Title}");
				return true;
            }
			_logger.Log($"Assignment not found: {assignment.Title}");
			return false;
        }

    }
}