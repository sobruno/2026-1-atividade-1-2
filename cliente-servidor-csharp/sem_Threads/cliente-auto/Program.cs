using System.Net.Sockets;
using System.Text;

// Cliente automatizado que envia mensagens sem interação do usuário
string hostName = Environment.GetEnvironmentVariable("SERVER_HOST") ?? "localhost";
int port = int.Parse(Environment.GetEnvironmentVariable("SERVER_PORT") ?? "5000");
string clienteId = Environment.GetEnvironmentVariable("CLIENTE_ID") ?? "1";

try
{
    TcpClient cliente = new TcpClient();
    
    Console.WriteLine($"[Cliente {clienteId}] Tentando conectar em {hostName}:{port}...");
    await cliente.ConnectAsync(hostName, port);
    Console.WriteLine($"[Cliente {clienteId}] Conectado ao servidor");

    NetworkStream stream = cliente.GetStream();

    // Envia 5 mensagens e depois desconecta
    for (int i = 1; i <= 5; i++)
    {
        string mensagem = $"Mensagem {i} do Cliente {clienteId}";
        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        
        await stream.WriteAsync(dados, 0, dados.Length);
        Console.WriteLine($"[Cliente {clienteId}] Enviado: {mensagem}");

        // Lê resposta
        byte[] buffer = new byte[1024];
        int bytesLidos = await stream.ReadAsync(buffer, 0, buffer.Length);
        
        if (bytesLidos > 0)
        {
            string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
            Console.WriteLine($"[Cliente {clienteId}] Resposta: {resposta}");
        }

        // Pequena pausa entre mensagens
        await Task.Delay(500);
    }

    stream.Close();
    cliente.Close();
    Console.WriteLine($"[Cliente {clienteId}] Conexão encerrada");
}
catch (Exception ex)
{
    Console.WriteLine($"[Cliente {clienteId}] Erro: {ex.Message}");
}
