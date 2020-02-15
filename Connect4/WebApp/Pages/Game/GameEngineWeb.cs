using System;
using System.Linq;
using DAL;
using GameEngine;


namespace WebApp.Pages.Game
{
    public class GameEngineWeb
    {
        private readonly AppDatabaseContext _context;
        private CellState[,] Board { get; set; }
        private GameSettings _gameSettings;
        public int BoardWidth { get; private set; }
        public int BoardHeight { get; private set; }
        public string PlayerName { get; set; }
        public int GameId { get; set; }
        public int MoveCounter { get; set; }
        public bool AgainstAi { get; set; }

        public GameEngineWeb(AppDatabaseContext context)
        {
            _context = context;
        }

        public void LoadGameStateFromDb(int gameId)
        {
            _gameSettings = _context.Settings.First(n => n.GameSettingsId == gameId);

            BoardWidth = _gameSettings.BoardWidth;
            BoardHeight = _gameSettings.BoardHeight;
            GameId = _gameSettings.GameSettingsId;
            Board = _gameSettings.CellStates;
            MoveCounter = _gameSettings.MovesCounter;
            PlayerName = _gameSettings.PlayerName;
            AgainstAi = _gameSettings.AgainstAi;
        }


        public CellState GetBoardCellValue(int y, int x)
        {
            return Board[y, x];
        }

        public CellState[,] GetBoard()
        {
            var result = new CellState[BoardHeight, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }

        public void Move(int posX)
        {
            int y = 0;

            if (AgainstAi)
            {
                Random r = new Random();
                var x = r.Next(0, BoardWidth - 1);

                var h = 0;
                for (int i = 0; i < BoardHeight; i++)
                {
                    if (Board[i, x].Equals(CellState.Empty))
                    {
                        h = i;
                    }
                }

                if (Board[h, x] == CellState.Empty)
                {
                    Board[h, x] = MoveCounter % 2 != 0 ? CellState.X : CellState.O;
                    MoveCounter += 1;
                }
                else
                {
                    return;
                }
            }

            for (int i = 0; i < BoardHeight; i++)
            {
                if (Board[i, posX].Equals(CellState.Empty))
                {
                    y = i;
                }
            }

            if (Board[y, posX] == CellState.Empty)
            {
                Board[y, posX] = MoveCounter % 2 != 0 ? CellState.X : CellState.O;
                MoveCounter += 1;
                //There should be a pop up message with js  
            }
            else
            {
                return;
            }

            //_context.Entry(_gameSettings).State = EntityState.Detached;
            _gameSettings.MovesCounter = MoveCounter;
            _gameSettings.CellStates = Board;
            _context.Settings.Update(_gameSettings);
            _context.SaveChanges();
            if (Validator(Board, BoardHeight, BoardWidth))
            {
                
            }
        }

        public bool Validator(CellState[,] savedGameCellStates, int y, int x)
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