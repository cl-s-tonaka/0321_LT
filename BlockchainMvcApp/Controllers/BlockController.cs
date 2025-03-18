using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BlockchainMvcApp.Models;
using BlockchainMvcApp.Services;
using System.Collections.Generic;

namespace BlockchainMvcApp.Controllers
{
    public class BlockController : Controller
    {
        private readonly MiningService _miningService;
        private static List<Block> _blocks = new List<Block>();
        private const string SessionKeyDifficulty = "MiningDifficulty";

        public BlockController(MiningService miningService)
        {
            _miningService = miningService;
        }

        public IActionResult Index()
        {
            int difficulty = HttpContext.Session.GetInt32(SessionKeyDifficulty) ?? 1;
            ViewBag.Difficulty = difficulty;
            return View(_blocks);
        }

        [HttpPost]
        public IActionResult MineNextBlock(int difficulty)
        {
            HttpContext.Session.SetInt32(SessionKeyDifficulty, difficulty);
            string previousHash = _blocks.Count > 0 ? _blocks[^1].Hash : "0000abc123";
            var newBlock = _miningService.MineBlock(_blocks.Count + 1, previousHash, $"Data {_blocks.Count + 1}", difficulty);
            _blocks.Add(newBlock);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult TamperBlock(int index)
        {
            if (index < _blocks.Count && index > 0)
            {
                _blocks[index].TamperData("Hacked Data");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RecalculateBlock(int index)
        {
            if (index < _blocks.Count && index > 0)
            {
                _blocks[index].RecalculateHash();
            }
            return RedirectToAction("Index");
        }
    }
}
