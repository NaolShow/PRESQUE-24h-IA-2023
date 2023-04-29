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
        /// La cellule sur laquelle le nain se trouve (ou null si le nain n'appartient pas au joueur local)
        /// </summary>
        public Cell Cell { get; } = null;

        /// <summary>
        /// Initialise un nain
        /// </summary>
        /// <param name="player">Le joueur a qui appartient le nain</param>
        public Dwarf(Player player) {
            Player = player;
        }

    }

}
