namespace Network {

    /// <summary>
    /// Représente des coordonnées
    /// </summary>
    public class Coords {

        /// <summary>
        /// Représente la coordonnée en X (horizontale)
        /// </summary>
        public int X { get; }
        /// <summary>
        /// Représente la coordonnée en Y (verticale)
        /// </summary>
        public int Y { get; }
        /// <summary>
        /// Représente la coordonnée en Z (profondeur)
        /// </summary>
        public int Z { get; }

        /// <summary>
        /// Initialise les coordonnées
        /// </summary>
        /// <param name="x">Horizontale</param>
        /// <param name="y">Verticale</param>
        /// <param name="z">Profondeur</param>
        public Coords(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

    }

}
