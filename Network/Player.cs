namespace Network {

    /// <summary>
    /// Représente un joueur physique jouant au jeu et possédant des nains
    /// </summary>
    public class Player {

        public int ID { get; }

        /// <summary>
        /// Initialise un joueur avec son identifiant
        /// </summary>
        /// <param name="playerId">L'identifiant du joueur</param>
        public Player(int playerId) {
            ID = playerId;
        }

    }

}
