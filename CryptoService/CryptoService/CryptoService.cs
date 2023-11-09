using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;


namespace CryptoService
{
    public class CryptoService : ICryptoService
    {

        public BitArray shiftRight(BitArray registerArray, BitArray b)
        {

            for (int i = registerArray.Count - 1; i > 0; i--)
            {
                registerArray[i] = registerArray[i - 1];
            }
            bool result = b[0];
            for (int i = 1; i < b.Count; i++)
            {
                result ^= b[i];
            }
            registerArray[0] = result;
            return registerArray;

        }

        public bool majority_vote(BitArray b)
        {
            int sum = 0;
            for (int i = 0; i < b.Count; i++)
            {
                if (b[i] == true)
                {
                    sum++;
                }
            }

            if (sum >= 2)
                return true;
            else
                return false;
        }

        public byte[] A51_EncryptBytes(byte[] bytes)
        {
            BitArray bits = new BitArray(bytes);

            //postavim registre 
            int x_reg = Convert.ToInt32("0101000111001101100", 2);
            byte[] x_bytes = BitConverter.GetBytes(x_reg);
            BitArray x_bits = new BitArray(x_bytes);

            int y_reg = Convert.ToInt32("1100000111111001110001", 2);
            byte[] y_bytes = BitConverter.GetBytes(y_reg);
            BitArray y_bits = new BitArray(y_bytes);

            int z_reg = Convert.ToInt32("11001100011101001001100", 2);
            byte[] z_bytes = BitConverter.GetBytes(z_reg);
            BitArray z_bits = new BitArray(z_bytes);

            //kreiram nizove za step bitove
            BitArray x_step = new BitArray(4);
            BitArray y_step = new BitArray(3);
            BitArray z_step = new BitArray(4);


            BitArray major = new BitArray(3);
            BitArray result = new BitArray(bits.Count);


            for (int i = 0; i < bits.Count; i++)
            {
                major[0] = x_bits[7];
                major[1] = y_bits[7];
                major[2] = z_bits[10];
                bool m = majority_vote(major);

                x_step.Set(0, x_bits[3]);
                x_step.Set(1, x_bits[5]);
                x_step.Set(2, x_bits[12]);
                x_step.Set(3, x_bits[13]);

                y_step.Set(0, y_bits[14]);
                y_step.Set(1, y_bits[19]);
                y_step.Set(2, y_bits[20]);

                z_step.Set(0, z_bits[3]);
                z_step.Set(1, z_bits[7]);
                z_step.Set(2, z_bits[19]);
                z_step.Set(3, z_bits[20]);

                if (x_bits[7] == m)
                    x_bits = shiftRight(x_bits, x_step);
                if (y_bits[7] == m)
                    y_bits = shiftRight(y_bits, y_step);
                if (z_bits[10] == m)
                    z_bits = shiftRight(z_bits, z_step);

                bool res = x_bits[x_bits.Count - 1] ^ y_bits[y_bits.Count - 1] ^ z_bits[z_bits.Count - 1] ^ bits[i];
                result.Set(i, res);
            }

            byte[] codedBytes = new byte[bytes.Length];
            result.CopyTo(codedBytes, 0);

            return codedBytes;
        }

