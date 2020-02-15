using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public class Menu
    {
        private readonly int _menuLevel;
        private const string MenuCommandExit = "X";
        private const string ReturnToPrev = "R";
        private const string ReturnToMain = "M";
        
        private Dictionary<string, MenuItem> _menuItemsDictionary = new Dictionary<string, MenuItem>();

        public static bool AgainstAi { get; set; }

        public Menu(int menuLevel = 0)
        {
            _menuLevel = menuLevel;
        }

        public string Title { get; set; } = "title";

        public Dictionary<string, MenuItem> MenuItemsDictionary
        {
            get => _menuItemsDictionary;
            set
            { _menuItemsDictionary = value;
                if (_menuLevel >= 2)
                {
                    _menuItemsDictionary.Add(ReturnToPrev, new MenuItem() {Title = "Return"});
                }

                if (_menuLevel >= 1)
                {
                    _menuItemsDictionary.Add(ReturnToMain, new MenuItem() {Title = "Return to Main"});
                }
                _menuItemsDictionary.Add(MenuCommandExit,new MenuItem(){Title = "Exit"});
                
            }
        }


        public string Run()
        {
            string command;
            do
            {
                Console.WriteLine(Title);
                Console.WriteLine("==============================");
                foreach (var menuItem in MenuItemsDictionary)
                {
                    Console.Write(menuItem.Key);
                    Console.Write(" ");
                    Console.WriteLine(menuItem.Value);
                    
                }

                Console.WriteLine("==============================");
                Console.Write(">");

                command = Console.ReadLine()?.Trim().ToUpper() ?? "";
                if (command == "1")
                {
                    AgainstAi = true;
                }
                

                var returnCommand = "";
                
                if (MenuItemsDictionary.ContainsKey(command))
                {
                    var menuItem = MenuItemsDictionary[command];
                    if (MenuItemsDictionary[command].BoardToExecute != null)
                    {
                        returnCommand = menuItem.BoardToExecute(6,7); // beginner gameboard
                        //break;
                    }
                    if (MenuItemsDictionary[command].CommandToExecute != null)
                    {
                        returnCommand = menuItem.CommandToExecute(); // menu level 2
                        //break;
                    }
                }
                

                if (returnCommand == MenuCommandExit)
                {
                    command = MenuCommandExit;
                }

                if (returnCommand == ReturnToMain)
                {
                    if (_menuLevel != 0)
                    {
                        command = ReturnToMain;
                    }
                }
            } while (command != MenuCommandExit && 
                     command != ReturnToMain && 
                     command != ReturnToPrev);

            return command;
        }
    }
}