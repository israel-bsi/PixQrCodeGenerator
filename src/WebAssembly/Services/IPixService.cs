using WebAssembly.Domain.Requests;

namespace WebAssembly.Services;

public interface IPixService
{
    public CreatePixPlateResponse GeneratePayload(CreatePixPlateRequest request);
}