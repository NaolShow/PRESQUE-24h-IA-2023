namespace Network {

    /// <summary>
    /// Représente un joueur physique jouant au jeu et possédant des nains
    /// </summary>
    public class Player {

        public int ID { get; }

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
        }

    }

}
