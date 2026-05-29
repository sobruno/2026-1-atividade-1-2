using System.Net.Sockets;
using System.Text;
using System.Threading;

// Cliente automatizado que envia mensagens sem interação do usuário
string hostName = Environment.GetEnvironmentVariable("SERVER_HOST") ?? "localhost";
int port = int.Parse(Environment.GetEnvironmentVariable("SERVER_PORT") ?? "5000");
string clienteId = Environment.GetEnvironmentVariable("CLIENTE_ID") ?? "1";

try
{
    TcpClient cliente = new TcpClient();
    
    Console.WriteLine($"[Cliente {clienteId}] Tentando conectar em {hostName}:{port}...");
    cliente.Connect(hostName, port);
    Console.WriteLine($"[Cliente {clienteId}] Conectado ao servidor");

    NetworkStream stream = cliente.GetStream();
    bool conectado = true;

    // Thread para receber respostas
    Thread threadReceber = new Thread(() => ReceberRespostas(stream, clienteId, ref conectado));
    threadReceber.IsBackground = true;
    threadReceber.Start();

    // Envia mensagens
    for (int i = 1; i <= 5; i++)
    {
        string mensagem = $"Mensagem {i} do Cliente {clienteId}";
        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        
        stream.Write(dados, 0, dados.Length);
        Console.WriteLine($"[Cliente {clienteId}] Enviado: {mensagem}");

        Thread.Sleep(500);
    }

    conectado = false;
    stream.Close();
    cliente.Close();
    threadReceber.Join();
    Console.WriteLine($"[Cliente {clienteId}] Conexão encerrada");
}
catch (Exception ex)
{
    Console.WriteLine($"[Cliente {clienteId}] Erro: {ex.Message}");
}

void ReceberRespostas(NetworkStream stream, string clienteId, ref bool conectado)
{
    try
    {
        while (conectado)
        {
            byte[] buffer = new byte[1024];
            int bytesLidos = stream.Read(buffer, 0, buffer.Length);

            if (bytesLidos > 0)
            {
                string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
                Console.WriteLine($"[Cliente {clienteId}] Resposta: {resposta}");
            }
        }
    }
    catch (Exception)
    {
        // Conexão fechada
    }
}
