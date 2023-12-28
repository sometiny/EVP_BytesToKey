# EVP_BytesToKey
implementations of openssl EVP_BytesToKey: php, c#, javascript

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

### javascript测试
```javascript
const EVP_BytesToKey = require('./EVP_BytesToKey.js')

const password = '12345678901234561234567890123456';

const result = EVP_BytesToKey(password, 32, 16, 'md5', null, 1);
console.log(result)

/*
outputs
{
  key: [
    155, 105, 165,  70, 240, 197,  95,
    210,  45,  47,  91, 111, 220, 226,
    125, 235,  85, 216,  23, 181, 214,
     16,  72, 140, 250, 168, 140,  33,
    200, 160, 148, 235
  ],
  iv: [
     40,  83,  32,  24, 249, 160,
     88, 205, 248,  76,  11, 201,
    220, 125,  93, 138
  ],
  key_hex: '9b69a546f0c55fd22d2f5b6fdce27deb55d817b5d610488cfaa88c21c8a094eb',
  iv_hex: '28532018f9a058cdf84c0bc9dc7d5d8a'
}
*/
```



nodejs的createCipher方法使用的参数为：`EVP_BytesToKey(password, key_length, iv_length, 'md5', '', 1)`

即：使用MD5算法，salt为空，轮数为1
