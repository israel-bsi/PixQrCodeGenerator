# PixQrCodeGenerator

Um gerador de QR Code dinâmico para pagamentos Pix desenvolvido em **Blazor WebAssembly** utilizando o **.NET 10** e estilizado com **MudBlazor**. O projeto permite a geração rápida de códigos PIX no padrão estático para facilitar cobranças e pagamentos.

## 🚀 Tecnologias Utilizadas

* **Framework:** .NET 10 (Blazor WebAssembly)
* **Interface de Usuário:** MudBlazor v9.5.0
* **Geração de QR Code:** Net.Codecrete.QrCodeGenerator v3.0.0

## 📋 Pré-requisitos

Antes de iniciar, certifique-se de ter instalado em sua máquina:

* **SDK do .NET 10** (versão oficial ou preview compatível)
* IDE de sua preferência (JetBrains Rider, VS Code ou Visual Studio 2022)

## 🔧 Como Rodar o Projeto Localmente

Siga os passos abaixo para clonar e executar a aplicação:

1. **Faça o Fork do repositório para o seu perfil.**

2. **Clone o repositório:**
```bash
git clone https://github.com/seu-usuario/pixqrcodegenerator.git

```


3. **Navegue até a pasta do projeto WebAssembly:**
```bash
cd pixqrcodegenerator/src/WebAssembly

```


4. **Restaure as dependências do projeto:**
```bash
dotnet restore

```


5. **Execute a aplicação:**
```bash
dotnet run

```


6. **Acesse no navegador:**
A aplicação estará disponível através das URLs indicadas no terminal (geralmente `http://localhost:5000` ou `https://localhost:5001`).

## 🛠️ Estrutura de Inicialização

O projeto está configurado como uma Single Page Application (SPA) cliente com injeção de dependência nativa do Blazor para serviços HTTP, MudBlazor e serviços de domínio da aplicação Pix:

```csharp
// Program.cs
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddScoped<IPixService, PixService>();

```

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](https://www.google.com/search?q=LICENSE) para mais detalhes.