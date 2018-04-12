using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkNestTask.Requests
{
    public class UpdateTask
    {
        public string AssigneeId { get; set; }
        public string Name { get; set; }
        public DateTime? Completed { get; set; }
    }
}