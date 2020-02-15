using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace GameEngine
{
    public class GameSettings
    {
        [MinLength(1)]
        [MaxLength(32)]
        [Display(Name="Player Name")]
        public string PlayerName { get; set; } = "Player";
        [Range(6,100, ErrorMessage = "Minimum is 6, Maximum 100")]
        [Display(Name="Board Height")]
        public int BoardHeight { get; set; } = 6;
        [Range(6,100, ErrorMessage = "Minimum is 7, Maximum 100")]
        [Display(Name="Board Width")]
        public int BoardWidth { get; set; } = 7;

        [NotMapped] public CellState[,]? CellStates { get; set; }
        [Column("CellStates")]
        public string CellStatesSerialized
        {
            get => JsonConvert.SerializeObject(CellStates);
            set => CellStates = JsonConvert.DeserializeObject<CellState[,]>(value);
        }

        public int MovesCounter { get; set; } = 0;
        public int GameSettingsId { get; set; }
        [Display(Name="Against Computer")]
        public bool AgainstAi { get; set; }

        public DateTime Time { get; set; }
    }
}