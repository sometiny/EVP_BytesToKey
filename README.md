# EVP_BytesToKey
implements for openssl EVP_BytesToKey

### EVP_BytesToKey原理
数据都是基于二进制的，非hex
```text
hash = ''
all_hash = ''
while(all_hash长度 < 要求的key+iv的总长度){
  //第一轮HASH
  hash = HASH(hash + password + salt)

  //进行多轮HASH
  for(i = 1; i < round; i++) hash = HASH(hash)
  all_hash += hash
}

//取出key和iv
key = all_hash[0, key_length]
iv = all_hash[key_length, iv_length]
```

### C#测试
```csharp

(byte[] key, byte[] iv) = EVP_BytesToKey("12345678901234561234567890123456".ToASCIIBytes(), 32, 16, "md5", null, 1);

Console.WriteLine(key.ToHexString());
Console.WriteLine(iv.ToHexString());

/*
outputs
9b69a546f0c55fd22d2f5b6fdce27deb55d817b5d610488cfaa88c21c8a094eb
28532018f9a058cdf84c0bc9dc7d5d8a
*/
```



### PHP测试
```php

$key = '12345678901234561234567890123456';

$devd = EVP_BytesToKey($key, 32, 16, 'md5', '', 1);
$key = $devd['key'];
$iv = $devd['iv'];
echo bin2hex($key) . "\r\n";
echo bin2hex($iv);

/*
outputs
9b69a546f0c55fd22d2f5b6fdce27deb55d817b5d610488cfaa88c21c8a094eb
28532018f9a058cdf84c0bc9dc7d5d8a
*/
```
