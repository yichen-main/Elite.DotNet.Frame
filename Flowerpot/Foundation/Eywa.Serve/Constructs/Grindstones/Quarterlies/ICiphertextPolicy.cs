namespace Eywa.Serve.Constructs.Grindstones.Quarterlies;
public interface ICiphertextPolicy
{
    string GetRefreshToken();
    string GenerateRandomIV();
    string GenerateRandomSalt();
    string PaddingKey(in string secret);
    string HashMD5(in string plainText);
    string HashSHA256(in string plainText);
    string EncodeBase64(in string plainText);
    string DecodeBase64(in string encryptedText);
    byte[] HmacSHA256(in string plainText, in byte[] salt);
    byte[] HmacSHA256(in string plainText, in string salt);
    string HmacSHA256ToHex(in string plainText, in string salt);
    string EncryptAES(in string plainText, in string salt, in string iv);
    string DecryptAES(in string encryptedText, in string salt, in string iv);
    Task<string> PullRootCodeAsync();
    Task PushRootCodeAsync(in string secret);
    string RootCodePath { get; }
}

[Dependent(ServiceLifetime.Singleton)]
file sealed partial class CiphertextPolicy : ICiphertextPolicy
{
    static readonly ReaderWriterLockSlim ReaderWriterLock = new();
    public string GetRefreshToken() => ProfileExpand.RandomEncode256Bits();
    public string GenerateRandomIV() => ProfileExpand.RandomInteger64Bits();
    public string GenerateRandomSalt()
    {
        var bytes = new byte[32];
        using (var generator = RandomNumberGenerator.Create()) generator.GetBytes(bytes);
        return bytes.ToEncodeBase64();
    }
    public string PaddingKey(in string secret) => secret.PadRight(32, '0');
    public string HashMD5(in string plainText) => plainText.ToMD5();
    public string HashSHA256(in string plainText) => plainText.ToSHA256();
    public string EncodeBase64(in string plainText) => plainText.ToEncodeBase64();
    public string DecodeBase64(in string encryptedText) => encryptedText.ToDecodeBase64();
    public byte[] HmacSHA256(in string plainText, in byte[] salt) => plainText.ToHmacSHA256(salt);
    public byte[] HmacSHA256(in string plainText, in string salt) => HmacSHA256(plainText, Encoding.UTF8.GetBytes(salt));
    public string HmacSHA256ToHex(in string plainText, in string salt) => HmacSHA256(plainText, salt).ToHexString();
    public string EncryptAES(in string plainText, in string salt, in string iv) => plainText.ToEncryptAES(salt, iv);
    public string DecryptAES(in string encryptedText, in string salt, in string iv) => encryptedText.ToDecryptAES(salt, iv);
    public Task<string> PullRootCodeAsync()
    {
        try
        {
            ReaderWriterLock.EnterReadLock();
            return File.Exists(RootCodePath) ? File.ReadAllTextAsync(RootCodePath) : Task.FromResult(string.Empty);
        }
        finally
        {
            ReaderWriterLock.ExitReadLock();
        }
    }
    public Task PushRootCodeAsync(in string secret)
    {
        try
        {
            ReaderWriterLock.EnterWriteLock();
            return File.WriteAllTextAsync(RootCodePath, secret.ToSHA256());
        }
        finally
        {
            ReaderWriterLock.ExitWriteLock();
        }
    }
    public string RootCodePath => FileLayout.GetModuleFilePath($"{FileLayout.GetProjectName(this).ToMD5()}{Extension.Log}");
}