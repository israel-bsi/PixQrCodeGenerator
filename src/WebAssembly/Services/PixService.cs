using System.Text;
using WebAssembly.Domain.Requests;

namespace WebAssembly.Services;

public class PixService : IPixService
{
    public CreatePixPlateResponse GeneratePayload(CreatePixPlateRequest request)
    {
        var payload = new StringBuilder();

        payload.Append(FormatField("00", "01"));

        var gui = FormatField("00", "br.gov.bcb.pix");
        var key = FormatField("01", request.Key);
        payload.Append(FormatField("26", gui + key));

        payload.Append(FormatField("52", "0000"));
        payload.Append(FormatField("53", "986"));

        if (request.Value is not null && request.Value > 0)
            payload.Append(FormatField("54", request.Value.Value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));

        payload.Append(FormatField("58", "BR"));
        payload.Append(FormatField("59", RemoveAccents(request.Name)));
        payload.Append(FormatField("60", RemoveAccents(request.City)));

        var reference = FormatField("05", "***");
        payload.Append(FormatField("62", reference));

        var payloadSemCrc = $"{payload}6304";
        return new CreatePixPlateResponse
        {
            Payload = payloadSemCrc + CalculateCRC16(payloadSemCrc)
        };
    }

    private static string FormatField(string id, string value) => $"{id}{value.Length:D2}{value}";

    private static string RemoveAccents(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper();
    }

    private static string CalculateCRC16(string input)
    {
        ushort crc = 0xFFFF;
        var bytes = Encoding.UTF8.GetBytes(input);

        foreach (var b in bytes)
        {
            crc ^= (ushort)(b << 8);
            for (var i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) != 0)
                    crc = (ushort)((crc << 1) ^ 0x1021);
                else
                    crc <<= 1;
            }
        }
        return crc.ToString("X4");
    }
}