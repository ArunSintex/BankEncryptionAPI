using BankEncryptionAPI.common;
using BankEncryptionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using log4net;
using System.Reflection;

namespace BankEncryptionAPI.Controllers
{
    public class EncryptionController : ApiController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EncryptionController));
        [HttpPost]
        [Route("api/encryption")]
        public async Task<HttpResponseMessage> encryption([FromBody] object payload)
        {
            _logger.Info($"encryption controller for payload : {payload}");
            Boolean headerValidate = CommonUtility.validateHeader(Request.Headers);

            if (!headerValidate)
            {
                Error error = new Error();
                error.errorCode = "400";
                error.message = "Required Header Keys are missing/invalid";
                error.developerMessage = new Exception("Required Header Keys are missing/invalid");
                _logger.Info($"Header Validation Failed for payload : {payload}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            var payloadText = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            byte[] key = CommonUtility.GenererateKey();
            var encryptedText = CommonUtility.Encrypt(payloadText, key);
            EncryptedPayload encryptedPayload = new EncryptedPayload();
            encryptedPayload.bit = 0;
            encryptedPayload.key = CommonUtility.EncryptKey(CommonUtility.ToHexString(key));
            encryptedPayload.data = encryptedText;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(encryptedPayload);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = ConfigurationManager.AppSettings["BankingApiUrl"];
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("IBL-Client-Id", ConfigurationManager.AppSettings["IBLClientId"]);
            client.DefaultRequestHeaders.Add("IBL-Client-Secret", ConfigurationManager.AppSettings["IBLClientSecret"]);

            var response = await client.PostAsync(url, data);

            var result = await response.Content.ReadAsStringAsync();
            // Parse the JSON string
            JObject jsonResult = JObject.Parse(result);

            // Extract the inner JWT from the 'data' field
            string encryptedData = jsonResult["data"].ToString();

            EncryptedDataResponse encryptedDataResponse = new EncryptedDataResponse();
            encryptedDataResponse.data = encryptedData;
            encryptedDataResponse.key = key;
            _logger.Info($"Response from API : {encryptedDataResponse}");
            return Request.CreateResponse(HttpStatusCode.OK, encryptedDataResponse);
        }
        [HttpPost]
        [Route("api/decryption")]
        public HttpResponseMessage decryption([FromBody] DecryptRequestPayLoad payload)
        {
            Boolean headerValidate = CommonUtility.validateHeader(Request.Headers);
            _logger.Info($"decryption controller called for payload : {payload}");
            if (!headerValidate)
            {
                Error error = new Error();
                error.errorCode = "400";
                error.message = "Required Header Keys are missing/invalid";
                error.developerMessage = new Exception("Required Header Keys are missing/invalid");
                _logger.Info($"Header Validation Failed for payload : {payload}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
            string encryptedMessage = payload.data;
            byte [] encodedKey = payload.key;
            //byte[] key = Encoding.ASCII.GetBytes(encodedKey);
            Object response = CommonUtility.Decrypt(encryptedMessage, encodedKey);
            _logger.Info($"Response from API : {response}");
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpPost]
        [Route("api/bankdetails")]
        public async Task<HttpResponseMessage> getBankDetails([FromBody] object payload)
        {
            _logger.Info($"getBankDetails controller for payload : {payload}");
            Boolean headerValidate = CommonUtility.validateHeader(Request.Headers);

            if (!headerValidate)
            {
                Error error = new Error();
                error.errorCode = "400";
                error.message = "Required Header Keys are missing/invalid";
                error.developerMessage = new Exception("Required Header Keys are missing/invalid");
                _logger.Info($"Header Validation Failed for payload : {payload}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

            var payloadText = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            byte[] key = CommonUtility.GenererateKey();
            var encryptedText = CommonUtility.Encrypt(payloadText, key);
            EncryptedPayload encryptedPayload = new EncryptedPayload();
            encryptedPayload.bit = 0;
            encryptedPayload.key = CommonUtility.EncryptKey(CommonUtility.ToHexString(key));
            encryptedPayload.data = encryptedText;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(encryptedPayload);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = ConfigurationManager.AppSettings["BankingApiUrl"];
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("IBL-Client-Id", ConfigurationManager.AppSettings["IBLClientId"]);
            client.DefaultRequestHeaders.Add("IBL-Client-Secret", ConfigurationManager.AppSettings["IBLClientSecret"]);

            var response = await client.PostAsync(url, data);

            var result = await response.Content.ReadAsStringAsync();
            // Parse the JSON string
            JObject jsonResult = JObject.Parse(result);

            _logger.Info($"Without Formatting Response from API : {jsonResult}");

            // Extract the inner JWT from the 'data' field
            string encryptedData = jsonResult["data"].ToString();

            Object decryptedresponse = CommonUtility.Decrypt(encryptedData, key);
           
            _logger.Info($"Response from API : {decryptedresponse}");
            return Request.CreateResponse(HttpStatusCode.OK, decryptedresponse);
        }


    }
}
