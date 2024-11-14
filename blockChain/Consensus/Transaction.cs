using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using blockChain.Model;

namespace blockChain.Consensus;

public class Transaction
{
    public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal Amount { get; set; }
        public string Signature { get; set; }
}
public class Wallet
{
    public string? PrivateKey { get; set; }
    public string? PublicKey { get; set; }
    public string? Address { get; set; }
    public Wallet()
    {
        GenerateKeyPair();
    }
    
    private void GenerateKeyPair()
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
        {
            PrivateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            byte[] publicKeyBytes = rsa.ExportRSAPublicKey();
            Address = Convert.ToBase64String(publicKeyBytes);
        }
    }

    public void SignTransaction(Transaction transaction)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(PrivateKey), out _);
            byte[] dataToSign = Encoding.UTF8.GetBytes($"{transaction.SenderAddress}{transaction.ReceiverAddress}{transaction.Amount}");
            byte[] signature = rsa.SignData(dataToSign, CryptoConfig.MapNameToOID("SHA256"));
            transaction.Signature = Convert.ToBase64String(signature);
        }
    }

    public bool VerifyTransaction(Transaction transaction)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.ImportRSAPublicKey(Convert.FromBase64String(transaction.SenderAddress), out _);
            byte[] dataToVerify = Encoding.UTF8.GetBytes($"{transaction.SenderAddress}{transaction.ReceiverAddress}{transaction.Amount}");
            byte[] signature = Convert.FromBase64String(transaction.Signature);
            return rsa.VerifyData(dataToVerify, CryptoConfig.MapNameToOID("SHA256"), signature);
        }
    }
}

