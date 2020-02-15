using System;

namespace GameEngine
{
    public class Game
    {
        private CellState[,] Board { get; set; }
        public int BoardWidth { get; }
        public int BoardHeight { get; }

        private bool _playerZeroMove;

        public Game(GameSettings settings)
        {
            
            if (settings.BoardWidth < 7 || settings.BoardHeight < 6)
            {
                Console.WriteLine("Board has to be at least 6x7, beginner board initialized!");
            }

            BoardHeight = settings.BoardHeight;
            BoardWidth = settings.BoardWidth;
            Board = new CellState[BoardHeight, BoardWidth];
        }

        public CellState[,] GetBoard()
        {
            var result = new CellState[BoardHeight, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }

        public void SetBoard(CellState[,] cellState)
        {
            Board = cellState;
        }

        public int Move(int posX)
        {
            int y = 0;
            for (int i = 0; i < BoardHeight; i++)
            {
                if (Board[i,posX].Equals(CellState.Empty))
                {
                    y = i;
                }
            }
            
            if (Board[y, posX] != CellState.Empty)
            {
                Console.WriteLine("Position taken, choose new one!");
                return 0;
            }

            Board[y, posX] = _playerZeroMove ? CellState.X : CellState.O;
            _playerZeroMove = !_playerZeroMove;
            return 1;
        }
    }
}