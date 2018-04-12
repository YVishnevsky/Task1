using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkNestTask.Requests
{
    public class SignIn
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}