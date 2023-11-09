using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CryptoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICryptoService" in both code and config file together.
    [ServiceContract]
    public interface ICryptoService
    {

        [OperationContract]
        byte[] A51_EncryptBytes(byte[] bytes);

        [OperationContract]
        string A51_DecryptBytes(byte[] bytes1);

        [OperationContract]
        BitArray shiftRight(BitArray registerArray, BitArray b);

        [OperationContract]
        bool majority_vote(BitArray b);

        [OperationContract]
        int[] RSA_Encrypt(byte[] inputBytes, int p, int q);

        [OperationContract]
        string RSA_Decrypt(int[] plainText, int p, int q);

        [OperationContract]
        int GeneratePublicKey(int phi);

        [OperationContract]
        int GCD(int x, int y);

        [OperationContract]
        byte[] CFB_Encrypt(string plainText, string IV);

        [OperationContract]
        string CFB_Decrypt(byte[] plainTextNew1, string IV);

        [OperationContract]
        int Mod(int a, int b);

        [OperationContract]
        List<int> FindAllOccurrences(string str, char value);

        [OperationContract]
        string RemoveAllDuplicates(string str, List<int> indexes);

        [OperationContract]
        string RemoveOtherChars(string input);

        [OperationContract]
        string AdjustOutput(string input, string output);

        [OperationContract]
        string Cipher(string input, string key, bool encipher);

        [OperationContract]
        string PlayFair_Encrypt(string input, string key);

        [OperationContract]
        string PlayFair_Decrypt(string input, string key);

        [OperationContract]
        void PlayFair_EncryptParallel(string inputFile, string outputFile, int threadCount, string key);

        [OperationContract]
        void WriteBlocksToFile(string file, List<string> blocks);

        [OperationContract]
        string SHA2Hash(string text);

        [OperationContract]
        byte[] loadBytes(string fileString);

        [OperationContract]
        void saveBytes(byte[] bytes, string fileString);

        [OperationContract]
        void saveText(string text, string path);

        [OperationContract]
        string readText(string path);

        [OperationContract]
        void writeToFileInt(string path, int[] arr);

        [OperationContract]
        int[] readFromFileInt(string path);

        [OperationContract]
        byte[] loadBitmap(string fileName);

        [OperationContract]
        void saveBitmap(byte[] slikaBajtovi, string fileName);
    }
}
