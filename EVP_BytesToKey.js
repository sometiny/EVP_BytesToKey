const md5 = require('./md5.js')
const chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];

function getBytes(source){
    if(!source) return [];
    if(source instanceof Array) return source;
    return Array.prototype.map.call(source, value => value.charCodeAt(0))
}

function getHexString(bytes){
    return bytes.map(t => chars[t >> 4] + chars[t & 0xf]).join('');
}

function getHashData(hash, data, salt){
    hash = hash || [];
    return hash.concat(data).concat(salt)
}

function entry(password, nkey = 32, niv = 16, hashAlgo = "md5", salt = null, round = 1)
{
    if(hashAlgo !== 'md5') throw new Error('only md5 supported');
    const key = [];
    const iv = [];
    const salt_ = getBytes(salt);
    const password_ = getBytes(password);

    const hashSize = 16;
    let hash = null;
    let i = 0;

    while (true)
    {
        const hashData = getHashData(hash, password_, salt_);
        hash = md5(hashData);

        for (i = 1; i < round; i++) hash = md5(hash);

        i = 0;
        while (nkey > 0 && i < hashSize)
        {
            key.push(hash[i]);
            nkey--;
            i++;
        }

        while (niv > 0 && i < hashSize)
        {
            iv.push(hash[i]);
            niv--;
            i++;
        }
        if (nkey == 0 && niv == 0) break;
    }

    return {key, iv, key_hex: getHexString(key), iv_hex: getHexString(iv)};
}

module.exports = entry;
