using System.ComponentModel;

namespace WebAssembly.Domain;

public enum EKeyType
{
    [Description("CPF/CNPJ")]
    Document = 1,
    [Description("Telefone")]
    PhoneNumber = 2,
    [Description("Email")]
    Email = 3,
    [Description("Aleatória")]
    Random = 4,
}