using System.Text;
using WebAssembly.Domain;
using WebAssembly.Domain.Requests;

namespace WebAssembly.Services;

public class PixService : IPixService
{
    public CreatePixPlateResponse GeneratePayload(CreatePixPlateRequest request)
    {
        var payload = new StringBuilder();
        var keyTreated = SanitizePixKey(request.KeyType, request.Key);

        payload.Append(FormatField("00", "01"));

        var gui = FormatField("00", "br.gov.bcb.pix");
        var key = FormatField("01", keyTreated);
        payload.Append(FormatField("26", gui + key));

        payload.Append(FormatField("52", "0000"));
        payload.Append(FormatField("53", "986"));

        if (request.Value is not null && request.Value > 0)
            payload.Append(FormatField("54", request.Value.Value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));

        payload.Append(FormatField("58", "BR"));

        var name = RemoveAccents(request.Name);
        var finalName = name.Length > 25 ? name.Substring(0, 25).Trim() : name;
        payload.Append(FormatField("59", finalName));

        var city = RemoveAccents(request.City);
        var finalCity = city.Length > 15 ? city.Substring(0, 15).Trim() : city;
        payload.Append(FormatField("60", finalCity));

        var rawTxId = request.TxId?.Trim();
        var validTxId = string.IsNullOrWhiteSpace(rawTxId)
            ? "***"
            : new string(rawTxId.Where(char.IsLetterOrDigit).ToArray());

        validTxId = validTxId.Length > 25 ? validTxId.Substring(0, 25) : validTxId;

        if (string.IsNullOrWhiteSpace(validTxId)) 
            validTxId = "***";

        var reference = FormatField("05", validTxId);
        payload.Append(FormatField("62", reference));

        var payloadSemCrc = $"{payload}6304";
        return new CreatePixPlateResponse
        {
            Payload = payloadSemCrc + CalculateCRC16(payloadSemCrc)
        };
    }

    private static string SanitizePixKey(EKeyType keyType, string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return "";

        var cleanKey = key.Trim();

        switch (keyType)
        {
            case EKeyType.PhoneNumber:
                var numbersOnly = new string(cleanKey.Where(char.IsDigit).ToArray());

                if (numbersOnly.StartsWith("55") && numbersOnly.Length >= 12)
                    return "+" + numbersOnly;

                return "+55" + numbersOnly;

            case EKeyType.Document:
                return new string(cleanKey.Where(char.IsDigit).ToArray());

            default:
                return cleanKey;
        }
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