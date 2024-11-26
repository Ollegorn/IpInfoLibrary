namespace IPInfoLibrary;
public interface IIPInfoProvider
{
    Task<IPDetails> GetDetails(string ip);
}

public interface IPDetails
{
    public string City { get; set; }
    public string Country { get; set; }
    public string Continent { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}