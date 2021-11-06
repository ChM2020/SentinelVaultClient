using SentinelVaultClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using vm.data.library.blockchain.api.device;
using vm.data.library.blockchain.api.device.Model;


namespace SentinelVaultClient
{
    public class Send
    {
       /// <summary>
       /// Vault Collectable Digital Object
       /// </summary>
       /// <param name="name">Provider Slot</param>
       /// <param name="obj">Collectable Object</param>
       /// <returns>Vaulted Collectable Object</returns>
        public static string  AddCollectable (string name, Collectable obj)
        {
            DeviceSecureIdentity id = Device.GetDeviceSecureIdentity(name);
            obj.ObjectID = Guid.NewGuid();
            // Digital Object
            obj.ContentHash = Device.ComputeHash(obj.Content);
            obj.ContentSignature = Device.SignHash(name, obj.ContentHash);
            // Creator
            obj.ObjectCreator = id.sin;
            obj.ObjectCreatorPublicKey = id.ecdsa_PublicKeyBlob;
            obj.ObjectCreatorExchangeKey = id.ecdh_PublicKeyBlob;
            // Owner
            obj.ObjectOwner = obj.ObjectCreator; // Make the same
            byte[] ownerObjHash = Device.ComputeHash(obj.ObjectID.ToByteArray().Concat(obj.ContentHash).ToArray());
            obj.ObjectOwnerSignature = Device.SignHash(name, ownerObjHash);
         
            // Add Host details
            obj.VaultSecureIdentity = id.host_sin;
            
           // Setup HTTP Post
            HttpClient _httpClient = new(); 
            String _uri = "http://localhost:7071/api/VaultAddObject";
            _httpClient.BaseAddress = new Uri(_uri);
            try
            {
                // Post Content
                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_uri, obj).Result;
                // Returned Vaulted object
                string sjson = response.Content.ReadAsStringAsync().Result;
                return sjson;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.ToString());
            }
        }


    }
}
