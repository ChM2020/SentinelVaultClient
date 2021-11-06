using SentinelVaultClient.Model;
using System;
using System.Linq;
using System.Security.Cryptography;




namespace SentinelVaultClient
{
    /// <summary>
    /// Provides Windows 10 based Client Support...
    /// </summary>
    public class Device
    {
        #region Settings              
        public static void SaveDeviceSecureIdentities(DeviceSecureIdentities ids)
        {
            Users.Default.DEVICEIDENTIES = ids.asJason();
            Users.Default.Save();
        }
        
        public static void SaveDeviceSecureIdentity(DeviceSecureIdentity id)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            // Check if existing
            if (ids.identities.Where(x=> x.label.Equals(id.label)).Any())
            {
                // Update
                ids.identities.RemoveAll(chunk => chunk.label == id.label);
                ids.identities.Add(id);
            }
            else
            {
                // Add
                ids.identities.Add(id);
            }
            // Save
            SaveDeviceSecureIdentities(ids);
        }
        #endregion
        #region KeyStorage
        public static byte[] GetHostECDHKey(string name)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            return id.host_sin_ecdh_PublicKeyBlob;
        }

        
        /// <summary>
        /// Generate and Store ECDSA Device Identity Key Pairs
        /// </summary>
        /// <param name="name">Local Label </param>
        /// <param name="sin">Secure Identity</param>
        /// <param name="host_sin">Host Secure Identity</param>
        /// <returns></returns>
        public static string GenECDSAKeys(string name, string host_sin, string api_token)
        {
            string sin = Provider.GenECDSAKeys(name);
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            if (id != null)
            {
                // Update Existing
                id.ecdsa_PublicKeyBlob = Provider.GetECDSAPubKey(name);
                id.sin = id.SecureIdentity();
                id.host_sin = host_sin;
                id.token = api_token;
            }
            else
            {
                // Add New
                id = new DeviceSecureIdentity
                {
                    label = name,
                    // ECDSA
                    ecdsa_PublicKeyBlob = Provider.GetECDSAPubKey(name)
                };
                id.sin = id.SecureIdentity();
                id.host_sin = host_sin;
                id.token = api_token;
            }
            // Save 
            SaveDeviceSecureIdentity(id);
            return id.sin;
        }
        /// <summary>
        /// Generate and Store ECDH Key Pairs
        /// </summary>
        public static void GenECDHKeys(string name)
        {
            Provider.GenECDHKeys(name);
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            if (id != null)
            {
                // Update Existing
                id.ecdh_PublicKeyBlob = Provider.GetECDHPubKey(name);
            }
            else
            {
                // Add New
                id = new DeviceSecureIdentity
                {
                    label = name,
                    // ECDH
                    ecdh_PublicKeyBlob = Provider.GetECDHPubKey(name)
                };
            }
            // Save 
            SaveDeviceSecureIdentity(id);
        }
        #endregion
        #region Support
        /// <summary>
        /// Sign the SHA256 hash of the data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SignHashData(string name, byte[] data)
        {
            using SHA256 dSHA256 = SHA256.Create();
            byte[] hashBytes = dSHA256.ComputeHash(data);
            return SignHash(name, hashBytes);
        }
        public static byte[] ComputeHash(byte[] data)
        {
            using SHA256 dSHA256 = SHA256.Create();
            byte[] hashBytes = dSHA256.ComputeHash(data);
            return hashBytes;
        }
        #endregion
        #region DeviceSecureIdentity
        /// <summary>
        /// Sign the SHA256 Hash
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static byte[] SignHash(string name, byte[] hash)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            byte[] signature = Provider.SignHash(hash, id.label);
            bool bResult = Provider.VerifyHash(hash, signature, name);
            if (bResult == false)
                throw new Exception("Error: Signature Verify Failed");
            return signature;
        }
        /// <summary>
        /// Sign Data, using provider key store
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SignData(string name, byte[] data)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            byte[] signature = Provider.SignData(data, id.label);
            bool bResult = Provider.VerifyData(data, signature, name);
            if (bResult == false)
                throw new Exception("Error: Signature Verify Failed");
            return signature;

        }
        /// <summary>
        /// Verify Hash Signature
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool VerifyHash(string name, byte[] hash, byte[] signature)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            return Provider.VerifyHash(hash, signature, id.label);
        }
        /// <summary>
        /// Verify SHA256 of data, signature
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool VerifyData(string name, byte[] data, byte[] signature)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = ids.GetDeviceSecureIdentity(name);
            return Provider.VerifyData(data, signature, id.label);
           
        }
        /// <summary>
        /// Retrieve ECDSA Public Key
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static byte[] GetECDSAPubKey(string keyName)
        {
            return Provider.GetECDSAPubKey(keyName);
        }
        /// <summary>
        /// Retrieve ECDH Public Key
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static byte[] GetECDHPubKey(string keyName)
        {
            return Provider.GetECDHPubKey(keyName);
        }
        #endregion

    }
}
