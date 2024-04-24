using BankEncryptionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankEncryptionAPI.Services
{
    public interface ILoginService
    {
        Boolean validateClientDetails(ClientModel client);
        String GenererateTokenForClient(ClientModel client);
    }
}