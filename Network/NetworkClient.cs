﻿using System.Net.Sockets;

namespace Network {

    /// <summary>
    /// Représente le client bas niveau qui communique avec le serveur
    /// </summary>
    public static class NetworkClient {

        private const string teamName = "A.I.G.R.I.";

        private static TcpClient client;
        private static StreamReader inStream;
        private static StreamWriter outStream;

        /// <summary>
        /// Récupère le joueur local (reçus après <see cref="WaitForConnection"/>
        /// </summary>
        public static Player LocalPlayer => Players[localPlayerID];
        private static int localPlayerID;

        /// <summary>
        /// Détermine l'ensemble des joueurs présents dans la partie
        /// </summary>
        public static Dictionary<int, Player> Players { get; private set; } = new Dictionary<int, Player>();

        /// <summary>
        /// Représente la carte (reçue après le début de notre tour)
        /// </summary>
        public static Map Map { get; private set; }

        /// <summary>
        /// Initialise un client qui va communiquer avec le serveur
        /// </summary>
        static NetworkClient() {

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
        public static string GetMessage() => inStream.ReadLine();

        /// <summary>
        /// Envoie le message au serveur
        /// </summary>
        /// <param name="message">Le message qui va être envoyé</param>
        public static void SendMessage(string message) => outStream.WriteLine(message);

        /// <summary>
        /// Attend la connexion au serveur et lui indique le nom de l'équipe
        /// </summary>
        /// <returns>Le numéro du client reçus du serveur</returns>
        /// <exception cref="InvalidOperationException">Est lancé lorsque le serveur n'attend pas notre connexion</exception>
        public static int WaitForConnection() {

            // Si le message reçu n'est pas "DEBUT_PARTIE" alors on stop
            string message = GetMessage();
            if (message != "DEBUT_PARTIE") throw new InvalidOperationException("Mauvaise action, le serveur n'a pas indiqué le début de la partie");

            // On envoie le nom de l'équipe, puis on récupère notre identifiant dans la partie
            SendMessage(teamName);
            localPlayerID = int.Parse(GetMessage().Split('|')[1]);
            Players.Add(localPlayerID, new Player(localPlayerID));
            return localPlayerID;

        }

        /// <summary>
        /// Attend le tour du client
        /// </summary>
        /// <returns>Le numéro du tour actuel</returns>
        public static int WaitForTurn() {

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

            // On demande le score et la carte
            GetScores();
            GetMap();

            return turn;

        }

        /// <summary>
        /// Demande les scores au serveur et raffraichie les données actuelles
        /// </summary>
        private static void GetScores() {

            // On demande les scores et récupère la réponse
            SendMessage("SCORES");
            string message = GetMessage();

            // On boucle sur tous les joueurs
            int playerID = 0;
            foreach (string score in message.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1)) {

                // Si le joueur n'existe pas
                if (!Players.TryGetValue(playerID, out Player player)) {
                    Players[playerID] = new Player(playerID);
                    player = Players[playerID];
                }

                // On donne le score du joueur
                player.Score = int.Parse(score);
                playerID++;
            }

        }

        /// <summary>
        /// Demande la carte au serveur et raffraichie les données actuelles
        /// </summary>
        private static void GetMap() {

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
                cell.Player = clientID == -1 ? null : Players[clientID];

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
        public static void DoEndTurn() {

            // On indique que l'on souhaite terminer notre tour
            SendMessage("FIN_TOUR");

            // On récupère la réponse du serveur et log la réponse
            string message = GetMessage();

            // Si c'est bien le message de fin de tour on laisse sinon on log
            if (message.StartsWith("TOUR_FINI")) return;
            else Console.WriteLine($"Erreur lors du passage de tour: {message}");

        }

        /// <summary>
        /// Sabote le joueur spécifié
        /// </summary>
        /// <param name="player">Le joueur qui va être saboté</param>
        public static void Sabotage(Player player) {

            // On demande au serveur de retirer le nain
            SendMessage($"SABOTER|{player.ID}");
            string message = GetMessage();

            // Si la réponse n'est pas OK
            if (!message.StartsWith("OK")) {
                Console.WriteLine($"Error while saboting player n°{player.ID}: {message}");
                return;
            }

        }

        /// <summary>
        /// Scan les 4 couches en dessous des coordonnées spécifiées (la profondeur visible comprise)
        /// </summary>
        /// <param name="x">La ligne du scan</param>
        /// <param name="y">La colonne du scan</param>
        /// <returns>Les données du scan</returns>
        public static void Sonar(int x, int y) {

            // On demande au serveur de retirer le nain
            SendMessage($"SONAR|{x}|{y}");
            string message = GetMessage();

            // Si la réponse n'est pas OK
            if (!message.StartsWith("OK")) {
                Console.WriteLine($"Error while sonaring at pos x={x};y={y}: {message}");
                return;
            }

            // TODO: Vérifier ce qu'il se passe si on est tout en bas? manque d'argument?
            string[] sonarDatas = message.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).ToArray();

            // On récupère la profondeur actuelle
            int depth = Map.GetCellAt(x, y).Coords.Z;
            foreach (string sonarData in sonarDatas) {
                int score = int.Parse(sonarData);

                // On récupère la cellule et lui attribue sons core
                Cell cell = Map.Cells[x, y, depth];
                cell.SetSonnarizedScore(score);

                // Si le score sonnarisé est -1 c'est le mal ancien
                if (score == -1)
                    cell.Type = OreType.Demon;
                // Sinon on indique que la cellule est sonnarisée
                else cell.Type = OreType.Sonarized;

                depth++;

            }

        }

    }

}