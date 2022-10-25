using System.Text.Json.Serialization;

namespace WalletService.WebApi.Models;

public struct ErrorApplication
{
    public string ErrorMessage { get; set; }
}