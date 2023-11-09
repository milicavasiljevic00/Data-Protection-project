using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoService
{
    internal class SHA2
    {
        private uint[] K = new uint[64] {
            0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5, 0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5,
            0xD807AA98, 0x12835B01, 0x243185BE, 0x550C7DC3, 0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174,
            0xE49B69C1, 0xEFBE4786, 0x0FC19DC6, 0x240CA1CC, 0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA,
            0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7, 0xC6E00BF3, 0xD5A79147, 0x06CA6351, 0x14292967,
            0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13, 0x650A7354, 0x766A0ABB, 0x81C2C92E, 0x92722C85,
            0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3, 0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070,
            0x19A4C116, 0x1E376C08, 0x2748774C, 0x34B0BCB5, 0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3,
            0x748F82EE, 0x78A5636F, 0x84C87814, 0x8CC70208, 0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2
        };

        private uint[] H = new uint[8] {
                0x6A09E667, 0xBB67AE85, 0x3C6EF372, 0xA54FF53A, 0x510E527F, 0x9B05688C, 0x1F83D9AB, 0x5BE0CD19
            };

        public string SHA2Hash(string plainText)
        {

            //preprocesiranje

            byte[] plainTextBytes = Encoding.ASCII.GetBytes(plainText);
            BitArray plainTextBitsStart = new BitArray(plainTextBytes);
            int br = 1;
            int p = plainTextBitsStart.Count;
            while ((p + br) % 512 != 448)
            {
                br++;
            }

            long len = Convert.ToInt64(p);
            byte[] plainLen = BitConverter.GetBytes(len);

            if (BitConverter.IsLittleEndian)
            {
                plainLen.Reverse();
            }

            BitArray plainLenBits = new BitArray(plainLen);
            Console.WriteLine("Duzina duzine teksta u bitovima (treba da bude 64): " + plainLenBits.Count);


            BitArray plainTextBits = new BitArray(plainTextBitsStart.Count + br + 64);

            for (int i = 0; i < plainTextBits.Count; i++)
            {
                if (i < plainTextBitsStart.Count)
                    plainTextBits.Set(i, plainTextBitsStart[i]);
                else if (i == plainTextBitsStart.Count)
                {
                    plainTextBits.Set(i, true);
                }
                else if (i % 512 < 448)
                {
                    plainTextBits.Set(i, false);
                }
                else
                {
                    plainTextBits.Set(i, plainLenBits[plainLenBits.Count - (plainTextBits.Count - i)]);
                }
            }

            //sad bi trebalo da je duzina poruke u bitovima deljiva sa 512
            //preprocesiranje zavrseno

            int t = plainTextBits.Count / 512;
            Console.WriteLine("T = " + t);

            //DO OVDE JE VALJDA OK
            BitArray[] blocks = new BitArray[t];

            for (int i = 0; i < blocks.Length; i++)
                blocks[i] = new BitArray(512);

            //podelim plainTextBits na blokove od po 512 bitova
            for (int i = 0; i < plainTextBits.Count; i++)
            {
                int b = i / 512;
                int r = i % 512;
                blocks[b].Set(r, plainTextBits[i]);
            }


            //petlja koja prolazi kroz svaki blok (od 512 bitova)
            for (int i = 0; i < t; i++)
            {
                processBlock(blocks[i]);
            }

            string result = "";

            //samo jos appenduj ove vrednosti
            for (int i = 0; i < 8; i++)
            {
                result += H[i].ToString();
            }

            return result;

        }

        public void processBlock(BitArray block)
        {
            uint[] W = new uint[64]; //64 32-bit reci
            BitArray[] wBits = new BitArray[64];

            //64 elementa od po 32 bita
            for (int i = 0; i < wBits.Length; i++)
                wBits[i] = new BitArray(32);

            for (int i = 0; i < block.Count; i++)
            {
                int bl = i / 32;
                int r = i % 32;
                wBits[bl].Set(r, block[i]);
            }

            for (int i = 0; i < 16; i++)
            {
                W[i] = BitArrayToInt(wBits[i]);
            }

            //nakon ovoga smo podelili prosledjeni blok na 16 32-bit delova

            uint s0;
            uint s1;

            //sada popunjavamo ostalih 48 BiArray-a iz wBits
            for (int i = 16; i < 64; i++)
            {
                s0 = RotateRight(W[i - 15], 7) ^ RotateRight(W[i - 15], 18) ^ (W[i - 15] >> 3);
                s1 = RotateRight(W[i - 2], 17) ^ RotateRight(W[i - 2], 19) ^ (W[i - 2] >> 10);
                W[i] = W[i - 16] + s0 + W[i - 7] + s1;
            }

            //inicijalizacija promenljivih stanja za aktuelni blok
            uint a = H[0],
               b = H[1],
               c = H[2],
               d = H[3],
               e = H[4],
               f = H[5],
               g = H[6],
               h = H[7];

            uint ma;
            uint ch;
            uint t1;  //pomocna promenljiva
            uint t2;  //pomocna promenljiva

            //runde:
            for (int i = 0; i < 64; i++)
            {
                s0 = RotateRight(a, 2) ^ RotateRight(a, 13) ^ RotateRight(a, 22);
                ma = (a & b) ^ (a & c) ^ (b & c);
                t2 = s0 + ma;
                s1 = RotateRight(e, 6) ^ RotateRight(e, 11) ^ RotateRight(e, 25);
                ch = (e & f) ^ ((~e) & g);
                t1 = h + s1 + ch + K[i] + W[i];

                h = g;
                g = f;
                f = e;
                e = d + t1;
                d = c;
                c = b;
                b = a;
                a = t1 + t2;
            }


            // Rezultat dobijen nakon svih rundi za aktuelni blok podataka se
            // dodaje u inicijalizacione promenljive


            H[0] = H[0] + a;
            H[1] = H[1] + b;
            H[2] = H[2] + c;
            H[3] = H[3] + d;
            H[4] = H[4] + e;
            H[5] = H[5] + f;
            H[6] = H[6] + g;
            H[7] = H[7] + h;

        }

        public uint RotateRight(uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

        public uint BitArrayToInt(BitArray input)
        {
            uint result = 0;
            uint bitCount = 1;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i]) result |= bitCount;

                bitCount = bitCount << 1;
            }

            return result;
        }
    }
}
