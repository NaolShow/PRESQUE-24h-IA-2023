namespace Network {

    /// <summary>
    /// Représente une cellule
    /// </summary>
    public class Cell {

        /// <summary>
        /// Représente les coordonnées de la cellule
        /// </summary>
        public Coords Coords { get; }

        /// <summary>
        /// Représente le type de minerai disponible sur la cellule
        /// </summary>
        public OreType Type { get; set; } = OreType.Rock;

        /// <summary>
        /// Représente le score total de la cellule (<see cref="Quantity"/> * <see cref="Type.ToScore()"/> ou score sonnarisé)
        /// </summary>
        public int TotalScore => Type == OreType.Sonarized ? sonnarizedScore : Quantity * Type.ToScore();
        private int sonnarizedScore;
        /// <summary>
        /// Représente la quantité totale de minerai sur la cellule (non connu si <see cref="OreType.Sonarized"/>)
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Représente le joueur qui occupe la cellule
        /// </summary>
        public Player? Player { get; set; } = null;

        /// <summary>
        /// Récupère le nain (ou null) qui est présent sur la cellule
        /// </summary>
        public Dwarf Dwarf => Player?.Dwarves.Where(a => a.Cell == this).FirstOrDefault();

        /// <summary>
        /// Initialise un cellule à des coordonnées
        /// </summary>
        /// <param name="coords">Les coordonnées de la cellule</param>
        public Cell(Coords coords) {
            Coords = coords;
        }

        /// <summary>
        /// Copie la cellule spécifiée
        /// </summary>
        /// <param name="cell"La cellule qui va être copiée></param>
        public Cell(Cell cell) {
            Coords = cell.Coords;
            Type = cell.Type;
            sonnarizedScore = cell.sonnarizedScore;
            Player = cell.Player;
        }

        /// <summary>
        /// Défini le score sonnarisé (sans connaître le type de minerai)
        /// </summary>
        /// <param name="score">Le score sonnarisé</param>
        public void SetSonnarizedScore(int score) {
            sonnarizedScore = score;
        }

    }

}
