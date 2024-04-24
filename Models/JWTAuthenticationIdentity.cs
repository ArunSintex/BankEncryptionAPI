using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BankEncryptionAPI.Models
{
    public class JWTAuthenticationIdentity : GenericIdentity
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }


        public JWTAuthenticationIdentity(string clientId) : base(clientId)
        {
            this.clientId = clientId;
        }
    }
}