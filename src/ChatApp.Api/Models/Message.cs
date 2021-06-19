using System;

namespace ChatApp.Api.Models
{
    public class Message
    {
        public string Content {get; set;}
        public DateTime DateTime {get;} = DateTime.UtcNow;
    }
}