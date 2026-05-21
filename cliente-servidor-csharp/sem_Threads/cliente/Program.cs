using System.Net.Sockets;
using System.Text;

TcpClient cliente = new TcpClient();

cliente.Connect("localhost", 5000);

Console.WriteLine("Conectado ao servidor Echo");
Console.WriteLine("Digite 'sair' para encerrar\n");

NetworkStream stream = cliente.GetStream();

try
{
    while (true)
    {
        Console.Write("Digite uma mensagem: ");

        string mensagem = Console.ReadLine()!;

        if (mensagem.ToLower() == "sair")
            break;

        byte[] dados = Encoding.UTF8.GetBytes(mensagem);

        stream.Write(dados, 0, dados.Length);

        byte[] buffer = new byte[1024];

        int bytesLidos = stream.Read(buffer, 0, buffer.Length);

        string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        Console.WriteLine($"Resposta: {resposta}\n");
    }
}
finally
{
    stream.Close();
    cliente.Close();

    Console.WriteLine("Conexão encerrada");
}