
using System.IO;
using Newtonsoft.Json;

namespace GameEngine
{
    public static class GameConfigHandler
    {
        private const string FileName = "gamesettings";

        public static void SaveConfig(GameSettings settings)
        {
            using (var writer = File.CreateText($"SavedGames//{settings.PlayerName}.json"))
            {
                var jsonString = JsonConvert.SerializeObject(settings);
                writer.Write(jsonString);
            }
        }

        public static GameSettings LoadConfig(string fileName = FileName)
        {
            if (!Directory.Exists("SavedGames"))
            {
                Directory.CreateDirectory("SavedGames");
            }

            if (!File.Exists($"SavedGames//{fileName}.json"))
            {
                return new GameSettings();
            }

            var jsonString = File.ReadAllText($@"SavedGames//{fileName}.json");
            var res = JsonConvert.DeserializeObject<GameSettings>(jsonString);
            return res;
        }
    }
}