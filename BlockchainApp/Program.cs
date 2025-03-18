using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

class Block
{
    public int Index { get; }
    public string PreviousHash { get; }
    public string Data { get; }
    public int Nonce { get; private set; }
    public string Hash { get; private set; }
    public int Difficulty { get; private set; } // 難易度（ゼロの桁数）
    public long MiningTime { get; private set; } // マイニング時間（ms）

    public Block(int index, string previousHash, string data, int difficulty)
    {
        Index = index;
        PreviousHash = previousHash;
        Data = data;
        Difficulty = difficulty;
        Nonce = 0;

        Hash = MineBlock();
    }

    private string ComputeHash(int nonce)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string rawData = $"{Index}{PreviousHash}{Data}{nonce}";
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }

    private string MineBlock()
    {
        string hash;
        string difficultyPrefix = new string('0', Difficulty); // 例: "0000" (4桁)

        Stopwatch stopwatch = Stopwatch.StartNew(); // 計測開始
        do
        {
            Nonce++;
            hash = ComputeHash(Nonce);
        } while (!hash.StartsWith(difficultyPrefix)); // 難易度の条件を満たすまでナンスを増やす

        stopwatch.Stop(); // 計測終了
        MiningTime = stopwatch.ElapsedMilliseconds; // マイニング時間を記録

        return hash;
    }

    public static int AdjustDifficulty(int currentDifficulty, long lastMiningTime, long targetTime)
    {
        if (lastMiningTime > targetTime * 1.2) return Math.Max(1, currentDifficulty - 1); // 遅い→難易度を下げる
        if (lastMiningTime < targetTime * 0.8) return currentDifficulty + 1; // 速い→難易度を上げる
        return currentDifficulty; // 目標に近いなら変更なし
    }
}

// **テスト**
class Program
{
    static void Main()
    {
        int difficulty = 4; // 初期難易度（ゼロ4桁）
        long targetTime = 5000; // 目標マイニング時間（ms）

        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"Mining Block {i}... (Difficulty: {difficulty})");
            Block block = new Block(i, "0000abc123", $"Data {i}", difficulty);
            Console.WriteLine($"Block {i} Mined! Nonce: {block.Nonce}, Hash: {block.Hash}, Time: {block.MiningTime}ms");

            // 難易度調整
            difficulty = Block.AdjustDifficulty(difficulty, block.MiningTime, targetTime);
        }
    }
}
