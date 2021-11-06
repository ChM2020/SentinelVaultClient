using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SentinelVaultClient.Model
{
    public class DeviceSecureIdentity
    {
        public DeviceSecureIdentity()
        {

        }
           
        public DateTime createdDate { get; set; }
        public byte[] ecdh_PrivateKeyBlob { get; set; }
        public byte[] ecdh_PublicKeyBlob { get; set; }
        public byte[] ecdsa_PrivateKeyBlob { get; set; }
        public byte[] ecdsa_PublicKeyBlob { get; set; }
        public byte[] host_sin_ecdh_PublicKeyBlob { get; set; }
        public string host_sin { get; set; }
        public string sin { get; set; }
        public string label { get; set; }
        public string token { get; set; }

        public string SecureIdentity()
        {
            byte[] hashBytes = RIPEMD160.Create().ComputeHash(SHA256.Create().ComputeHash(ecdsa_PublicKeyBlob));
            return "0101" + ToHex(hashBytes);
        }
        private static string ToHex(byte[] bytes)
        {
            return string.Concat(Array.ConvertAll(bytes, b => b.ToString("X2")));
        }
    }

    public class DeviceSecureIdentities
    {
        public List<DeviceSecureIdentity> identities { get; set; }

        public DeviceSecureIdentities(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                this.identities = new List<DeviceSecureIdentity>();
            }
            else
            {
                List<DeviceSecureIdentity> identities = JsonConvert.DeserializeObject<List<DeviceSecureIdentity>>(json);
                 this.identities = identities;
               
            }
           
        }
    
        public string asJason()
        {
          return  JsonConvert.SerializeObject(identities);
        }
        public DeviceSecureIdentity GetDeviceSecureIdentity(string name)
        {
            DeviceSecureIdentities ids = new DeviceSecureIdentities(Users.Default.DEVICEIDENTIES);
            DeviceSecureIdentity id = (from DeviceSecureIdentity i in ids.identities
                                       where i.label.Equals(name)
                                       select i).SingleOrDefault();
            return id;
        }
        
       
    }
}
