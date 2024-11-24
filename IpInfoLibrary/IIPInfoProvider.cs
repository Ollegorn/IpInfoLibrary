namespace IPInfoLibrary;
public interface IIPInfoProvider
{
    Task<IPDetails> GetDetails(string ip);
}