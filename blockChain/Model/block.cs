using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using blockChain.Consensus;

namespace blockChain.Model;

public class Block
{
    public List<Block> Chain { get; private set; }
    public int difficulty = 2;
    public Block()
    {
        Chain = new List<Block> { GenisisbBlock() };
    }
    public int Index { get; set; }// position.
    public string Data { get;  set; }
    public int Nonce { get; set; }// mining?? let see later
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public DateTime timelog { get; set; }
    public double TimeElapsed { get; set; }
    // public int HashesComputed { get; set; } 
    public string Validator { get; set; }
    
    public string CalculateHash()
    {
        using (var sha256 = SHA256.Create())
        {
            string rawData = $"{Index}{PreviousHash}{timelog}{Data}";// the heckkk
            byte[] bytes = Encoding.UTF8.GetBytes(rawData);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (var byt in hash)
            {
                sb.Append(byt.ToString("x2"));
            }
            return sb.ToString();
        } 
    }
    private Block GenisisbBlock()// something is wrong here
    {
        return new Block()
        {
            Index = 0,
            timelog = DateTime.Now,
            Data = "Genisis Block",
            PreviousHash = "0",
            Hash = "0",
            Nonce = 0
        };
    }
    
    public Block NextBlock(string data)
    {
        return new Block()
        {
            Index = Index + 1,
            timelog = DateTime.Now,
            Data = data,
            PreviousHash = Hash,
            Hash = CalculateHash()
        };
    }
    public Block GetLastBlock()
    {
        return Chain[Chain.Count - 1];
    }
    public bool IsValid()
    {
        return Hash == CalculateHash();
    }
    public bool IsChainValid()
    {
        for (var i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }
        return true;
    }

}
public class BlockchainNode
{
    private List<Block> blockchain;
    private Dictionary<string, PoS> wallets;
    private decimal minimumStake;

    public BlockchainNode(decimal minimumStake = 1000.0M)
    {
        blockchain = new List<Block>();
        wallets = new Dictionary<string, PoS>();
        this.minimumStake = minimumStake;

        // Create genesis block
        CreateGenesisBlock();
    }

    private void CreateGenesisBlock()
    {
        var genesisWallet = new PoS();
        genesisWallet.Balance = 1000000.0M;
        wallets[genesisWallet.Address] = genesisWallet;

        /*var genesisBlock = new Block(0, "Genesis Block", "0", PoS.Address);//static or non static, but whats with the PoS.Address Address variable is in Pos thingy  
        blockchain.Add(genesisBlock);*/
    }

    public void AddWallet(PoS wallet)
    {
        if (wallet.Balance >= minimumStake)
        {
            wallets[wallet.Address] = wallet;
            Console.WriteLine($"New wallet added: {wallet.Address} (Balance: {wallet.Balance})");
        }
        else
        {
            Console.WriteLine($"Wallet not added. Minimum stake required: {minimumStake}");
        }
    }

    public void AddBlock(string data) // what to do with this lot of shit have to change from pilot
    {
        var previousBlock = blockchain[blockchain.Count - 1];
        var validatorAddress = SelectValidator(previousBlock.Index);
        /*var newBlock = new Block(previousBlock.Index + 1, data, previousBlock.Hash, validatorAddress);// idk what to do with this, should i add parameters or not.
        blockchain.Add(newBlock);*/
        //Console.WriteLine($"New block added: {newBlock.Hash} (Validator: {newBlock.Validator})");
    }

    private string SelectValidator(int blockIndex)
    {
        // Simple implementation: Select the validator based on the last block's hash
        int validatorIndex = (int)(blockIndex % wallets.Count);
        int i = 0;
        foreach (var wallet in wallets.Values)
        {
            if (i == validatorIndex)
                return wallet.Address;
            i++;
        }
        return null;
    }

    public bool IsChainValid()
    {
        for (var i = 1; i < blockchain.Count; i++)
        {
            var currentBlock = blockchain[i];
            var previousBlock = blockchain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
                return false;

            if (currentBlock.PreviousHash != previousBlock.Hash)
                return false;

            if (!wallets.ContainsKey(currentBlock.Validator))
                return false;
        }
        return true;
    }

    public void DisplayBlockchain()
    {
        foreach (var block in blockchain)
        {
            Console.WriteLine("\nBlock Information:");
            Console.WriteLine($"Index: {block.Index}");
            Console.WriteLine($"Timestamp: {block.timelog}");
            Console.WriteLine($"Data: {block.Data}");
            Console.WriteLine($"Previous Hash: {block.PreviousHash}");
            Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine($"Validator: {block.Validator}");
        }
    }
}