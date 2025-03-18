using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlockchainMvcApp.Models;

namespace BlockchainMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // プライバシーページ
        public IActionResult Privacy()
        {
            return View();
        }

        // ページ2: ブロックチェーンの構造
        public IActionResult Structure()
        {
            // Block モデルを使用
            var blockList = new List<BlockchainMvcApp.Models.Block>(); // 必要ならデータを設定
            return View("~/Views/Block/Structure.cshtml", blockList);
        }

        // ページ3: 改ざん検出public IActionResult Integrity()
        public IActionResult Integrity()
        {
            var blockList = new List<BlockchainMvcApp.Models.Block>(); // ここに適切なデータを設定
            ViewBag.IntegrityStatus = "正常";
            _logger.LogInformation("BlockList: " + (blockList == null ? "null" : blockList.Count.ToString()));
            return View("~/Views/Block/Integrity.cshtml", blockList);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
