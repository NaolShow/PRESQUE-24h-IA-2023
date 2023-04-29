namespace Network {

    /// <summary>
    /// Le type de pioche
    /// </summary>
    public enum PickaxeType {

        /// <summary>
        /// Pioche en bois
        /// </summary>
        Wood,
        /// <summary>
        /// Pioche en fer
        /// </summary>
        Iron,
        /// <summary>
        /// Pioche en diamant
        /// </summary>
        Diamond

    }

    /// <summary>
    /// Extensions pour l'énumération de type de pioche
    /// </summary>
    public static class PickaxeTypeExtensions {

        /// <summary>
        /// Détermine la prochaine amélioration de la pioche (ou la même si l'on a atteint le maximum)
        /// </summary>
        /// <param name="type">L type de pioche actuelle</param>
        /// <returns>La prochaine amélioration</returns>
        public static PickaxeType GetNext(this PickaxeType type) => type switch {
            PickaxeType.Wood => PickaxeType.Iron,
            PickaxeType.Iron => PickaxeType.Diamond,
            PickaxeType.Diamond => PickaxeType.Diamond,
            _ => throw new Exception("Ce type de pioche n'existe pas")
        };

    }

}
