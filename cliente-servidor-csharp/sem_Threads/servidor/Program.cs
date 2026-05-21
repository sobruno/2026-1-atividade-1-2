using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener servidor = new TcpListener(IPAddress.Any, 5000);

servidor.Start();

Console.WriteLine("Servidor Echo escutando na porta 5000");

while (true)
{
    Console.WriteLine("\nAguardando conexão...");

    TcpClient cliente = servidor.AcceptTcpClient();

    Console.WriteLine("Cliente conectado!");

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
    finally
    {
        stream.Close();
        cliente.Close();
    }
}