using BlockchainMvcApp.Models;

namespace BlockchainMvcApp.Services
{
    public class MiningService
    {
        public Block MineBlock(int index, string previousHash, string data, int difficulty)
        {
            // ブロックのマイニング処理を担当
            return new Block(index, previousHash, data, difficulty);
        }

    }
}
