using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using blockChain.Model;

namespace blockChain.Consensus;

public class PoW // proof of work
{
    public static string Mine(Block block)
    {
        while (block.Hash.Substring(0, 4) != "0000")
        {
            block.Nonce++;
            block.Hash = block.CalculateHash();
        }
        return block.Hash;
    }

    public static Block Mine(Block block, string data,int difficulty)
    {
        if (difficulty < 1) throw new ArgumentException("Difficulty must be greater than 0");
        
        string target = new string('0', difficulty);
        int nonce = 0;
        string hash;
        int hashesComputed = 0;
        Stopwatch stopwatch = Stopwatch.StartNew();

        using (SHA256 sha256 = SHA256.Create())
        {
            while (true)
            {
                string attemptString = $"{data}{nonce}";
                byte[] attemptBytes = Encoding.UTF8.GetBytes(attemptString);
                byte[] hashBytes = sha256.ComputeHash(attemptBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                hashesComputed++;

                // Check if hash meets difficulty requirement (starts with required number of zeros)
                if (hash.StartsWith(target))
                {
                    break;
                }
                nonce++;
            }
        }

        stopwatch.Stop();

        return new Block()
        {
            Nonce = nonce,
            Hash = hash,
            TimeElapsed = stopwatch.Elapsed.TotalSeconds,
            PreviousHash = null,
        };
    }
    public static bool verify(Block block)
    {
        return block.Hash == block.CalculateHash();
    }
}
