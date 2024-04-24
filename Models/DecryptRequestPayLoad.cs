using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankEncryptionAPI.Models
{
    public class DecryptRequestPayLoad
    {
        public string data { get; set; }
        public byte [] key { get; set; }
    }
}