using WebAssembly.Domain;
using WebAssembly.Domain.Requests;
using WebAssembly.Services;

namespace PixQrCodeGenerator.UnitTests.Services;

public class PixServiceTests
{
    [Fact]
    public void GeneratePayload_DeveRetornarStringCorreta_ComCRC16Calculado()
    {
        // Arrange
        IPixService service = new PixService();
        var request = new CreatePixPlateRequest
        {
            Key = "12345678909",
            KeyType = EKeyType.Document,
            Name = "João da Silva",
            City = "SÃO PAULO",
            Value = 100.50m
        };

        // Act
        var response = service.GeneratePayload(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Payload);
        Assert.StartsWith("00020126330014br.gov.bcb.pix0111123456789095204000053039865406100.505802BR5913JOAO DA SILVA6009SAO PAULO62070503***6304", response.Payload);
        Assert.EndsWith("6304", response.Payload.Substring(0, response.Payload.Length - 4));

        var crc = response.Payload.Substring(response.Payload.Length - 4);
        Assert.Equal(4, crc.Length);
        Assert.Matches("^[0-9A-F]{4}$", crc);
    }
}