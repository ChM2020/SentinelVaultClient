using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentinelVaultClient.Model
{
    public class TransferOwnership
    {
        // Vaulted Object Identifier
        public Guid ObjectId { get; set; }

        // Existing Owner
        public string OwnerSecureIdentity { get; set; }
        public byte[] OwnerSignature { get; set; } // Signed (ObjectId | NewOwnerSecureIdentity)
        public byte[] OwnerVaultKey { get; set; }  // Derived Owner Key

        // New Owner
        public string NewOwnerSecureIdentity { get; set; }
        public byte[] NewOwnerEcdhPublicKey { get; set; } // Used to create new Owner VaultKey



    }
}
