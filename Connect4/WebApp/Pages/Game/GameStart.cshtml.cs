using System;
using System.Threading.Tasks;
using DAL;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class GameStart : PageModel
    {
        [BindProperty]
        public GameSettings GameSettings { get; set; }

        private readonly DAL.AppDatabaseContext _context;

        public GameStart(AppDatabaseContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            var gameSettings = new GameSettings()
            {
                BoardHeight = GameSettings.BoardHeight,
                BoardWidth = GameSettings.BoardWidth,
                PlayerName = GameSettings.PlayerName,
                CellStates = new CellState[GameSettings.BoardHeight,GameSettings.BoardWidth],
                AgainstAi = GameSettings.AgainstAi,
                Time = DateTime.Now


            };
            _context.Settings.Add(gameSettings);
            await _context.SaveChangesAsync();
            return RedirectToPage("./PlayGame", new{gameId = gameSettings.GameSettingsId});

        }
    }
    
    
}