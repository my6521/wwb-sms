namespace WWB.SMS
{
    public interface ISMSServiceFactory
    {
        ISMSService Create();

        ISMSService Create(string name);
    }
}