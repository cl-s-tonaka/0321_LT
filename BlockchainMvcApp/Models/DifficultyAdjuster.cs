namespace BlockchainMvcApp.Models
{
    public class DifficultyAdjuster
    {
        public int AdjustDifficulty(int currentDifficulty, long lastMiningTime, long targetTime)
        {
            if (lastMiningTime > targetTime * 1.2)
                return Math.Max(1, currentDifficulty - 1);  // 難易度を下げる
            if (lastMiningTime < targetTime * 0.8)
                return currentDifficulty + 1;  // 難易度を上げる
            return currentDifficulty;  // 変更なし
        }
    }
}
