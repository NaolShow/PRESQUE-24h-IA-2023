using System.Net.Sockets;

namespace Network {

    /// <summary>
    /// Représente le client bas niveau qui communique avec le serveur
    /// </summary>
    public class NetworkClient {

        private TcpClient client;
        private StreamReader inStream;
        private StreamWriter outStream;

        /// <summary>
        /// Initialise un client qui va communiquer avec le serveur
        /// </summary>
        public NetworkClient() {

            // Initialisation du client TCP
            client = new TcpClient("127.0.0.1", 1234);

            // On récupère le network stream et le transforme en reader et writer
            NetworkStream stream = client.GetStream();
            inStream = new StreamReader(stream);
            outStream = new StreamWriter(stream) {
                AutoFlush = true
            };

        }

        /// <summary>
        /// Lit le prochain message que le serveur va envoyer (bloque le thread entrant)
        /// </summary>
        /// <returns>Le message du serveur</returns>
        public string GetMessage() => inStream.ReadLine();

        /// <summary>
        /// Envoie le message au serveur
        /// </summary>
        /// <param name="message">Le message qui va être envoyé</param>
        public void SendMessage(string message) => outStream.WriteLine(message);

    }

}