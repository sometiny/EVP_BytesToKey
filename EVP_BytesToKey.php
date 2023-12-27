        /**
         * @param string $password
         * @param int $nkey 需要生成的密钥长度
         * @param int $niv 需要生成的iv长度
         * @param string $hash HASH算法，默认MD5
         * @param string $salt salt。默认为空
         * @param int $round HASH运行轮数
         * @return array
         */
        function EVP_BytesToKey(string $password, int $nkey = 32, int $niv = 16, string $hash = 'md5', string $salt = '', int $round = 1): array
        {
            $bytes = '';
            $last = '';
            $total = $nkey + $niv;

            while (strlen($bytes) < $total) {
                $last = hash($hash, $last . $password . $salt, true);

                for ($i = 1; $i < $round; $i++) $last = hash($hash, $last, true);

                $bytes .= $last;
            }

            return [
                'key' => substr($bytes, 0, $nkey),
                'iv' => substr($bytes, $nkey, $niv),
            ];

        }
