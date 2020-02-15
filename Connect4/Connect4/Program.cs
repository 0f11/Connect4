using System;
using System.Collections.Generic;
using ConsoleUI;
using MenuSystem;



namespace Connect4
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Clear();
            var menu2 = new Menu(2)
            {
                Title = "Select board size",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "B", new MenuItem()
                        {
                            Title = "Beginner",
                            BoardToExecute = GameRunner.GameEngine
                        }
                    },
                    {
                        "C", new MenuItem()
                        {
                            Title = "Custom",
                            CommandToExecute = GameRunner.GameEngine
                        }
                    },
                }
            };
            var gameMenu = new Menu(1)
            {
                Title = "Start a new game of Connect-4",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Computer Starts",
                            CommandToExecute = menu2.Run
                        }
                        
                    },
                    {
                        "2", new MenuItem()
                        {
                            Title = "1 v 1",
                            CommandToExecute = menu2.Run
                        }
                        
                    },
                }
            };
            var menu0 = new Menu()
            {
                Title = "Connect-4 Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "S", new MenuItem()
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    },
                    {
                        "L", new MenuItem()
                        {
                            Title = "Load game",
                            CommandToExecute = GameRunner.LoadSavedGame
                        }
                    },
                }
            };
            menu0.Run();
        }
    }
}