using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankEncryptionAPI.Models
{
    public class EncryptedPayload
    {
        public string data { get; set; }
        public string key { get; set; }
        public int bit { get; set; }

        public override string ToString()
        {
            return $"data: {data}, key: {key}, bit: {bit}";
        }
    }
}