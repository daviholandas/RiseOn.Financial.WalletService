namespace WalletService.WebApi.Models;

public class ApplicationSettings
{
    public DatabaseSettings DatabaseSettings { get; set; }
};
public record DatabaseSettings
{
    public string ConnectionString { get; set; }
    public string CollectionName { get; set; }
};