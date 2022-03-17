using System;
using System.Text.Json;
using UnitSense.Extensions.Encryption;
using Xunit;

namespace UnitSense.Extensions.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        DummyModel token = new DummyModel()
        {
            ModelId = 4,
            GeneratedAt = DateTime.UtcNow.AddMinutes(-5)
        };

        var enc = token.GetToken("toto");

        var dec = DummyModel.FromCipher(enc, "toto");

        Assert.Equal(dec.ModelId, token.ModelId);
    }
}

public class DummyModel
{
    public int ModelId { get; set; }
    public DateTime GeneratedAt { get; set; }

    public static DummyModel FromCipher(string token, string password)
    {
        var dec = AesEncryption.Decrypt(token, password,
            AesEncryption.CipherStyle.Base58);
        return JsonSerializer.Deserialize<DummyModel>(dec);
    }

    public string GetToken(string password) => AesEncryption.Encrypt(JsonSerializer.Serialize(this), password,
        AesEncryption.CipherStyle.Base58);
}