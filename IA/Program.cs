using Network;

namespace IA {

    internal class Program {

        static void Main(string[] args) {

            // On attend la connexion
            NetworkClient.WaitForConnection();

            Console.WriteLine("Connecté");

            NetworkClient.WaitForTurn();

            // On embauche un nain
            NetworkClient.LocalPlayer.Dwarves[0].Move(4, 4);

            Console.ReadLine();

        }

    }

}