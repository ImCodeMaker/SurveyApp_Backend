using System.Security.Cryptography;

public class HashingMethod : IHashingServices
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iteractions = 10000;
    private static readonly HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iteractions,hashAlgorithmName,KeySize);
        
        return string.Join(Delimiter,Convert.ToBase64String(salt),Convert.ToBase64String(hash));
    }
    public bool Verify(string HashedPassword, string InputPassword)
    {
        var elements =  HashedPassword.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(InputPassword,salt,Iteractions,hashAlgorithmName,KeySize);
        return CryptographicOperations.FixedTimeEquals(hash,hashInput);

    }
}