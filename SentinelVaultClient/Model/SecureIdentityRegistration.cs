using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentinelVaultClient.Model
{
    public class SecureIdentityRegistrationResponse
    {
        public string SecureIdentity { get; set; }
        public string RecoveryToken { get; set; }
        public string JWTToken { get; set; }
        public byte[] HostECDHPublicKey { get; set; }
        public byte[] HostECDSAPublicKey { get; set; }
    }
}
