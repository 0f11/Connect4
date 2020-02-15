using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game
{
    public class LoadGame : PageModel
    {
        private readonly DAL.AppDatabaseContext _context;
        public readonly GameEngineWeb Engine;
        public List<GameSettings> gameData { get; set; }
        public int GameSettingsId { get; set; }

        public LoadGame(AppDatabaseContext context)
        {
            _context = context;
        }

        
        [BindProperty]
        public GameSettings GameSettings { get; set; }
        public async Task OnGetAsync()
        {
            gameData = await _context.Settings.ToListAsync();
        }
        public async Task<ActionResult> OnPost()
        {
            GameSettingsId = int.Parse(Request.Form["GameSettingsId"]);
            return RedirectToPage("./PlayGame", new{gameId = GameSettingsId});
            
        }
    }
}