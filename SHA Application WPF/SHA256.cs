using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SHA_Console
{
    class SHA256
    {
        List<Chunk> messageChunks;
        private int nBitPerBlock = 512;

        public SHA256()
        {
            messageChunks = new List<Chunk>();
        }

        public string GetMessageDigest(string message)
        {
            string res = "";

            // Initialize Variables
            uint[] h = new uint[8] {
                0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
            };

            uint[] k = new uint[64] {
                   0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                   0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                   0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                   0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                   0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                   0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                   0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                   0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            };

            ChunkMessage(message);
            //Console.WriteLine(RightRotate((uint)2, 2));
            //Console.WriteLine(messageChunks.Count);
            foreach (Chunk chunk in messageChunks)
            {
                uint[] w = new uint[64];
                chunk.GetWords().CopyTo(w, 0);

                uint S0, S1;

                for (int i = 16; i < 64; i++)
                {
                    S0 = RightRotate(w[i - 15], 7) ^ RightRotate(w[i - 15], 18) ^ (w[i - 15] >> 3);
                    S1 = RightRotate(w[i - 2], 17) ^ RightRotate(w[i - 2], 19) ^ (w[i - 2] >> 10);

                    w[i] = w[i - 16] + S0 + w[i - 7] + S1;
                    //Console.WriteLine("w[" + i + "] : " + w[i]);
                }

                uint[] a = new uint[8];

                for (int i = 0; i < 8; i++)
                {
                    a[i] = h[i];
                }

                for (int i = 0; i < 64; i++)
                {
                    S1 = RightRotate(a[4], 6) ^ RightRotate(a[4], 11) ^ RightRotate(a[4], 25);
                    uint ch = (a[4] & a[5]) ^ ((~a[4]) & a[6]);
                    uint temp = a[7] + S1 + ch + k[i] + w[i];
                    a[3] = a[3] + temp;
                    S0 = RightRotate(a[0], 2) ^ RightRotate(a[0], 13) ^ RightRotate(a[0], 22);
                    uint maj = (a[0] & (a[1] ^ a[2])) ^ (a[1] & a[2]);
                    temp = temp + S0 + maj;

                    for (int j = 7; j > 0; j--)
                    {
                        a[j] = a[j - 1];
                    }
                    a[0] = temp;
                }

                for (int i = 0; i < 8; i++)
                {
                    h[i] += a[i];
                }
            }

            //foreach (Chunk chunk in messageChunks)
            //{
            //    chunk.Print();
            //}

            for (int i = 0; i < 8; i++)
            {
                res += h[i].ToString("X") + " ";
            }

            //Console.WriteLine(res);
            return res;
        }

        public string GetMessageDigestFromFile(string fileName)
        {
            string res = "";

            // Initialize Variables
            uint[] h = new uint[8] {
                0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
            };

            uint[] k = new uint[64] {
                   0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                   0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                   0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                   0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                   0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                   0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                   0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                   0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            };

            ChunkMessageFile(fileName);
            //Console.WriteLine(RightRotate((uint)2, 2));
            //Console.WriteLine(messageChunks.Count);
            foreach (Chunk chunk in messageChunks)
            {
                uint[] w = new uint[64];
                chunk.GetWords().CopyTo(w, 0);

                uint S0, S1;

                for (int i = 16; i < 64; i++)
                {
                    S0 = RightRotate(w[i - 15], 7) ^ RightRotate(w[i - 15], 18) ^ (w[i - 15] >> 3);
                    S1 = RightRotate(w[i - 2], 17) ^ RightRotate(w[i - 2], 19) ^ (w[i - 2] >> 10);
                    
                    w[i] = w[i - 16] + S0 + w[i - 7] + S1;
                    //Console.WriteLine("w["+i+"] : " + w[i]);
                }

                uint[] a = new uint[8];

                for (int i = 0; i < 8; i++)
                {
                    a[i] = h[i];
                }

                for (int i = 0; i < 64; i++)
                {
                    S1 = RightRotate(a[4],6) ^ RightRotate(a[4],11) ^ RightRotate(a[4],25);
                    uint ch = (a[4] & a[5]) ^ ((~a[4]) & a[6]);
                    uint temp = a[7] + S1 + ch + k[i] + w[i];
                    a[3] = a[3] + temp;
                    S0 = RightRotate(a[0],2) ^ RightRotate(a[0],13) ^ RightRotate(a[0],22);
                    uint maj = (a[0] & (a[1] ^ a[2])) ^ (a[1] & a[2]);
                    temp = temp + S0 + maj;

                    for(int j = 7; j > 0; j--) {
                        a[j] = a[j-1];
                    }
                    a[0] = temp;
                }

                for (int i = 0; i < 8; i++)
                {
                    h[i] += a[i];
                }
            }

            //foreach (Chunk chunk in messageChunks)
            //{
            //    chunk.Print();
            //}

            for (int i = 0; i < 8; i++)
            {
                res += h[i].ToString("X") + " ";
            }

            //Console.WriteLine(res);
            return res;
        }

        private int RightRotate(int i, int bits)
        {
            return ((i >> bits) | (i << (32-bits)));
        }

        private uint RightRotate(uint i, int bits)
        {
            return ((i >> bits) | (i << (32 - bits)));
        }

        private long RightRotate(long i, int bits)
        {
            return ((i >> bits) | (i << (32 -bits)));
        }

        private int LeftRotate(int i, int bits)
        {
            return ((i << bits) | (i >> (32 -bits)));
        }

        private uint LeftRotate(uint i, int bits)
        {
            return ((i << bits) | (i >> (32 - bits)));
        }

        private void ChunkMessageFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            messageChunks.Clear();

            int n_byte = nBitPerBlock / 8;
            int offset = 0;
            byte[] temp = new byte[n_byte];
            int length = fs.Read(temp, offset, n_byte);

            //Console.WriteLine(Encoding.ASCII.GetString(temp));
            Chunk chunk;

            while (length == n_byte)
            {
                chunk = new Chunk();

                for (int i = 0; i < n_byte; i += 4)
                {
                    chunk.Add32Bit(BytesToUInt(new byte[4] { temp[i], temp[i+1], temp[i+2], temp[i+3] }));
                }

                messageChunks.Add(chunk);

                offset += length;
                length = fs.Read(temp, 0, n_byte);
                //Console.WriteLine(Encoding.ASCII.GetString(temp));
            }
            //Console.WriteLine(Encoding.ASCII.GetString(temp) +":"+ length);

            int k_zero = n_byte - ((int)(fs.Length % n_byte)) - 64 / 8;
            if (k_zero == 0) k_zero = 448 / 8;

            //Console.WriteLine("Zero adding : "+ k_zero);

            byte[] bytes_to_append = new byte[k_zero];
            bytes_to_append[0] = 128;
            for (int i = 1; i < k_zero; i++)
            {
                bytes_to_append[i] = 0;
            }
            
            bytes_to_append = bytes_to_append.Concat(LongToBytes(fs.Length*8,64/8)).ToArray();

            int i_append = 0;
            if (length > 0)
            {
                for (int i = length; i < n_byte; i++)
                {
                    temp[i] = bytes_to_append[i_append++];
                }

                chunk = new Chunk();

                for (int i = 0; i < n_byte; i += 4)
                {
                    chunk.Add32Bit(BytesToUInt(new byte[4] { temp[i], temp[i + 1], temp[i + 2], temp[i + 3] }));
                }
                messageChunks.Add(chunk);
            }
            //Console.WriteLine("Length of Bit Append :" + bytes_to_append.Count());
            //Console.WriteLine("Index of Bit Append :" + i_append);
            while (i_append < bytes_to_append.Count())
            {
                chunk = new Chunk();
                for (int i = 0; i < n_byte; i += 4)
                {
                    chunk.Add32Bit(BytesToUInt(new byte[4] { bytes_to_append[i_append], bytes_to_append[i_append+1], 
                    bytes_to_append[i_append+2], bytes_to_append[i_append+3] }));
                    i_append += 4;
                }
                messageChunks.Add(chunk);
            }

            fs.Close();
        }

        private void ChunkMessage(string message)
        {

            messageChunks.Clear();

            int n_byte = nBitPerBlock / 8;
            int offset = 0;
            byte[] temp = new byte[n_byte];

            int length = (message.Length-offset) > n_byte ? n_byte : message.Length-offset;
            Encoding.ASCII.GetBytes(message.Substring(offset, length)).CopyTo(temp, 0);

            Chunk chunk;

            while (length == n_byte)
            {
                chunk = new Chunk();

                for (int i = 0; i < n_byte; i += 4)
                {
                    chunk.Add32Bit(BytesToUInt(new byte[4] { temp[i], temp[i + 1], temp[i + 2], temp[i + 3] }));
                }

                messageChunks.Add(chunk);

                offset += length;
                length = (message.Length-offset) > n_byte ? n_byte : message.Length-offset;
                Encoding.ASCII.GetBytes(message.Substring(offset, length)).CopyTo(temp, 0);
                Console.WriteLine(Encoding.ASCII.GetString(temp));
            }
            //Console.WriteLine(Encoding.ASCII.GetString(temp) + ":" + length);

            int k_zero = n_byte - ((int)(message.Length % n_byte)) - 64 / 8;
            if (k_zero == 0) k_zero = 448 / 8;

            //Console.WriteLine("Zero adding : " + k_zero);

            byte[] bytes_to_append = new byte[k_zero];
            bytes_to_append[0] = 128;
            for (int i = 1; i < k_zero; i++)
            {
                bytes_to_append[i] = 0;
            }

            bytes_to_append = bytes_to_append.Concat(LongToBytes(message.Length * 8, 64 / 8)).ToArray();

            int i_append = 0;
            if (length > 0)
            {
                for (int i = length; i < n_byte; i++)
                {
                    temp[i] = bytes_to_append[i_append++];
                }

                chunk = new Chunk();

                for (int i = 0; i < n_byte; i += 4)
                {
                    chunk.Add32Bit(BytesToUInt(new byte[4] { temp[i], temp[i + 1], temp[i + 2], temp[i + 3] }));
                }
                messageChunks.Add(chunk);
            }
            //Console.WriteLine("Length of Bit Append :" + bytes_to_append.Count());
            //Console.WriteLine("Index of Bit Append :" + i_append);
            while (i_append < bytes_to_append.Count())
            {
                chunk = new Chunk();
                for (int i = 0; i < n_byte; i += 4)
                {
                    chunk.Add32Bit(BytesToUInt(new byte[4] { bytes_to_append[i_append], bytes_to_append[i_append+1], 
                    bytes_to_append[i_append+2], bytes_to_append[i_append+3] }));
                    i_append += 4;
                }
                messageChunks.Add(chunk);
            }
        }

        private long StringToLong(string s)
        {
            long res = 0;
            foreach(char c in s) {
                res = res << 8;
                res += (int)c;
            }
            return res;
        }

        private long BytesToLong(byte[] bytes)
        {
            long res = 0;
            foreach (byte b in bytes)
            {
                res = res << 8;
                res += (int)b;
            }
            return res;
        }

        private byte[] LongToBytes(long _long, int length)
        {
            byte[] res = new byte[length];
            int i = length - 1;
            while (_long > 0)
            {
                res[i--] = (byte) (_long % 256);
                _long = _long / 256;
            }
            return res;
        }

        private int BytesToInt(byte[] bytes)
        {
            int res = 0;
            foreach (byte b in bytes)
            {
                res = res << 8;
                res += (int)b;
            }
            return res;
        }

        private uint BytesToUInt(byte[] bytes)
        {
            uint res = 0;
            foreach (byte b in bytes)
            {
                res = res << 8;
                res += (uint)b;
            }
            return res;
        }

        #region Chunk
        class Chunk
        {
            List<uint> chunkBlocks;

            public Chunk()
            {
                chunkBlocks = new List<uint>();
            }

            public uint[] GetWords()
            {
                return chunkBlocks.ToArray();
            }

            public void Add32Bit(uint x) {
                chunkBlocks.Add(x);
            }

            public void Print()
            {
                //Console.WriteLine("Chunk Length : " + chunkBlocks.Count);
                foreach (uint i in chunkBlocks)
                {
                    //Console.Write(i + " ");
                }
                //Console.WriteLine();
            }
        }
        #endregion
    }
}
