using Newtonsoft.Json;
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
            String _uri = "https://functionsentinel.azurewebsites.net/api/VaultAddObject";
            _httpClient.BaseAddress = new Uri(_uri);
            _httpClient.DefaultRequestHeaders.Add("x-token", id.token); // Add API token for this Vault Provider
            try
            {
                // Generate Json Content
                var postContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                // Post Content
                HttpResponseMessage response = _httpClient.PostAsync(_uri, postContent).Result;
                // Process response
                if (response.IsSuccessStatusCode)
                {
                    // Returned Vaulted object
                    string sjson = response.Content.ReadAsStringAsync().Result;
                    return sjson;
                }
                else
                    return String.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.ToString());
            }
        }


    }
}
