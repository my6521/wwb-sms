namespace WWB.SMS.Models
{
    public class SendResult
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public bool IsSuccess => Code.Equals("OK");
    }
}