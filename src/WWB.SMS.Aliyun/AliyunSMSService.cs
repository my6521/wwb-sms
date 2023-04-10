using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using System.Threading.Tasks;
using WWB.SMS.Models;

namespace WWB.SMS.Aliyun
{
    public class AliyunSMSService : ISMSService
    {
        private readonly Client _client;
        public SMSOptions Options { get; }

        public AliyunSMSService(SMSOptions options)
        {
            Options = options;
            var config = new Config
            {
                AccessKeyId = options.AccessKeyId,
                AccessKeySecret = options.AccessKeySecret,
                Endpoint = "dysmsapi.aliyuncs.com"
            };

            _client = new Client(config);
        }

        public async Task<SendResult> Send(string phoneNumbers, string signName, string templateCode, string templateParams)
        {
            var sendSmsRequest = new SendSmsRequest();
            sendSmsRequest.PhoneNumbers = phoneNumbers;
            sendSmsRequest.SignName = signName;
            sendSmsRequest.TemplateCode = templateCode;
            sendSmsRequest.TemplateParam = templateParams;

            var result = new SendResult();

            var resp = await _client.SendSmsAsync(sendSmsRequest);

            if (resp.Body.Code == "OK")
            {
            }

            result.Code = resp.Body.Code;
            result.Message = resp.Body.Message;

            return result;
        }
    }
}