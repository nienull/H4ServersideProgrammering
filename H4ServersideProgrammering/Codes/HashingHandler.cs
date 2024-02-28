using SQLitePCL;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace H4ServersideProgrammering.Codes;

public enum HashedValueReturnFormats
{
    SimpleString,
    ByteArrray,
    BitString,
    UtfString,
    HexadecimalString
}

/// <summary>
/// This class contains "Generic cryptographic hash functions".
/// </summary>
public class HashingHandler
{
    private byte[]? _inputBytes = null;

    public HashingHandler(string textToHash) 
    {
        _inputBytes = Encoding.ASCII.GetBytes(textToHash);
    }

    # region Standard hashing methods

    /// <summary>
    /// MD5 hashing is deprecated, but still OK for manipulation check.
    /// </summary>
    /// <param name="hashedValueReturnFormats"></param>
    /// <returns></returns>
    public dynamic MD5Hashing(HashedValueReturnFormats hashedValueReturnFormats)
    {
        MD5 md5 = MD5.Create();
        byte[] hashedValue = md5.ComputeHash(_inputBytes);

        return ReturnSpecifyedFormat(hashedValue, hashedValueReturnFormats);
    }

    public dynamic SHAHashing(HashedValueReturnFormats hashedValueReturnFormats)
    {
        // SH1 hashing is deprecated. Following is recommended istead: SHA256, SHA384, SHA3_256, SHA3_384, SHA3_512 and SHA512.
        //SHA1 sha = SHA1.Create();
        
        SHA256 sha = SHA256.Create();
        byte[] hashedValue = sha.ComputeHash(_inputBytes);

        return ReturnSpecifyedFormat(hashedValue, hashedValueReturnFormats);
    }

    public dynamic HMACHashing(HashedValueReturnFormats hashedValueReturnFormats)
    {
        // - HMAC == Hash-based Massage Authentication Code
        // - Uses a hash function and a secret key.
        // - Uses "Generic cryptographic hash functions" to hash, any of these can apply : MD5, SHA1, SHA256, SHA384, SHA3_256, SHA3_384, SHA3_512 and SHA512.
        HMACSHA256 hmac = new HMACSHA256();
        
        byte[] myKey = Encoding.ASCII.GetBytes("NielsErMinFavoritLærer");
        hmac.Key = myKey;
        
        byte[] hashedValue = hmac.ComputeHash(_inputBytes);

        return ReturnSpecifyedFormat(hashedValue, hashedValueReturnFormats);
    }

    #endregion

    #region Advance hashing methods

    /// <summary>
    /// Requires the Microsoft.AspNetCore.Cryptography.KeyDerivation package.
    /// </summary>
    /// <param name="textToHash"></param>
    /// <returns></returns>
    public dynamic PBKDF2Hashing(string salt, HashedValueReturnFormats hashedValueReturnFormats)
    {
        byte[] saltAsbytes = Encoding.ASCII.GetBytes(salt);
        var hashAlgorithm = new System.Security.Cryptography.HashAlgorithmName("SHA256");

        byte[] hashedValue = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(_inputBytes, saltAsbytes, 10, hashAlgorithm, 32);

        return ReturnSpecifyedFormat(hashedValue, hashedValueReturnFormats);
    }

    /// <summary>
    /// Requires the BCrypt.Net-Next package.
    /// </summary>
    /// <param name="textToHash"></param>
    /// <returns></returns>
    public static string BCryptHashing(string textToHash)
    {
        return BCrypt.Net.BCrypt.HashPassword(textToHash, 10, true);
        //return BCrypt.Net.BCrypt.HashPassword(textToHash, salt, true, BCrypt.Net.HashType.SHA256);
    }

    public static bool BCryptVerifyHashing(string textToHash, string hashedValue)
    {
        return BCrypt.Net.BCrypt.Verify(textToHash, hashedValue, true);
    }

    #endregion

    #region Formats for returning a hashed value 

    private dynamic ReturnSpecifyedFormat(byte[] hashedValue, HashedValueReturnFormats hashedValueReturnFormats)
    {
        switch (hashedValueReturnFormats)
        {
            case HashedValueReturnFormats.SimpleString:
                return Convert.ToBase64String(hashedValue);
            case HashedValueReturnFormats.ByteArrray:
                return hashedValue;
            case HashedValueReturnFormats.BitString:
                return BitConverter.ToString(hashedValue);
            case HashedValueReturnFormats.UtfString:
                return Encoding.UTF8.GetString(hashedValue, 0, hashedValue.Length);
            case HashedValueReturnFormats.HexadecimalString:
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashedValue)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            default:
                return null;
        }
    }

    #endregion

}