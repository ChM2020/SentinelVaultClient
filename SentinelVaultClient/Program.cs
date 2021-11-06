using SentinelVaultClient.Model;
using System;
using System.IO;

namespace SentinelVaultClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Source Code Respository 
            // https://github.com/ChM2020/SentinelVaultClient
            // The code is provided as is, with no WARRENTY or SUPPORT.
            //  Any support is via the Issues and Discussions facilities..
            //  https://github.com/ChM2020/SentinelVaultClient/issues , https://github.com/ChM2020/SentinelVaultClient/discussions


            // Step 1 Initialise Device Crypyo Support
            // The Digital Object Creator
            String labelCreator = "Sentinal-Creator"; // Change
            String vaultProviderSecureIdentity = String.Empty; // Obtain from your Vault Service provider
            String api_token = String.Empty; // Obtain API Token from your Vault Service Provider
            // Generate Creator Key Pairs
            Device.GenECDSAKeys(labelCreator, vaultProviderSecureIdentity, api_token);
            Device.GenECDHKeys(labelCreator);

            // Step 2 Load Digital Object Content into Collectable
            Collectable obj = new Collectable();
            // Access Digital Object
            string path = String.Empty; // HD red ballon from movie

            FileInfo fi = new FileInfo(path);
            obj.ObjectSize = fi.Length;
            obj.ObjectCreationDate = fi.CreationTime;
            obj.ObjectFileName = fi.Name;
            // Add Digital Object
            obj.Content = System.IO.File.ReadAllBytes(path);
            // Submit to Vault
            string json = Send.AddCollectable(labelCreator, obj);
            if (!String.IsNullOrEmpty(json))
            {
                // Save Vaulted Object
            }
        }
    }
}
