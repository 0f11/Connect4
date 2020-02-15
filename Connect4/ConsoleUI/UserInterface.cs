using System;
using System.ComponentModel;
using GameEngine;


namespace ConsoleUI
{
    public static class UserInterface
    {
        private const string VerticalSeparator = "|";
        private const string HorizontalSeparator = "-";
        private const string CenterSeparator = "+";

        public static void BoardPrint(Game game)
        {
            var board = game.GetBoard();
            for (int yIndex = 0; yIndex < game.BoardHeight; yIndex++)
            {
                var line = "";
                for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                {
                    line = line + " " + GetSingleState(board[yIndex, xIndex]) + " ";
                    if (xIndex < game.BoardWidth - 1)
                    {
                        line = line + VerticalSeparator;
                    }
                }

                Console.WriteLine(line);
                if (yIndex < game.BoardHeight - 1)
                {
                    line = "";
                    for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                    {
                        line = line + HorizontalSeparator + HorizontalSeparator + HorizontalSeparator;
                        if (xIndex < game.BoardWidth - 1)
                        {
                            line = line + CenterSeparator;
                        }
                    }

                    Console.WriteLine(line);
                }
            }
        }

        private static string GetSingleState(CellState state)
        {
            switch (state)
            {
                case CellState.Empty:
                    return " ";
                case CellState.O:
                    return "O";
                case CellState.X:
                    return "X";
                default:
                    throw new InvalidEnumArgumentException("Unknown enum!");
            }
        }
    }
}