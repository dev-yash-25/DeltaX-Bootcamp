using System;

namespace IMDBConsoleApp.Models
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
    }
}
