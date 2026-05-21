using System.Net.Sockets;
using System.Text;
using System.Threading;

TcpClient cliente = new TcpClient();
cliente.Connect("localhost", 5000);

Console.WriteLine("Conectado ao servidor Echo");
Console.WriteLine("Digite 'sair' para encerrar\n");

NetworkStream stream = cliente.GetStream();
bool conectado = true;

Thread threadReceber = new Thread(ReceberRespostas);
threadReceber.IsBackground = true;
threadReceber.Start();

try
{
    while (conectado)
    {
        Console.Write("Digite uma mensagem: ");
        string mensagem = Console.ReadLine()!;

        if (mensagem.ToLower() == "sair")
        {
            conectado = false;
            break;
        }

        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(dados, 0, dados.Length);
    }
}
finally
{
    conectado = false;
    stream.Close();
    cliente.Close();

    threadReceber.Join();
    Console.WriteLine("Conexão encerrada");
}

void ReceberRespostas()
{
    try
    {
        while (conectado)
        {
            byte[] buffer = new byte[1024];
            int bytesLidos = stream.Read(buffer, 0, buffer.Length);

            if (bytesLidos == 0)
                break;

            string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
            Console.WriteLine($"\nResposta: {resposta}\n");
        }
    }
    catch (Exception)
    {
        // Se a conexão for fechada pelo cliente, sai silenciosamente.
    }
}
