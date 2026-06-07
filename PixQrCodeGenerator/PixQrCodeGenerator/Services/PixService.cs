using System.Text;

namespace PixQrCodeGenerator.Services;

public class PixService
{
    public string GeneratePayload(string chave, string nome, string cidade, decimal? valor, string txtId = "***")
    {
        var payload = new StringBuilder();

        // 00: Payload Format Indicator
        payload.Append(FormatField("00", "01"));

        // 26: Merchant Account Information - Pix
        var gui = FormatField("00", "br.gov.bcb.pix");
        var key = FormatField("01", chave);
        payload.Append(FormatField("26", gui + key));

        // 52: Merchant Category Code
        payload.Append(FormatField("52", "0000"));

        // 53: Transaction Currency (986 = BRL)
        payload.Append(FormatField("53", "986"));

        // 54: Transaction Amount (Opcional)
        if (valor.HasValue && valor > 0)
            payload.Append(FormatField("54", valor.Value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));

        // 58: Country Code
        payload.Append(FormatField("58", "BR"));

        // 59: Merchant Name (Remover acentos para evitar erros)
        payload.Append(FormatField("59", RemoveAccents(nome)));

        // 60: Merchant City
        payload.Append(FormatField("60", RemoveAccents(cidade)));

        // 62: Additional Data Field Template
        var reference = FormatField("05", txtId);
        payload.Append(FormatField("62", reference));

        // 63: CRC16
        string payloadSemCrc = payload.ToString() + "6304";
        return payloadSemCrc + CalculateCRC16(payloadSemCrc);
    }

    private string FormatField(string id, string value)
    {
        return $"{id}{value.Length:D2}{value}";
    }

    private string RemoveAccents(string text)
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

    private string CalculateCRC16(string input)
    {
        ushort crc = 0xFFFF;
        byte[] bytes = Encoding.UTF8.GetBytes(input);

        foreach (byte b in bytes)
        {
            crc ^= (ushort)(b << 8);
            for (int i = 0; i < 8; i++)
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