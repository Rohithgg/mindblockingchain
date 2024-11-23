using blockChain.Consensus;
using blockChain.Model;
namespace blockChain;

public class BlockChain
{
    public List<Block> Chain { get; private set; }
    public static void Blocks(int index, string prvhash, DateTime timelog, string data)
    {
        var blockchain = new Block();
        blockchain.Index = index;
        blockchain.timelog = timelog;
        blockchain.Data = data;
        blockchain.PreviousHash = prvhash;
        blockchain.Hash = blockchain.CalculateHash();
    }
    public static void Main(string[] args)
    {
        var data = "Hello World";
        var difficulty = 2;
        var block = new Block
        {
            Index = 1,
            PreviousHash = "0",
            timelog = DateTime.Now,
            Data = data,
            Nonce = 0
        };
        // also try printing blockchainnode fucntion's display
        
        /*PoW.Mine(block, data, difficulty);
        Console.WriteLine("Mining started");
        
        Console.WriteLine($"hash {block.Hash}");
        Console.WriteLine($"\nNonce found: {block.Nonce}");
        Console.WriteLine($"Hash: {block.Hash}");
        Console.WriteLine($"Time elapsed: {block.TimeElapsed:F2} seconds");
        Console.WriteLine($"Hashes computed: {block.PreviousHash:N0}");
        //Console.WriteLine($"Hash rate: {block.PreviousHash / block.TimeElapsed:N0} H/s");// got error help
        Console.WriteLine($"Is valid: {PoW.verify(block)}");*/
        Block.DisplayBlockChain(block);
        
    }
}
// smart contract in enhance folder is a simple contract that can be used to create a blockchain
// then improve and make it more complex.

