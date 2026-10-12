using Microsoft.AspNetCore.Mvc;
using CoreMVC002.Models;
using System.Text.Json;

namespace CoreMVC002.Controllers
{
    public class XAXBController : Controller
    {
        private const string SessionKey = "XAXBGameSession";

        private XAXBEngine GetGameFromSession()
        {
            var sessionData = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionData))
            {
                var game = new XAXBEngine();
                game.GenerateSecretNumber();
                SaveGameToSession(game);
                return game;
            }
            return JsonSerializer.Deserialize<XAXBEngine>(sessionData);
        }

        private void SaveGameToSession(XAXBEngine game)
        {
            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));
        }

        public IActionResult Index()
        {
            var game = GetGameFromSession();
            return View(game);
        }

        [HttpPost]
        public IActionResult Guess(string userGuess)
        {
            var game = GetGameFromSession();

            if (game.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            // 簡單驗證輸入是否為 4 位數字
            if (string.IsNullOrEmpty(userGuess) || userGuess.Length != 4)
            {
                ModelState.AddModelError("", "請輸入 4 位數字！");
                return View("Index", game);
            }

            game.GuessCount++;
            var (a, b) = game.CheckGuess(userGuess);
            string resultStr = $"{a}A{b}B";

            game.History.Insert(0, new GuessRecord
            {
                Count = game.GuessCount,
                Guess = userGuess,
                Result = resultStr
            });

            if (a == 4)
            {
                game.IsSuccess = true;
            }

            SaveGameToSession(game);
            return View("Index", game);
        }

        public IActionResult Restart()
        {
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction("Index");
        }
    }
}
