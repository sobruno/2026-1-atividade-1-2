using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

TcpListener servidor = new TcpListener(IPAddress.Any, 5000);
servidor.Start();

Console.WriteLine("Servidor Echo escutando na porta 5000");

while (true)
{
    Console.WriteLine("\nAguardando conexão...");

    TcpClient cliente = servidor.AcceptTcpClient();
    IPEndPoint? enderecoRemoto = cliente.Client.RemoteEndPoint as IPEndPoint;
    Console.WriteLine($"Cliente conectado: {enderecoRemoto?.Address}:{enderecoRemoto?.Port}");

    Thread threadCliente = new Thread(() => HandleCliente(cliente));
    threadCliente.IsBackground = true;
    threadCliente.Start();
}

static void HandleCliente(TcpClient cliente)
{
    NetworkStream stream = cliente.GetStream();

    try
    {
        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesLidos = stream.Read(buffer, 0, buffer.Length);

            if (bytesLidos == 0)
            {
                Console.WriteLine("Cliente desconectou");
                break;
            }

            string mensagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
            Console.WriteLine($"Recebido: {mensagem}");

            string resposta = $"Echo: {mensagem}";
            byte[] respostaBytes = Encoding.UTF8.GetBytes(resposta);
            stream.Write(respostaBytes, 0, respostaBytes.Length);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro no cliente: {ex.Message}");
    }
    finally
    {
        stream.Close();
        cliente.Close();
    }
}
