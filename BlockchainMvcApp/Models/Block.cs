using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace BlockchainMvcApp.Models
{
    public class Block
    {
        public int Index { get; }
        public string PreviousHash { get; }
        public string Data { get; set; } // 外部からデータを変更可能に
        public int Nonce { get; private set; }
        public string Hash { get; private set; }
        public int Difficulty { get; private set; }
        public long MiningTime { get; private set; }
        public DateTime Timestamp { get; }  // 追加：ブロックの生成時間

        public Block(int index, string previousHash, string data, int difficulty)
        {
            Index = index;
            PreviousHash = previousHash;
            Data = data;
            Difficulty = difficulty;
            Nonce = 0;
            Timestamp = DateTime.UtcNow; // ブロック生成時の時刻を記録
            Hash = MineBlock();
        }

        // 改ざん検出メソッド
        public bool IsTampered()
        {
            string calculatedHash = CalculateHash();
            return !calculatedHash.Equals(Hash);
        }

        // ハッシュ計算メソッド
        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Index}{PreviousHash}{Data}{Nonce}{Difficulty}{Timestamp}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string ComputeHash(int nonce)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Index}{PreviousHash}{Data}{nonce}{Timestamp}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }

        // ブロックのマイニング処理
        private string MineBlock()
        {
            string hash;
            string difficultyPrefix = new string('0', Difficulty);

            Stopwatch stopwatch = Stopwatch.StartNew();
            do
            {
                Nonce++;
                hash = ComputeHash(Nonce);
            } while (!hash.StartsWith(difficultyPrefix));

            stopwatch.Stop();
            MiningTime = stopwatch.ElapsedMilliseconds;
            return hash;
        }

        // 改ざん後に再マイニングを行う
        public void RecalculateHash()
        {
            Hash = MineBlock();
        }
    }
}
