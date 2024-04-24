using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankEncryptionAPI.Models
{
    public class Error
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public Exception developerMessage { get; set; }

        public override string ToString()
        {
            return $"ErrorCode: {errorCode}, Message: {message}, DeveloperMessage: {developerMessage?.ToString() ?? "N/A"}";
        }
    }
}