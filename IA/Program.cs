using Network;

namespace IA {

    internal class Program {

        static void Main(string[] args) {

            // On initialise le client et on attend la connexion
            NetworkClient client = new NetworkClient();
            client.WaitForConnection();

            Console.WriteLine("Connecté");

            client.WaitForTurn();

        }

    }

}