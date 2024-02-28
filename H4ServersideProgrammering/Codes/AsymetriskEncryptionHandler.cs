using System.Security.Cryptography;
namespace H4ServersideProgrammering.Codes;

public class AsymetriskEncryptionHandler
{
    private string _privateKey;
    private string _publicKey;

    public AsymetriskEncryptionHandler()
    {
        using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            _privateKey = rsa.ToXmlString(true);
            _publicKey = rsa.ToXmlString(false);
        }
    }

    public string EncryptAsymetrisk(string textToEncrypt)
    {
        return AsymetriskEncrypter.Encrypt(textToEncrypt, _publicKey);
    }

    public string DecryptAsymetrisk(string textToDecrypt)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(_privateKey);

            byte[] dataToDecryptAsByteArray = Convert.FromBase64String(textToDecrypt);
            byte[] decryptDataAsByteArray = rsa.Decrypt(dataToDecryptAsByteArray, true);
            var decryptedDataAsString = System.Text.Encoding.UTF8.GetString(decryptDataAsByteArray);

            return decryptedDataAsString;
        }
    }
}
