namespace Network {

    /// <summary>
    /// Représente un nain
    /// </summary>
    public class Dwarf {

        /// <summary>
        /// Le joueur possédant le nain
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// L'identifiant du nain
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Le type de pioche que possède le nain
        /// </summary>
        public PickaxeType Pickaxe { get; private set; } = PickaxeType.Wood;

        /// <summary>
        /// La cellule sur laquelle le nain se trouve (ou null si le nain n'appartient pas au joueur local)
        /// </summary>
        public Cell Cell { get; } = null;

        /// <summary>
        /// Initialise un nain
        /// </summary>
        /// <param name="player">Le joueur a qui appartient le nain</param>
        public Dwarf(Player player, int id) {
            Player = player;
        }

        /// <summary>
        /// Déplace le nain aux coordonnées spécifiées
        /// </summary>
        /// <param name="x">La ligne sur laquelle le nain va être déplacé</param>
        /// <param name="y">La colonne sur laquelle le nain va être déplacé</param>
        /// <returns>La cellule sur laquelle le nain se trouvera</returns>
        public Cell Move(int x, int y) {

            // On demande au serveur d'avoir un nouveau nain
            NetworkClient.SendMessage($"DEPLACER|{ID}|{x}|{y}");
            string message = NetworkClient.GetMessage();

            // Si la réponse n'est pas OK
            if (!message.StartsWith("OK")) {
                Console.WriteLine($"Error while moving dwarf n°{ID}: {message}");
                return null;
            }
            return NetworkClient.Map.GetCellAt(x, y);

        }

        /// <summary>
        /// Enlève le nain du plateau
        /// </summary>
        public void Remove() {

            // On demande au serveur de retirer le nain
            NetworkClient.SendMessage($"RETIRER|{ID}");
            string message = NetworkClient.GetMessage();

            // Si la réponse n'est pas OK
            if (!message.StartsWith("OK")) {
                Console.WriteLine($"Error while removing dwarf n°{ID}: {message}");
                return;
            }

        }

        /// <summary>
        /// Améliore la pioche du nain
        /// </summary>
        public void Upgrade() {

            // On demande au serveur d'améliorer la pioche du nain
            NetworkClient.SendMessage($"AMELIORER|{ID}");
            string message = NetworkClient.GetMessage();

            // Si la réponse n'est pas OK
            if (!message.StartsWith("OK")) {
                Console.WriteLine($"Error while upgrading dwarf n°{ID}: {message}");
                return;
            }

        }

    }

}
