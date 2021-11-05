﻿using SentinelVaultClient.Model;
using System;
using System.IO;

namespace SentinelVaultClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1 Initialise Device Crypyo Support

            // The Digital Object Creator
            String labelCreator = "Sentinal-Creator";
            String vaultProviderSecureIdentity = String.Empty; // Obtain from your Vault Serive provider
            Device.GenECDSAKeys(labelCreator, vaultProviderSecureIdentity);
            Device.GenECDHKeys(labelCreator);

            // Step 2 Load Digital Object Content into Collectable
            Collectable obj = new Collectable();
            // Access Digital Object
            string path = String.Empty;  
            FileInfo fi = new FileInfo(path);
            obj.ObjectSize = fi.Length;
            obj.ObjectCreationDate = fi.CreationTime;
            obj.ObjectFileName = fi.Name;
            // Add Digital Object
            obj.Content = System.IO.File.ReadAllBytes(path);
            Send.AddCollectable(labelCreator, obj);


        }
    }
}
