namespace blockChain.Consensus;


public class PoS //proof of stake
{
    public string Address {get; private set;}
    public decimal Balance { get; set; }
    public PoS()
        {
            GenerateAddress();
        }
    
        private void GenerateAddress()
        {
            // In a real implementation, this would be a cryptographic public key
            Address = Guid.NewGuid().ToString();
        }
        // add validate blcok
}