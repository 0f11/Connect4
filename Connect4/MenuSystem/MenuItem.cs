using System;

namespace MenuSystem
{
    public class MenuItem
    {
        public string Title { get; set; } = "title";

        public Func<string> CommandToExecute { get; set; }

        public Func<int, int, string> BoardToExecute { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}