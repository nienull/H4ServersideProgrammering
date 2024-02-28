using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace H4ServersideProgrammering.Codes;

public class SymetriskEncryptionHandler
{
    private readonly IDataProtector _proctor;
    public SymetriskEncryptionHandler(IDataProtectionProvider protector)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            _proctor = protector.CreateProtector(rsa.ToXmlString(false));
        }
    }

    public string EncryptSymetrisk(string textToEncrypt) => 
        _proctor.Protect(textToEncrypt);

    public string DecryptSymetrisk(string textToDecrypt) =>
        _proctor.Unprotect(textToDecrypt);
}