        public string A51_DecryptBytes(byte[] bytes1)
        {
            BitArray bits = new BitArray(bytes1);
            //postavim registre 
            int x_reg = Convert.ToInt32("0101000111001101100", 2);
            byte[] x_bytes = BitConverter.GetBytes(x_reg);
            BitArray x_bits = new BitArray(x_bytes);

            int y_reg = Convert.ToInt32("1100000111111001110001", 2);
            byte[] y_bytes = BitConverter.GetBytes(y_reg);
            BitArray y_bits = new BitArray(y_bytes);

            int z_reg = Convert.ToInt32("11001100011101001001100", 2);
            byte[] z_bytes = BitConverter.GetBytes(z_reg);
            BitArray z_bits = new BitArray(z_bytes);

            //kreiram nizove za step bitove
            BitArray x_step = new BitArray(4);
            BitArray y_step = new BitArray(3);
            BitArray z_step = new BitArray(4);


            BitArray major = new BitArray(3);
            BitArray result = new BitArray(bits.Count);


            for (int i = 0; i < bits.Count; i++)
            {
                major[0] = x_bits[7];
                major[1] = y_bits[7];
                major[2] = z_bits[10];
                bool m = majority_vote(major);

                x_step.Set(0, x_bits[3]);
                x_step.Set(1, x_bits[5]);
                x_step.Set(2, x_bits[12]);
                x_step.Set(3, x_bits[13]);

                y_step.Set(0, y_bits[14]);
                y_step.Set(1, y_bits[19]);
                y_step.Set(2, y_bits[20]);

                z_step.Set(0, z_bits[3]);
                z_step.Set(1, z_bits[7]);
                z_step.Set(2, z_bits[19]);
                z_step.Set(3, z_bits[20]);

                if (x_bits[7] == m)
                    x_bits = shiftRight(x_bits, x_step);
                if (y_bits[7] == m)
                    y_bits = shiftRight(y_bits, y_step);
                if (z_bits[10] == m)
                    z_bits = shiftRight(z_bits, z_step);

                bool res = x_bits[x_bits.Count - 1] ^ y_bits[y_bits.Count - 1] ^ z_bits[z_bits.Count - 1] ^ bits[i];
                result.Set(i, res);
            }

            int bytes = (result.Length + 7) / 8;
            byte[] arr2 = new byte[bytes];
            int bitIndex = 0;
            int byteIndex = 0;

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i])
                {
                    arr2[byteIndex] |= (byte)(1 << bitIndex);
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            string ret = Encoding.ASCII.GetString(arr2);
            return ret;

        }


        public byte[] loadBytes(string fileString)
        {
            byte[] bytes = File.ReadAllBytes(fileString);
            return bytes;
        }

        public void saveBytes(byte[] bytes, string fileString)
        {
            try
            {

                File.WriteAllBytes(fileString, bytes);
                Console.WriteLine("\n> Rezultat je sacuvan u fajlu sa nazivom:" + fileString);

            }
            catch (Exception)
            {
                Console.WriteLine("> File not found. Try again.");
            }
        }

        public void saveText(string text, string path)
        {
            try
            {
                File.WriteAllText(path, text);
                Console.WriteLine("\n> Rezultat je sacuvan u fajlu sa nazivom:" + path);

            }
            catch (Exception)
            {
                Console.WriteLine("> File not found. Try again.");
            }
        }

        public void writeToFileInt(string path, int[] arr)
        {
            File.WriteAllText(path, "");
            for (int i = 0; i < arr.Length; i++)
            {
                File.AppendAllText(path, arr[i].ToString());
                File.AppendAllText(path, ",");
            }
        }

