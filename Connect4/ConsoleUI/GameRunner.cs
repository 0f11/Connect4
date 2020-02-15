using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using DAL;
using GameEngine;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleUI
{
    public static class GameRunner
    {
        private static GameSettings? _settings;

        public static string GameEngine()
        {
            _settings = new GameSettings {MovesCounter = 0, Time = DateTime.Now};
            if (Menu.AgainstAi)
            {
                _settings.AgainstAi = true;
            }

            int userBoardHeightInt;
            int userBoardWidthInt;
            bool success;
            bool nameSet;

            do
            {
                Console.WriteLine("Enter your Name");
                Console.Write(">");
                _settings.PlayerName = Console.ReadLine();
                nameSet = _settings.PlayerName != null && _settings.PlayerName.Length >= 1;

                if (!nameSet)
                {
                    Console.WriteLine("Please enter your name!");
                    //_settings.Time = DateTime.Now;
                }
            } while (!nameSet);

            do
            {
                Console.WriteLine("Enter board height");
                Console.Write(">");
                var userBoardHeight = Console.ReadLine();
                success = int.TryParse(userBoardHeight, out userBoardHeightInt);
                if (!success || userBoardHeightInt < 6)
                {
                    Console.WriteLine($"{userBoardHeight} is not a number or too small(6)!");
                    success = false;
                }
            } while (!success);

            do
            {
                Console.WriteLine("Enter board width");
                Console.Write(">");
                var userBoardWidth = Console.ReadLine();
                success = int.TryParse(userBoardWidth, out userBoardWidthInt);
                if (!success || userBoardWidthInt < 7)
                {
                    Console.WriteLine($"{userBoardWidth} is not a number or is too small(7)!");
                    success = false;
                }
            } while (!success);

            _settings.BoardHeight = userBoardHeightInt;
            _settings.BoardWidth = userBoardWidthInt;
            var game = new Game(_settings);
            bool done;

            do
            {
                UserInterface.BoardPrint(game);
                int userYInt;
                do
                {
                    if (_settings.MovesCounter % 2 == 0 && _settings.AgainstAi)
                    {
                        Console.WriteLine("Mastermind AI move");
                        var r = new Random();
                        userYInt = r.Next(0, _settings.BoardWidth - 1);
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine("Enter EXIT to Go to Main Menu");
                        Console.WriteLine($"Enter column number(0-{userBoardHeightInt - 1})");
                        Console.WriteLine("Your move");
                        Console.Write(">");

                        var userY = Console.ReadLine();
                        if (userY == "EXIT")
                        {
                            Menu.AgainstAi = false;
                            return "";
                        }

                        success = int.TryParse(userY, out userYInt);
                        if (!success || userYInt > _settings.BoardWidth)
                        {
                            Console.WriteLine($"{userY} is not a number or is out of board!");
                            success = false;
                        }
                    }
                } while (!success);

                _settings.MovesCounter += game.Move(userYInt);
                _settings.CellStates = game.GetBoard();
                using (var ctx = new AppDatabaseContext())
                {
                    if (ctx.Settings.Any(n => n.PlayerName == _settings.PlayerName))
                    {
                        ctx.Settings.Update(_settings);
                    }
                    else
                    {
                        ctx.Settings.Add(_settings);
                    }

                    ctx.SaveChanges();
                }


                done = Validator(_settings.CellStates, userBoardHeightInt, userBoardWidthInt);
            } while (!done);

            UserInterface.BoardPrint(game);
            using (var ctx = new AppDatabaseContext())
            {
                ctx.Attach(_settings);
                ctx.Remove(_settings);
                ctx.SaveChanges();
            }

            Console.WriteLine("Game Over!");
            return "";
        }

        public static string GameEngine(int height, int width)
        {
            _settings = new GameSettings {MovesCounter = 0, Time = DateTime.Now};
            if (Menu.AgainstAi)
            {
                _settings.AgainstAi = true;
            }

            var game = new Game(_settings);
            bool done;
            bool nameSet;

            do
            {
                Console.WriteLine("Enter your Name");
                Console.Write(">");
                _settings.PlayerName = Console.ReadLine();
                nameSet = _settings.PlayerName != null && _settings.PlayerName.Length >= 1;
                if (!nameSet)
                {
                    Console.WriteLine("Please enter your name!");
                    //_settings.Time = DateTime.Now;
                }
            } while (!nameSet);

            do
            {
                UserInterface.BoardPrint(game);
                int userYInt;
                bool success;

                do
                {
                    if (_settings.MovesCounter % 2 == 0 && _settings.AgainstAi)
                    {
                        Console.WriteLine("Mastermind AI move");
                        Random r = new Random();
                        userYInt = r.Next(0, _settings.BoardWidth - 1);
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine("Enter EXIT to Go to Main Menu");
                        Console.WriteLine($"Enter column number(0-{height - 1})");
                        Console.WriteLine("Your move");
                        Console.Write(">");

                        var userY = Console.ReadLine();
                        if (userY == "EXIT")
                        {
                            Menu.AgainstAi = false;
                            return "";
                        }

                        success = int.TryParse(userY, out userYInt);
                        if (!success || userYInt > _settings.BoardWidth)
                        {
                            Console.WriteLine($"{userY} is not a number or is out of board!");
                            success = false;
                        }
                    }
                } while (!success);

                _settings.MovesCounter += game.Move(userYInt);
                _settings.CellStates = game.GetBoard();
                //GameConfigHandler.SaveConfig(_settings);
                using (var ctx = new AppDatabaseContext())
                {
                    if (ctx.Settings.Any(n => n.PlayerName == _settings.PlayerName))
                    {
                        ctx.Settings.Update(_settings);
                    }
                    else
                    {
                        ctx.Settings.Add(_settings);
                    }

                    ctx.SaveChanges();
                }

                done = Validator(_settings.CellStates, height, width);
                //done = _settings.MovesCounter == height * width;
            } while (!done);

            UserInterface.BoardPrint(game);
            using (var ctx = new AppDatabaseContext())
            {
                ctx.Attach(_settings);
                ctx.Remove(_settings);
                ctx.SaveChanges();
            }

            Console.WriteLine("Game Over!");
            return "";
        }

        public static string LoadSavedGame()
        {
            var filenames = new List<string>();
            var fileNames2 = new Dictionary<int, string>();
            int savedFileInt;
            string savedFile;
            using var ctx = new AppDatabaseContext();
            do
            {
                Console.WriteLine("Saved Games :");
                foreach (var fileName in ctx.Settings)
                {
                    Console.WriteLine( fileName.GameSettingsId+" "+fileName.PlayerName + " " + fileName.Time.ToString("yyyy-MM-dd HH:mm"));
                    fileNames2.Add(fileName.GameSettingsId,fileName.PlayerName);
                    filenames.Add(fileName.PlayerName);
                }
                

                Console.WriteLine();
                Console.WriteLine("Enter saved game number");
                Console.WriteLine("Enter EXIT to leave");
                Console.Write(">");
                savedFile = Console.ReadLine();
                if (savedFile == "EXIT")
                {
                    return "";
                }

                var loadSuccess = false;
                loadSuccess = int.TryParse(savedFile, out savedFileInt);
                if (!loadSuccess)
                {
                    Console.WriteLine($"{savedFile} is not a number");
                }

                if (!ctx.Settings.Any(n => n.GameSettingsId == savedFileInt))
                {
                    Console.WriteLine("No Such Saved game!");
                }
            } while (!fileNames2.ContainsKey(savedFileInt));


            GameSettings savedGame = ctx.Settings.First(n => n.GameSettingsId == savedFileInt);
            if (savedGame != null)
            {
                ctx.Entry(savedGame).State = EntityState.Detached;
            }

            var game = new Game(savedGame);
            game.SetBoard(savedGame?.CellStates);
            bool done;
            do
            {
                UserInterface.BoardPrint(game);
                int userYInt;
                bool success;

                do
                {
                    if (savedGame?.MovesCounter % 2 == 0 && savedGame.AgainstAi)
                    {
                        Console.WriteLine("Mastermind AI move");
                        Random r = new Random();
                        userYInt = r.Next(0, savedGame.BoardWidth - 1);
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine("Enter EXIT to Go to Main Menu");
                        Console.WriteLine($"Enter column number(0-{savedGame?.BoardHeight - 1})");
                        Console.WriteLine("Your move");
                        Console.Write(">");

                        var userY = Console.ReadLine();
                        if (userY == "EXIT")
                        {
                            Menu.AgainstAi = false;
                            return "";
                        }

                        success = int.TryParse(userY, out userYInt);
                        if (!success || userYInt > savedGame?.BoardWidth)
                        {
                            Console.WriteLine($"{userY} is not a number or is out of board!");
                            success = false;
                        }
                    }
                } while (!success);


                savedGame.MovesCounter += game.Move(userYInt);
                savedGame.CellStates = game.GetBoard();

                ctx.Entry(savedGame).State = EntityState.Modified;
                //ctx.Settings.Update(savedGame);
                ctx.SaveChanges();

                done = Validator(savedGame.CellStates, savedGame.BoardHeight, savedGame.BoardWidth);
            } while (!done);
            UserInterface.BoardPrint(game);
            ctx.Attach(savedGame);
            ctx.Remove(savedGame);
            ctx.SaveChanges();
            Console.WriteLine("Game Over!");
            return "";
        }

        private static bool Validator(CellState[,]? savedGameCellStates, int y, int x)
        {
            y -= 1;
            x -= 1;
            for (var i = 0; i < y - 2; i++)
            {
                for (var j = 0; j < x - 2; j++)
                {
                    //horizontal check
                    if (!savedGameCellStates[y - i, j].Equals(CellState.Empty) &&
                        savedGameCellStates[y - i, j].Equals(savedGameCellStates[y - i, j + 1]) &&
                        savedGameCellStates[y - i, j + 1].Equals(savedGameCellStates[y - i, j + 2]) &&
                        savedGameCellStates[y - i, j + 2].Equals(savedGameCellStates[y - i, j + 3]))
                    {
                        return true;
                    }

                    //vertical check
                    if (!savedGameCellStates[i, j].Equals(CellState.Empty) &&
                        savedGameCellStates[i, j].Equals(savedGameCellStates[i + 1, j]) &&
                        savedGameCellStates[i + 1, j].Equals(savedGameCellStates[i + 2, j]) &&
                        savedGameCellStates[i + 2, j].Equals(savedGameCellStates[i + 3, j]))
                    {
                        return true;
                    }

                    //diagonal from LEFT
                    if (!savedGameCellStates[y - i, j].Equals(CellState.Empty) &&
                        savedGameCellStates[y - i, j].Equals(savedGameCellStates[y - i - 1, j + 1]) &&
                        savedGameCellStates[y - i - 1, j + 1].Equals(savedGameCellStates[y - i - 2, j + 2]) &&
                        savedGameCellStates[y - i - 2, j + 2].Equals(savedGameCellStates[y - i - 3, j + 3]))
                    {
                        return true;
                    }

                    //diagonal from RIGHT
                    if (!savedGameCellStates[y - i, x - j].Equals(CellState.Empty) &&
                        savedGameCellStates[y - i, x - j].Equals(savedGameCellStates[y - i - 1, x - j - 1]) &&
                        savedGameCellStates[y - i - 1, x - j - 1].Equals(savedGameCellStates[y - i - 2, x - j - 2]) &&
                        savedGameCellStates[y - i - 2, x - j - 2].Equals(savedGameCellStates[y - i - 3, x - j - 3]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}