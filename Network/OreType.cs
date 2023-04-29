namespace Network {

    /// <summary>
    /// Le type de minerai
    /// </summary>
    public enum OreType {

        /// <summary>
        /// Représente "vraiment rien" un minerai qui a été miné
        /// </summary>
        Air,
        /// <summary>
        /// Représente "rien"
        /// </summary>
        Rock,
        /// <summary>
        /// Représente "fer"
        /// </summary>
        Iron,
        /// <summary>
        /// Représente "or"
        /// </summary>
        Gold,
        /// <summary>
        /// Représente "diamant"
        /// </summary>
        Diamond,
        /// <summary>
        /// Représente "mithril"
        /// </summary>
        Mithril

    }

    /// <summary>
    /// Extensions pour l'énumération <see cref="OreType"/>
    /// </summary>
    public static class OreTypeExtensions {

        /// <summary>
        /// Transforme le type de minerai en valeur de l'énumération
        /// </summary>
        /// <param name="oreType">Le type de minerai</param>
        /// <returns>Le type de minerai en tant que <see cref="OreType"/></returns>
        public static OreType AsOreType(this string oreType) => oreType switch {
            "RIEN" => OreType.Rock,
            "FER" => OreType.Iron,
            "DIAMANT" => OreType.Diamond,
            "MITHRIL" => OreType.Mithril
        };

        /// <summary>
        /// Récupère le score d'un <see cref="OreType"/>
        /// </summary>
        /// <param name="oreType">L'OreType qui possède un score</param>
        /// <returns>Le score de l'OreType</returns>
        public static int ToScore(this OreType oreType) => oreType switch {
            OreType.Air or OreType.Rock => 0,
            OreType.Iron => 10,
            OreType.Gold => 20,
            OreType.Diamond => 40,
            OreType.Mithril => 80
        };

    }

}
