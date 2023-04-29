namespace Network {

    /// <summary>
    /// Représente un joueur physique jouant au jeu et possédant des nains<br/>
    /// MERCI DE PAS UTILISER LES FONCTIONS OU DONNEES SI C'EST PAS LE JOUEUR LOCAL!
    /// </summary>
    public class Player {

        public int ID { get; }

        public int Score { get; set; }

        /// <summary>
        /// Les nains du joueur (attention, ne pas utiliser les valeurs des nains des autres joueurs autre que le joueur local)
        /// </summary>
        public List<Dwarf> Dwarves { get; } = new List<Dwarf>();

        /// <summary>
        /// Initialise un joueur avec son identifiant
        /// </summary>
        /// <param name="playerId">L'identifiant du joueur</param>
        public Player(int playerId) {
            ID = playerId;

            // On donne le nain par défaut au joueur
            Dwarves.Add(new Dwarf(this, 0));
        }

        /// <summary>
        /// Embauche un nain auprès du serveur
        /// </summary>
        public Dwarf HireDwarf() {

            // On demande au serveur d'avoir un nouveau nain
            NetworkClient.SendMessage("EMBAUCHER");
            string message = NetworkClient.GetMessage();

            // Si la réponse n'est pas OK
            if (!message.StartsWith("OK")) {
                Console.WriteLine($"Error while hiring a new dwarf: {message}");
                return null;
            }

            // J'initialise un nouveau nain et l'ajoute au joueur
            Dwarf dwarf = new Dwarf(this, Dwarves.Count);
            Dwarves.Add(dwarf);
            return dwarf;

        }

    }

}
