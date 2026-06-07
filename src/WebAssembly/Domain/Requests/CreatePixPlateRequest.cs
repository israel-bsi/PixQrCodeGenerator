namespace WebAssembly.Domain.Requests;

public class CreatePixPlateRequest
{
    public string Key { get; set; } = string.Empty;
    public EKeyType KeyType { get; set; } = EKeyType.Document;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal? Value { get; set; }
    public string TxId { get; set; } = string.Empty;
}