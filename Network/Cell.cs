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
        /// Représente le score total de la cellule (<see cref="Quantity"/> * <see cref="Type.ToScore()"/>)
        /// </summary>
        public int TotalScore => Quantity * Type.ToScore();
        /// <summary>
        /// Représente la quantité totale de minerai sur la cellule
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Représente le nain sur la cellule (ou null si aucun nain n'est dessus)
        /// </summary>
        public Dwarf Dwarf { get; set; } = null;

        /// <summary>
        /// Initialise un cellule à des coordonnées
        /// </summary>
        /// <param name="coords">Les coordonnées de la cellule</param>
        public Cell(Coords coords) {
            Coords = coords;
        }

    }

}
