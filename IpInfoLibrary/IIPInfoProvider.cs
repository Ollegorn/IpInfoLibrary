namespace IpInfoLibrary;
    public interface IIPInfoProvider
    {
        Task<IPDetails> GetDetails(string ip);
    }