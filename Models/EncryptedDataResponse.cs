using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankEncryptionAPI.Models
{
    public class EncryptedDataResponse
    {
        public string data { get; set; }
        public byte[] key { get; set; }

        public override string ToString()
        {
            return $"data: {data}, key: {key}";
        }
    }
}