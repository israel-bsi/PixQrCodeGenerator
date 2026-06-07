namespace WebAssembly.Domain.Requests;

public class CreatePixPlateRequest
{
    public string Key { get; set; } = string.Empty;
    public string KeyType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal? Value { get; set; }
}