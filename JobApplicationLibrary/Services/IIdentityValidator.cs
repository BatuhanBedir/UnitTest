namespace JobApplicationLibrary.Services
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);

        //bool CheckConnectionToRemoteServer();
        ICountryDataProvier CountryDataProvider { get; }
    }
    public interface ICountryData
    {
        string Country { get; }

    }
    public interface ICountryDataProvier
    {
        ICountryData CountryData { get; }
    }
}