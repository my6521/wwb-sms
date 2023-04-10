using System.Threading.Tasks;
using WWB.SMS.Models;

namespace WWB.SMS
{
    public interface ISMSService
    {
        SMSOptions Options { get; }

        Task<SendResult> Send(string phoneNumbers, string signName, string templateCode, string templateParams);
    }
}