        public int[] readFromFileInt(string path)
        {
            string text = File.ReadAllText(path);
            int counter = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].CompareTo(',') == 0)
                    counter++;
            }
            int[] result = new int[counter];
            string[] arr1 = text.Split(',');
            for (int i = 0; i < arr1.Length - 1; i++)
            {
                result[i] = Convert.ToInt32(arr1[i]);
            }
            return result;
        }
        public string readText(string path)
        {
            string res = File.ReadAllText(path);
            return res;
        }

        public int[] RSA_Encrypt(byte[] inputBytes, int p, int q)
        {
            int[] plainText = Array.ConvertAll(inputBytes, c => (int)c);
            int n = p * q;
            int fi = (p - 1) * (q - 1);
            int e = GeneratePublicKey(fi);
            int[] result = new int[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                int ret = plainText[i];
                for (int j = 1; j < e; j++)
                {
                    ret = (ret * plainText[i]) % n;
                }
                result[i] = ret;
            }


            return result;

        }

        public string RSA_Decrypt(int[] plainText, int p, int q)
        {
            int n = p * q;
            int fi = (p - 1) * (q - 1);
            int e = GeneratePublicKey(fi);
            int[] result = new int[plainText.Length];
            int br = 0;
            int d = 0;
            int x;
            while (d == 0)
            {
                x = br * fi + 1;
                if ((x / (double)e) % 1 == 0.0)
                {
                    d = x / e;
                }
                br++;
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                int ret = plainText[i];
                for (int j = 1; j < d; j++)
                {
                    ret = (ret * plainText[i]) % n;
                }
                result[i] = ret;
            }
            byte[] output = Array.ConvertAll(result, c => (byte)c);

            string ret1 = Encoding.ASCII.GetString(output);

            return ret1;
        }

        public int GeneratePublicKey(int phi)
        {
            int e = 2;
            while (GCD(e, phi) != 1)
            {
                e++;
            }
            return e;
        }

        public int GCD(int x, int y)
        {
            int gcd = 1;
            int temp;

            if (x > y)
            {
                temp = x;
                x = y;
                y = temp;
            }

            for (int i = 1; i < (x + 1); i++)
            {
                if (x % i == 0 && y % i == 0)
                    gcd = i;
            }
            return gcd;
        }

        public byte[] CFB_Encrypt(string plainText, string IV)
        {

            byte[] ivBytes = Encoding.ASCII.GetBytes(IV);
            BitArray ivBits = new BitArray(ivBytes);
            Console.WriteLine("IvBits:" + ivBits.Count);

            //enkriptujemo IV 
            byte[] blockCiph = A51_EncryptBytes(ivBytes);
            BitArray blockCipher = new BitArray(blockCiph);
        
            Console.WriteLine("BC:" + blockCipher.Count);

            //prevodimo plainText u niz bitova
            byte[] plainTextBytes = Encoding.ASCII.GetBytes(plainText);
        
            BitArray plainTextBits = new BitArray(plainTextBytes);

            int ostatak = plainTextBits.Count % blockCipher.Count;
            BitArray plainTextNew;

            //dopunjujemo plainTextNew bitovima do odgovarajuce duzine i u plainTextNew je sad nas osnovni plainText
            if (ostatak != 0)
            {
                plainTextNew = new BitArray(plainTextBits.Count + blockCipher.Count - ostatak);

                for (int i = 0; i < plainTextBits.Count; i++)
                {
                    plainTextNew.Set(i, plainTextBits[i]);
                }
                for (int i = plainTextBits.Count; i < plainTextNew.Count; i++)
                {
                    plainTextNew.Set(i, false);
                }
            }
            else
            {
                plainTextNew = new BitArray(plainTextBits.Count);

                for (int i = 0; i < plainTextBits.Count; i++)
                {
                    plainTextNew.Set(i, plainTextBits[i]);
                }

            }

            //pravimo blockTemp koji je izlaz iz prve faze algoritma
            BitArray blockTemp = new BitArray(blockCipher.Count);

            //Ciphertext
            BitArray output = new BitArray(plainTextNew.Count);

            //petlja za ostatak algoritma
            int j = 0;
            int pom;
            while (j < plainTextNew.Count)
            {

                for (int i = 0; i < blockCipher.Count; i++)
                {
                    pom = i + j;
                    bool res = blockCipher[i] ^ plainTextNew[pom];
                    blockTemp.Set(i, res);
                    output.Set(pom, res);
                }

                j = j + blockCipher.Count;
                byte[] blockTempBytes = new byte[blockCiph.Length];
                blockTemp.CopyTo(blockTempBytes, 0);
                byte[] blockResult = A51_EncryptBytes(blockTempBytes);
                blockCipher = new BitArray(blockResult);


            }


            int bytess = (output.Length + 7) / 8;
            byte[] arr = new byte[bytess];
            int bitIndexx = 0;
            int byteIndexx = 0;

            for (int k = 0; k < output.Length; k++)
            {
                if (output[k])
                {
                    arr[byteIndexx] |= (byte)(1 << bitIndexx);
                }

                bitIndexx++;
                if (bitIndexx == 8)
                {
                    bitIndexx = 0;
                    byteIndexx++;
                }
            }

            return arr;
        }

        public string CFB_Decrypt(byte[] plainTextNew1, string IV)
        {
            BitArray plainTextNew = new BitArray(plainTextNew1);
            //enkriptujemo IV 
            byte[] ivBytes = Encoding.ASCII.GetBytes(IV);
            BitArray ivBits = new BitArray(ivBytes);

            //BitArray blockCipher = A51_Encryptt(ivBits);
            byte[] blockCiph = A51_EncryptBytes(ivBytes);
            BitArray blockCipher = new BitArray(blockCiph);

            //pravimo blockTemp koji je izlaz iz prve faze algoritma
            BitArray blockTemp = new BitArray(blockCipher.Count);
            BitArray output = new BitArray(plainTextNew.Count);

            //petlja za ostatak algoritma
            int j = 0;
            int pom;
            while (j < plainTextNew.Count)
            {
                for (int i = 0; i < blockCipher.Count; i++)
                {
                    pom = i + j;
                    bool res = blockCipher[i] ^ plainTextNew[pom];
                    blockTemp.Set(i, plainTextNew[pom]);
                    output.Set(pom, res);
                }

                //prevodimo blockTemp u string da bi mogo dalje da se kriptuje

                j = j + blockCipher.Count;
                byte[] blockTempBytes = new byte[blockCiph.Length];
                blockTemp.CopyTo(blockTempBytes, 0);
                byte[] blockResult = A51_EncryptBytes(blockTempBytes);
                blockCipher = new BitArray(blockResult);

                Console.WriteLine("BlockCipher u petlji duzina:" + blockCipher.Count);
            }

            int bytess = (output.Length + 7) / 8; 
            byte[] arr = new byte[bytess];
            int bitIndexx = 0;
            int byteIndexx = 0;
            string ret1;

            for (int k = 0; k < output.Length; k++)
            {
                if (output[k])
                {
                    arr[byteIndexx] |= (byte)(1 << bitIndexx);
                }

                bitIndexx++;
                if (bitIndexx == 8)
                {
                    bitIndexx = 0;
                    byteIndexx++;
                }
            }


            ret1 = Encoding.ASCII.GetString(arr);
            string ret2 = ret1.TrimEnd(new char[] { (char)0 }); 

            return ret2;
        }


        public int Mod(int a, int b)
        {
            char[,] keySquare = new char[5, 5];
            return (a % b + b) % b;
        }

        public List<int> FindAllOccurrences(string str, char value)
        {
            List<int> indexes = new List<int>();

            int index = 0;
            while ((index = str.IndexOf(value, index)) != -1)
                indexes.Add(index++);

            return indexes;
        }

        public string RemoveAllDuplicates(string str, List<int> indexes)
        {
            string retVal = str;

            for (int i = indexes.Count - 1; i >= 1; i--)
                retVal = retVal.Remove(indexes[i], 1);

            return retVal;
        }

        public string RemoveOtherChars(string input)
        {
            string output = input;

            for (int i = 0; i < output.Length; ++i)
                if (!char.IsLetter(output[i]))
                    output = output.Remove(i, 1);

            return output;
        }

        public string AdjustOutput(string input, string output)
        {
            StringBuilder retVal = new StringBuilder(output);

            for (int i = 0; i < input.Length; ++i)
            {
                if (!char.IsLetter(input[i]))
                    retVal = retVal.Insert(i, input[i].ToString());

                if (char.IsLower(input[i]))
                    retVal[i] = char.ToLower(retVal[i]);
            }
            string rv = retVal.ToString();
            rv = rv.Replace("X", "");
            return rv;
        }

        public string Cipher(string input, string key, bool encipher)
        {
            string retVal = string.Empty;

            //generisanje keySquare-a
            char[,] keySquare = new char[5, 5];
            string defaultKeySquare = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            string tempKey = string.IsNullOrEmpty(key) ? "CIPHER" : key.ToUpper();

            tempKey = tempKey.Replace("J", "");
            tempKey += defaultKeySquare;

            for (int i = 0; i < 25; ++i)
            {
                List<int> indexes = FindAllOccurrences(tempKey, defaultKeySquare[i]);
                tempKey = RemoveAllDuplicates(tempKey, indexes);
            }

            tempKey = tempKey.Substring(0, 25);

            for (int i = 0; i < 25; ++i)
                keySquare[(i / 5), (i % 5)] = tempKey[i];


            string tempInput = RemoveOtherChars(input);
            int e = encipher ? 1 : -1;

            if ((tempInput.Length % 2) != 0)
                tempInput += "X";

            for (int i = 0; i < tempInput.Length; i += 2)
            {
                int row1 = 0;
                int col1 = 0;
                int row2 = 0;
                int col2 = 0;

                if (char.ToUpper(tempInput[i]) == 'J')
                {
                    for (int k = 0; k < 5; ++k)
                        for (int j = 0; j < 5; ++j)
                            if (keySquare[k, j] == 'I')
                            {
                                row1 = k;
                                col1 = j;
                            }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                        for (int j = 0; j < 5; ++j)
                            if (keySquare[k, j] == char.ToUpper(tempInput[i]))
                            {
                                row1 = k;
                                col1 = j;
                            }
                }


                if (char.ToUpper(tempInput[i + 1]) == 'J')
                {
                    for (int k = 0; k < 5; ++k)
                        for (int j = 0; j < 5; ++j)
                            if (keySquare[k, j] == 'I')
                            {
                                row2 = k;
                                col2 = j;
                            }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                        for (int j = 0; j < 5; ++j)
                            if (keySquare[k, j] == char.ToUpper(tempInput[i + 1]))
                            {
                                row2 = k;
                                col2 = j;
                            }
                }

                if (row1 == row2 && col1 == col2)
                {
                    retVal += new string(new char[] { keySquare[Mod((row1 + e), 5), Mod((col1 + e), 5)], keySquare[Mod((row1 + e), 5), Mod((col1 + e), 5)] });
                }
                else if (row1 == row2)
                {
                    retVal += new string(new char[] { keySquare[row1, Mod((col1 + e), 5)], keySquare[row1, Mod((col2 + e), 5)] });
                }
                else if (col1 == col2)
                {
                    retVal += new string(new char[] { keySquare[Mod((row1 + e), 5), col1], keySquare[Mod((row2 + e), 5), col1] });
                }
                else
                {
                    retVal += new string(new char[] { keySquare[row1, col2], keySquare[row2, col1] });
                }
            }

            retVal = AdjustOutput(input, retVal);

            return retVal;
        }

        public string PlayFair_Encrypt(string input, string key)
        {
            return Cipher(input, key, true);
        }

        public string PlayFair_Decrypt(string input, string key)
        {
            return Cipher(input, key, false);
        }

        public void PlayFair_EncryptParallel(string inputFile, string outputFile, int threadCount, string key)
        {
            int blockCount = threadCount;
            string ucitanText = File.ReadAllText(inputFile);
            int blockSize = ucitanText.Length / blockCount;
            string[] blocks = new string[blockCount];
            for (int i = 0; i < blockCount; i++)
            {
                int startIndex = i * blockSize;
                int endIndex = startIndex + blockSize;
                if (i == blockCount - 1)
                {
                    endIndex = ucitanText.Length;
                }
                blocks[i] = ucitanText.Substring(startIndex, endIndex - startIndex);
            }
 
            object lockObject = new object();
            var encryptedBlocks = new List<string>();
            Parallel.ForEach(blocks, new ParallelOptions { MaxDegreeOfParallelism = blockCount }, block =>
            {
                string ciphertext = PlayFair_Encrypt(block, key);
                lock (lockObject)
                {
                    encryptedBlocks.Add(string.Join("", ciphertext));
                }
            });
            WriteBlocksToFile(outputFile, encryptedBlocks);
        }

        public void WriteBlocksToFile(string file, List<string> blocks)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                foreach (string block in blocks)
                {
                    writer.Write(block);
                }
            }
        }

        public string SHA2Hash(string text)
        {
            SHA2 sha2 = new SHA2();
            return sha2.SHA2Hash(text);
        }
        public byte[] loadBitmap(string fileName)
        {
            byte[] slikaBajtovi = File.ReadAllBytes(fileName);
            return slikaBajtovi;
        }
        public void saveBitmap(byte[] slikaBajtovi, string fileName)
        {
            var ms = new MemoryStream(slikaBajtovi);
            Image slika = Image.FromStream(ms);

            slika.Save(fileName, ImageFormat.Bmp);
        }

    }
}
