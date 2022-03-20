using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class UserMessage
    {
        public string User { get; set; }

        public string Message { get; set; }

        public DateTime DateSent { get; set; }
    }
}
