/// <summary>
/// implement for openssl EVP_BytesToKey
/// </summary>
/// <param name="password">秘钥</param>
/// <param name="nkey">需要的新密钥长度</param>
/// <param name="niv">需要的IV长度</param>
/// <param name="hashAlgo">hash算法</param>
/// <param name="salt">salt</param>
/// <param name="round">hash轮数</param>
/// <returns></returns>
static (byte[], byte[]) EVP_BytesToKey(byte[] password, int nkey = 32, int niv = 16, string hashAlgo = "md5", byte[] salt = null, int round = 1)
{
    byte[] key = new byte[nkey];
    byte[] iv = new byte[niv];

    using var algo = HashAlgorithm.Create(hashAlgo);
    int hashSize = algo.HashSize >> 3;
    byte[] hash = new byte[hashSize];

    bool hashed = false;
    int i;

    while (true)
    {
        algo.Initialize();
        if (hashed) algo.TransformBlock(hash, 0, hashSize, null, 0);
        if (salt != null)
        {
            algo.TransformBlock(password, 0, password.Length, null, 0);
            algo.TransformFinalBlock(salt, 0, salt.Length);
        }
        else
        {
            algo.TransformFinalBlock(password, 0, password.Length);
        }
        hashed = true;
        hash = algo.Hash;

        for (i = 1; i < round; i++)
        {
            algo.Initialize();
            algo.TransformFinalBlock(hash, 0, hash.Length);
            hash = algo.Hash;
        }
        i = 0;
        while (nkey > 0 && i < hashSize)
        {
            key[^nkey] = hash[i];
            nkey--;
            i++;
        }

        while (niv > 0 && i < hashSize)
        {
            iv[^niv] = hash[i];
            niv--;
            i++;
        }
        if (nkey == 0 && niv == 0) break;
    }

    return (key, iv);
}
