using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatP2P;

public class Peer{
    private readonly TcpListener _listener;
    private TcpClient? _client; //? es decir que por el momento es nulo
    private const int Port = 8080;
    
    public Peer() => _listener = new TcpListener(IPAddress.Any, Port);

    public async Task ConnectToPeer(string ipAdress, string port){
        try{
            _client = new TcpClient(ipAdress, Convert.ToInt32(port)); //Convert.ToInt32 parsea los strings a ints, el 32 solo es para definir el número máximo de bits
            Console.WriteLine("Connected to Peer :D");

            var recieveTask = RecieveMessage();
            await SendMessage();
            await recieveTask;

        }catch(Exception ex){
            Console.WriteLine("Connection Lost :c" +ex.Message);
        }
    }

    public async Task StartListening(){
        try{
            _listener.Start();
            Console.WriteLine("Listening from incoming connections...");
            _client = await _listener.AcceptTcpClientAsync();
            Console.WriteLine("Connected to Peer :D");

           var recieveTask = RecieveMessage();
            await SendMessage();
            await recieveTask;

        }catch(Exception ex){
            Console.WriteLine("Connection Closed :c " + ex.Message);
        }
    }

private async Task RecieveMessage(){
    try{
        var stream = _client!.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);

        while (true){
            var message = await reader.ReadLineAsync();
            if (message == null) break; // Sal del bucle si la conexión se cierra
            Console.WriteLine($"Peer message: {message}");
        }
    }catch (Exception e){
        Console.WriteLine($"Error receiving message: {e.Message}");
    }finally{
        Close(); // Cierra la conexión después de salir del bucle
    }
}

    private async Task SendMessage(){
    try{
        var stream = _client!.GetStream();
        var writer = new StreamWriter(stream, Encoding.UTF8) {AutoFlush = true };

        while (true){
            Console.Write("");
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) break;

            await writer.WriteLineAsync(message);
        }
    }catch (Exception ex){
        Console.WriteLine($"Error sending message: {ex.Message}");
    }finally{
        Close(); // Cierra la conexión después de salir del bucle
    }
}


    private void Close(){
        _client?.Close();
        _listener.Stop();
    }
}