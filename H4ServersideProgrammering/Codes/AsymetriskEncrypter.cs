using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace H4ServersideProgrammering.Codes;

public class AsymetriskEncrypter
{
    public static string Encrypt(string textToEncrypt, string publicKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);

            byte[] dataToEncryptAsByteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
            byte[] encryptDataAsByteArray = rsa.Encrypt(dataToEncryptAsByteArray, true);
            var encryptedDataAsString = Convert.ToBase64String(encryptDataAsByteArray);

            return encryptedDataAsString;
        }
    }
}
