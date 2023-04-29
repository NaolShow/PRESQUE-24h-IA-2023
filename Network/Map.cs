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
        /// Représente la profondeur de la carte qui a été découverte (ou la profondeur à laquelle le mal a été trouvé)
        /// </summary>
        public int Depth { get; set; } = 0;

        /// <summary>
        /// Détermine si le mal a été détecté et que la <see cref="Depth"/> représente la profondeur du mal
        /// </summary>
        public bool HasDetectedMal { get; set; } = false;

        /// <summary>
        /// Représente les cellules de la carte
        /// </summary>
        public Cell[,,] Cells { get; }

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

        /// <summary>
        /// Copie la map spécifiée
        /// </summary>
        /// <param name="map">La map copiée</param>
        public Map(Map map) {

            // On copie les valeurs
            Size = map.Size;
            Depth = map.Depth;
            HasDetectedMal = map.HasDetectedMal;

            // On initialise toutes les cellules en les copiant
            Cells = new Cell[Size, Size, Depth];
            for (int x = 0; x < Size; x++) {
                for (int y = 0; y < Size; y++) {
                    for (int z = 0; z < 20; z++) {
                        Cells[x, y, z] = new Cell(map.Cells[x, y, z]);
                    }
                }
            }

        }

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
        /// Récupère un tableau de cellules (ordonnées du plus grand au plus petit) ayant le meilleur score<br/>
        /// Attention: faut vérifier avant de se déplacer s'il y a un joueur sur cette cellule<br/>
        /// Faut également vérifier si le mal est pas juste en dessous
        /// </summary>
        /// <returns>Les cellules ordonnées par la profondeur</returns>
        public Cell[] GetGreatestCells() => Get2DCells().OrderByDescending(a => a.TotalScore).ToArray();

        /// <summary>
        /// La cellule avec le meilleur score ou il n'y a pas de joueur (ou alors la ou le nain est déjà présent)
        /// </summary>
        public Cell GetGreatestCellFor(Dwarf dwarf) => GetGreatestCells().Where(a => a.Player == null || (dwarf != null && a.Dwarf == dwarf)).ToArray()[0];

        /// <summary>
        /// Récupère un tableau de cellules (ordonnées du plus grand au plus petit) étant à la plus grande profondeur<br/>
        /// Attention: faut vérifier avant de se déplacer s'il y a un joueur sur cette cellule<br/>
        /// Faut également vérifier si le mal est pas juste en dessous
        /// </summary>
        /// <returns>Les cellules ordonnées par la profondeur</returns>
        public Cell[] GetDeepestCells() => Get2DCells().OrderByDescending(a => a.Coords.Z).ToArray();
        /// <summary>
        /// La cellule la plus profonde ou il n'y a pas de joueur (ou alors la ou le nain est déjà présent)
        /// </summary>
        public Cell GetDeepestCellFor(Dwarf dwarf) => GetDeepestCells().Where(a => a.Player == null || (dwarf != null && a.Dwarf == dwarf)).ToArray()[0];

        /// <summary>
        /// Récupère une cellule aléatoire ou il n'y a pas de joueur (ou alors la ou le nain est déjà présent)
        /// </summary>
        /// <param name="dwarf">Le nain qui va être déplacé</param>
        /// <returns>La cellule</returns>
        public Cell GetRandomCellFor(Dwarf dwarf) {

            Cell? cell = null;
            while (cell == null || cell.Player != null || cell.Dwarf != dwarf) {

                // Récupère une cellule aléatoire
                cell = GetCellAt(Random.Shared.Next(0, Size), Random.Shared.Next(0, Size));

            }
            return cell;

        }

        /// <summary>
        /// Récupère la liste des cellules visibles
        /// </summary>
        /// <returns>La liste des cellules visibles</returns>
        public List<Cell> Get2DCells() {

            List<Cell> cells = new List<Cell>();
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    cells.Add(GetCellAt(x, y));
            return cells;

        }

    }

}
