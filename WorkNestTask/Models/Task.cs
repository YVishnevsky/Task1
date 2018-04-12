using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkNestTask.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string ReporterId { get; set; }
        public string AssigneeId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }
    }
}