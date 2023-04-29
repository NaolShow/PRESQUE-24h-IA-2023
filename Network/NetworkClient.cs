using System.Net.Sockets;

namespace Network {

    /// <summary>
    /// Représente le client bas niveau qui communique avec le serveur
    /// </summary>
    public class NetworkClient {

        private const string teamName = "A.I.G.R.I.";

        private TcpClient client;
        private StreamReader inStream;
        private StreamWriter outStream;

        /// <summary>
        /// Détermine l'identifiant du client donné par le serveur (reçus après <see cref="WaitForConnection"/>)
        /// </summary>
        public int ClientID { get; private set; }

        /// <summary>
        /// Représente la carte (reçue après le début de notre tour)
        /// </summary>
        public Map Map { get; private set; }

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

        /// <summary>
        /// Attend la connexion au serveur et lui indique le nom de l'équipe
        /// </summary>
        /// <returns>Le numéro du client reçus du serveur</returns>
        /// <exception cref="InvalidOperationException">Est lancé lorsque le serveur n'attend pas notre connexion</exception>
        public int WaitForConnection() {

            // Si le message reçu n'est pas "DEBUT_PARTIE" alors on stop
            string message = GetMessage();
            if (message != "DEBUT_PARTIE") throw new InvalidOperationException("Mauvaise action, le serveur n'a pas indiqué le début de la partie");

            // On envoie le nom de l'équipe, puis on récupère notre identifiant dans la partie
            SendMessage(teamName);
            ClientID = int.Parse(GetMessage().Split('|')[1]);
            return ClientID;

        }

        /// <summary>
        /// Attend le tour du client
        /// </summary>
        /// <returns>Le numéro du tour actuel</returns>
        public int WaitForTurn() {

            int turn = -1;

            // On attend que le message reçus soit le début de notre tour
            while (turn == -1) {

                // Si le message n'est pas un début de tour
                string message = GetMessage();
                if (!message.StartsWith("DEBUT_TOUR")) {

                    // On le log on sait jamais (si le comportement est chelou et pas indiqué dans le doc)
                    // => Genre on peut recevoir des données alors que c'est pas notre tour ? :')
                    Console.WriteLine($"Message inconnu reçus en attente de tour: '{message}'");
                    continue;

                }

                // On récupère l'identifiant du tour
                turn = int.Parse(message.Split('|')[1]);

            }

            // On demande la carte
            GetMap();

            return turn;

        }

        /// <summary>
        /// Demande la carte au serveur et raffraichie les données actuelles
        /// </summary>
        private void GetMap() {

            // On demande les informations de la carte et récupère la réponse
            SendMessage("CARTE");
            string message = GetMessage();

            // On récupère les données des cellules
            string[] cellsData = message.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Si la carte n'est pas encore définie
            if (Map == null)
                Map = new Map((int)MathF.Sqrt(cellsData.Length));

            int x = 0;
            int y = 0;

            foreach (string cellData in cellsData) {

                // On récupère toutes les informations de la cellule
                string[] informations = cellData.Split(';');
                int z = int.Parse(informations[0]);
                int clientID = int.Parse(informations[3]);

                // On récupère la cellule
                Cell cell = Map.Cells[x, y, z];

                // Si la cellule supérieure existe on l'ajoute en air
                if (z - 1 >= 0)
                    Map.Cells[x, y, z - 1].Type = OreType.Air;

                // On récupère et sauvegarde les données des cellules
                cell.Quantity = int.Parse(informations[1]);
                cell.Type = informations[2].AsOreType();

                // On incrémente les coordonnées
                x++;
                if (x >= Map.Size) {
                    x = 0;
                    y++;
                }

            }

        }

        /// <summary>
        /// Indique la fin du tour en cours
        /// </summary>
        public void DoEndTurn() {

            // On indique que l'on souhaite terminer notre tour
            SendMessage("FIN_TOUR");

            // On récupère la réponse du serveur et log la réponse
            string message = GetMessage();

            // Si c'est bien le message de fin de tour on laisse sinon on log
            if (message.StartsWith("TOUR_FINI")) return;
            else Console.WriteLine($"Erreur lors du passage de tour: {message}");

        }

    }

}