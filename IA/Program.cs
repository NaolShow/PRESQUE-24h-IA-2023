using Network;

namespace IA {

    internal class Program {

        static void Main(string[] args) {

            // On attend la connexion
            NetworkClient.WaitForConnection();

            Console.WriteLine("Connecté");

            NetworkClient.WaitForTurn();

        }

    }

}