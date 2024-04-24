using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Jose;
using System.Text;
using BankEncryptionAPI.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Configuration;

namespace BankEncryptionAPI.common
{
    public class CommonUtility
    {
        public static string EncryptKey(string key)
        {

            /*string publiccertstr_UAT = "MIIDojCCAoqgAwIBAgIIBmMSCJVcNv4wDQYJKoZIhvcNAQELBQAwOzELMAkGA1UE"
                + "BhMCSU4xETAPBgNVBAoMCGluZHVzaW5kMRkwFwYDVQQDDBBpbmR1c2luZC1lbmMt"
                + "ZGVjMB4XDTIyMDcxMjA3NDAxMVoXDTMyMDcwOTA3NDAxMVowOzELMAkGA1UEBhMC"
                + "SU4xETAPBgNVBAoMCGluZHVzaW5kMRkwFwYDVQQDDBBpbmR1c2luZC1lbmMtZGVj"
                + "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAu60AzxMOMrBQ4zrsyh4y"
                + "ftU82X+bUz5NqVAa7kvrHJQVawqfQJiI6T72tFDULHxyiBXu+zOmPQH9WGIk9Rri"
                + "IIAUT6iRKtmLfk7ihZkVoYSbvN3mKFAhOGghBJmlJeEL301yhU38y2Nu/nx0mm/Y"
                + "/r5DsSAzhet+U5GNBL8fYo0uOZ9Ooziuv9h+nqX0u2tcIPJmausesw42ceXXDJul"
                + "YjHOMIRg8cyidWSIYLEdebxocOzXuq9hcpoxF45F5br9+syYuQSqzSYDj02xRcee"
                + "nU/rh78Al4cRcYDTmQ6OrZL+OrAcUjiqkR+mX+QKPI5vpo4I5cQMIzkSg+SQFevW"
                + "BwIDAQABo4GpMIGmMAwGA1UdEwQFMAMBAf8wHQYDVR0OBBYEFDlKyoJrELE0Ftrz"
                + "WSdZQNTGzCEYMGoGA1UdIwRjMGGAFDlKyoJrELE0FtrzWSdZQNTGzCEYoT+kPTA7"
                + "MQswCQYDVQQGEwJJTjERMA8GA1UECgwIaW5kdXNpbmQxGTAXBgNVBAMMEGluZHVz"
                + "aW5kLWVuYy1kZWOCCAZjEgiVXDb+MAsGA1UdDwQEAwICvDANBgkqhkiG9w0BAQsF"
                + "AAOCAQEAs3VlD7kLZZ7TH9S4KGm5s+5feJdl7Xnjq1f+GE8lSKC7hPgHoeiCHb2r"
                + "7TNWHszhHvBfMfYXPk0Pb60q2VaDZYQbcaetoZsyP33/S/ZxjMIL3KVb9sp7kMXI"
                + "JTby+SqXNxAipoO0RJapiaEBidOgRspYFAjjgeGGvmmxU6yLIsSM12jIxGSm0Mrd"
                + "zzEkzOMADlPj4TW8Mwo7rSls7nQ120qJTZRwpqu2FsiSxk4Krt/L0WbIjzXjnxqQ"
                + "O1sDVzmo0g35a0+MhfFJvsJFJ4GcLu+s22GPXYvVMXn6WFcxgW/CN2LggO1VDYCs"
                + "bmJIfMS8JWis0fdzPkCdPXWZvIY7OQ=="; */

            string publiccertstr = "MIIDUTCCAjmgAwIBAgIITuLp4XTMVvIwDQYJKoZIhvcNAQELBQAwIDEeMBwGA1UE"
                + "AwwVaW5kdXNhcGkuaW5kdXNpbmQuY29tMB4XDTIyMDkyNzA5MjkxN1oXDTI0MDky"
                + "NjA5MjkxN1owIDEeMBwGA1UEAwwVaW5kdXNhcGkuaW5kdXNpbmQuY29tMIIBIjAN"
                + "BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwgxoD73iv5+QniQBuAovy9wOW2P3"
                + "SGVVB3XyoEXrJo5wnQGbQRJC2s+PLTUUYCG8DrZdBQCc1A4O6EEMTz1N0n6hguky"
                + "wuUAdCMRByYxVtElzFHG94dPra181DSqMuXFRg2Ew4vnOFG/xO7C55NnmSSrR3b7"
                + "emja5ezDyWc+jHvWRxUgX4S+Ra+SYYqTFukS7z37izLhXojL2q6IlNDKIP3Vlkqu"
                + "8sUjEaYbNrEgmIzPS3QUB30FWRjvepNS/xbESN5VRcWm+WTKlqy9XLX0zsgQzSeK"
                + "Qw7POrcTehVD3JtuQ8COFVT3bbG78DD7J+La4KbeJu9NSqL7XoDspPWUawIDAQAB"
                + "o4GOMIGLMAwGA1UdEwQFMAMBAf8wHQYDVR0OBBYEFHwDaTJa6rdv992Nxr3//gPP"
                + "+wnhME8GA1UdIwRIMEaAFHwDaTJa6rdv992Nxr3//gPP+wnhoSSkIjAgMR4wHAYD"
                + "VQQDDBVpbmR1c2FwaS5pbmR1c2luZC5jb22CCE7i6eF0zFbyMAsGA1UdDwQEAwIC"
                + "vDANBgkqhkiG9w0BAQsFAAOCAQEAO72zwdiBBu0/5/fezPr5z9LrZtMvKvoYwMcA"
                + "ANb6IvHOYocPmbOmsBsm9FFutRD/dD8+nUvV4+mWvoNEpnp8C8gvjRGhxr1ZKFA6"
                + "W97UI9jd05Lp6tCW9QN4LH0UInD7Kd2xJtMKeEf7S6HdhcvVQur/KOUay4wF3u2P"
                + "3tIwIYB90fFaLiOxyYYrSnyZm7RmJBG4XDfM7ncc5XrqMb/iwIfHbdn7r+ogRWha"
                + "i3wQwaqd0nDLFyNeSeEUyUCABCcj2mqWa9Ohwlg0TEHnBMxxGXy7iOrRbEloDmwe"
                + "Y+eAMCpSOnI09BxieKIwaIEwlCdDeFKenqFmbCfDkoQg4BAakQ==";

            byte[] decodecert = Convert.FromBase64String(publiccertstr);
            X509Certificate2 cert2 = new X509Certificate2(decodecert);
            RSACryptoServiceProvider rSACrypto = (RSACryptoServiceProvider)cert2.PublicKey.Key;

            var encoded = JWT.Encode(key, rSACrypto, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256GCM);

            return encoded;

        }

