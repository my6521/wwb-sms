using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;
using WWB.SMS.Models;

namespace WWB.SMS.Tencent
{
    public class TencentSMSService : ISMSService
    {
        private readonly SmsClient _smsClient;

        public TencentSMSService(SMSOptions options)
        {
            Options = options;
            Credential cred = new Credential
            {
                SecretId = options.AccessKeyId,
                SecretKey = options.AccessKeyId
            };
            ClientProfile clientProfile = new ClientProfile();
            clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
            _smsClient = new SmsClient(cred, "ap-guangzhou", clientProfile);
        }

        public SMSOptions Options { get; }

        public async Task<SendResult> Send(string phoneNumbers, string signName, string templateCode, string templateParams)
        {
            var req = new SendSmsRequest();
            req.SmsSdkAppId = "1400787878";
            req.SignName = signName;
            req.SenderId = "";
            req.SessionContext = "";
            req.PhoneNumberSet = new String[] { $"+86{phoneNumbers}" };
            req.TemplateId = templateCode;
            req.TemplateParamSet = new[] { templateParams };
            var resp = await _smsClient.SendSms(req);

            var result = new SendResult();
            result.Code = resp.SendStatusSet[0].Code;
            result.Message = resp.SendStatusSet[0].Message;

            return result;
        }
    }
}