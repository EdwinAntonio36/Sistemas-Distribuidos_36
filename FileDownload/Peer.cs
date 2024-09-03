using System.Net;               // Importa las clases necesarias para trabajar con direcciones IP.
using System.Net.Sockets;       // Importa las clases necesarias para trabajar con sockets TCP/UDP.
using System.Text;              // Importa las clases necesarias para trabajar con la codificación de texto.

namespace FileDownload          // Define un espacio de nombres llamado 'FileDownload'.
{
    public class Peer           // Define la clase 'Peer' que manejará la lógica del peer-to-peer.
    {
        private readonly TcpListener _listener; // Declara un objeto 'TcpListener' que escuchará conexiones entrantes.
        private TcpClient _client;              // Declara un objeto 'TcpClient' para manejar la conexión con otros peers.
        private const int Port = 8080;          // Define un puerto constante (8080) para las conexiones TCP.

        public Peer()                          // Constructor de la clase 'Peer'.
        {
            _listener = new TcpListener(IPAddress.Any, Port); // Inicializa el 'TcpListener' para escuchar en cualquier IP en el puerto 8080.
        }

        // Método asíncrono para descargar un archivo desde otro peer.
        public async Task DownloadFile(string peerIP, int peerPort, string fileName, string savePath, CancellationToken cancellationToken)
        {
            _client = new TcpClient(peerIP, peerPort); // Conecta el 'TcpClient' a otro peer usando la IP y el puerto proporcionados.
            await using var stream = _client.GetStream(); // Obtiene el flujo de datos de la conexión para leer/escribir datos.

            var request = Encoding.UTF8.GetBytes(fileName); // Codifica el nombre del archivo a solicitar en un arreglo de bytes (UTF8).
            await stream.WriteAsync(request, cancellationToken); // Envía el nombre del archivo al peer remoto.

            await using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write); // Crea un archivo local donde se guardará el archivo descargado.
            var buffer = new byte[1024]; // Declara un buffer de 1024 bytes para leer el archivo en fragmentos pequeños.
            int bytesRead; // Variable para almacenar la cantidad de bytes leídos en cada iteración.

            // Bucle para leer el archivo desde el peer remoto en bloques y guardarlo localmente.
            while((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                await fs.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken); // Escribe los bytes leídos en el archivo local.
            }

            Console.WriteLine($"El archivo {fileName} se ha descargado en la ruta {savePath} ;u;"); // Mensaje de confirmación tras la descarga.
        }

        // Método asíncrono que inicia el 'TcpListener' para aceptar conexiones entrantes.
        public async Task Start(CancellationToken cancellationToken)
        {
            _listener.Start(); // Inicia el 'TcpListener' para comenzar a escuchar conexiones.
            while (true) // Bucle infinito para aceptar conexiones entrantes.
            {
                _client = await _listener.AcceptTcpClientAsync(cancellationToken); // Espera y acepta una conexión TCP entrante.
                await HandleClient(cancellationToken); // Maneja la conexión con el cliente que se acaba de conectar.
            }
        }

        // Método asíncrono para manejar la comunicación con un cliente conectado.
        private async Task HandleClient(CancellationToken cancellationToken)
        {
            await using var stream = _client.GetStream(); // Obtiene el flujo de datos de la conexión.
            var buffer = new byte[1024]; // Declara un buffer de 1024 bytes para leer los datos enviados por el cliente.
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken); // Lee los datos enviados por el cliente.

            var fileName = Encoding.UTF8.GetString(buffer, 0, bytesRead); // Decodifica el nombre del archivo desde los bytes leídos.

            // Verifica si el archivo solicitado existe en el sistema de archivos local.
            if (File.Exists(fileName))
            {
                var fileData = await File.ReadAllBytesAsync(fileName, cancellationToken); // Lee el archivo completo en un arreglo de bytes.
                await stream.WriteAsync(fileData, cancellationToken); // Envía los bytes del archivo al cliente.

                Console.WriteLine($"File {fileName} sent to client ;o;"); // Mensaje de confirmación de envío del archivo.
            }
            else
            {
                var errorMessage = Encoding.UTF8.GetBytes("File not found ;n;"); // Codifica un mensaje de error en caso de que el archivo no exista.
                await stream.WriteAsync(errorMessage, cancellationToken); // Envía el mensaje de error al cliente.

                Console.WriteLine($"File {fileName} not found"); // Mensaje de error en la consola del servidor.
            }
        }
    }
}
