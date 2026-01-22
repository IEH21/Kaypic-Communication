namespace Web3_kaypic.service
{
    public interface ISMSSenderService
    {
        Task SendSmsAsync(string number, string message);
    }
}