        public static byte[] GenererateKey()
        {
            byte[] key;
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            key = aes.Key;
            return key;
        }
        public static string Encrypt(string payloadText, byte[] key)
        {
            var encoded = JWT.Encode(payloadText, key, JweAlgorithm.A256KW, JweEncryption.A256GCM);

            return encoded;
        }
        public static string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string Decrypt(string encryptedResponse, byte[] key)
        {
            //var encryptedObj = Newtonsoft.Json.JsonConvert.DeserializeObject<EncryptedDataResponse>(encryptedResponse);

            var jwt = JWT.Decode(encryptedResponse, key, JweAlgorithm.A256KW, JweEncryption.A256GCM);

            return jwt;
        }

        public static Boolean validateHeader(HttpRequestHeaders header)
        {
            Boolean result = false;
            int headerSize = Constants.allowedHeaderValues.Length;
            int resultCheck = 0;
            foreach (string key in Constants.allowedHeaderValues)
            {
                if (header.Contains(key))
                {
                    if (header.TryGetValues(key, out var headerValues))
                    {
                        var firstValue = headerValues.FirstOrDefault();
                        if (firstValue == ConfigurationManager.AppSettings[key])
                        {
                            resultCheck = resultCheck + 1;
                        }
                    }
                }
            }
            if (resultCheck == headerSize)
            {
                result = true;
            }

            return result;
        }


    }
}