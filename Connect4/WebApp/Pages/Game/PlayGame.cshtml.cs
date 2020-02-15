
using System.Linq;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class PlayGame : PageModel
    {
        private readonly DAL.AppDatabaseContext _context;
        public readonly GameEngineWeb Engine;

        public PlayGame(AppDatabaseContext context)
        {
            _context = context;
            Engine = new GameEngineWeb(_context);
        }

        public ActionResult OnGet(int? gameId, int? col)
        {
            if (gameId == null)
            {
                return RedirectToPage("./GameStart");
            }
            
            Engine.LoadGameStateFromDb(gameId.Value);
            if (Engine.Validator(Engine.GetBoard(),Engine.BoardHeight,Engine.BoardWidth))
            {
                var settings = _context.Settings.First(n => n.GameSettingsId == gameId);
                _context.Attach(settings);
                _context.Remove(settings);
                _context.SaveChanges();
                return RedirectToPage("./Win");
            }
            //Engine.InitializeNewGame();
            if (col.HasValue)
            {
                Engine.Move(col.Value);
                
            }
            return Page();
        }
    }
}