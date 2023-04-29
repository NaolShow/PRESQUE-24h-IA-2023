namespace Network {

    /// <summary>
    /// Représente la carte
    /// </summary>
    public class Map {

        /// <summary>
        /// Représente la taille de la carte
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Représente la profondeur de la carte
        /// </summary>
        public int Depth { get; } = 20;

        /// <summary>
        /// Représente les cellules de la carte
        /// </summary>
        public Cell[,,] Cells { get; }

        /// <summary>
        /// Récupère une cellule aux coordonnées spécifiées en prenant le plus profond
        /// </summary>
        /// <param name="x">La ligne de la cellule</param>
        /// <param name="y">La colonne de la cellule</param>
        /// <returns>La cellule la plus profonde aux coordonnées spécifiées</returns>
        public Cell GetCellAt(int x, int y) {

            for (int z = 0; z < 20; z++) {

                // Si la cellule n'est pas vide alors on a atteint la plus profonde cellule
                if (Cells[x, y, z].Type != OreType.Air)
                    return Cells[x, y, z];

            }
            throw new Exception("Aucune cellule découverte n'est présente, la carte a été récupérée?");

        }

        /// <summary>
        /// Initialise 
        /// </summary>
        /// <param name="size"></param>
        public Map(int size) {
            Size = size;

            // Initialise la matrice 3D de la carte avec une profondeur de 20
            Cells = new Cell[size, size, 20];

            // On initialise toutes les cellules
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    for (int z = 0; z < 20; z++) {
                        Cells[x, y, z] = new Cell(new Coords(x, y, z));
                    }
                }
            }
        }
    }

}
