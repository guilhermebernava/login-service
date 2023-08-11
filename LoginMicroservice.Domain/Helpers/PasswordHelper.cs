using LoginMicroservice.Domain.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace LoginMicroservice.Domain.Helpers;

public static class PasswordHelper
{
    public static PasswordDto GeneratePassword(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] combinedBytes = CombineBytes(Encoding.UTF8.GetBytes(password), salt);
        byte[] hashBytes = ComputeHash(combinedBytes);
        string hashString = Convert.ToBase64String(hashBytes);
        return new PasswordDto(hashString, salt);
    }

    public static bool ValidatePassword(string password,string hashPassword, byte[] salt)
    {
        byte[] combinedBytes = CombineBytes(Encoding.UTF8.GetBytes(password), salt);
        byte[] hashBytes = ComputeHash(combinedBytes);
        string stringHash = Convert.ToBase64String(hashBytes);

        return stringHash == hashPassword;
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16]; // 16 bytes = 128 bits
        using (var rng =  RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    private static byte[] CombineBytes(byte[] arr1, byte[] arr2)
    {
        byte[] combined = new byte[arr1.Length + arr2.Length];
        Buffer.BlockCopy(arr1, 0, combined, 0, arr1.Length);
        Buffer.BlockCopy(arr2, 0, combined, arr1.Length, arr2.Length);
        return combined;
    }

    private static byte[] ComputeHash(byte[] data)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(data);
    }
}
