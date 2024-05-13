using System.Security.Cryptography;
using System.Text;

namespace Messenger.Authorization;

public static class PasswordEncryptor
{
    public static byte[] CalculateHash(string password)
    {
        var saltedPass = "777" + password + "123";
        return MD5.HashData(Encoding.UTF8.GetBytes(saltedPass));
    }
}