using Network;

namespace IA.Algorithms {

    /// <summary>
    /// Algorithme consistant en:<br/>
    /// * Recherche de minerai valant le plus cher<br/>
    /// * Minage<br/>
    /// * Repeat
    /// </summary>
    public class AlgorithmeLoan : IAlgorithm {

        private int? currentlyMiningLayer = null;

        public void Start() {

            while (true) {

                NetworkClient.WaitForTurn();

                // On récupère le dwarf
                Dwarf dwarf = NetworkClient.LocalPlayer.Dwarves[0];

                // Si on minait et que la case est minée alors on enlève le minage
                if (currentlyMiningLayer != null && dwarf.Cell.Coords.Z > currentlyMiningLayer)
                    currentlyMiningLayer = null;

                // Si l'on ne mine pas actuellement
                if (currentlyMiningLayer == null) {

                    // On récupère le meilleur endroit pour miner
                    Cell bestMiningCell = NetworkClient.Map.GetGreatestCellFor(dwarf);

                    // Si c'est une roche on choppe au sonar
                    if (!IsCellSafe(bestMiningCell) || bestMiningCell.Type == OreType.Rock)
                        bestMiningCell = GetBestCellWithSonnared();

                    // On indique la hauteur de mining
                    currentlyMiningLayer = bestMiningCell.Coords.Z;

                    // Si le nain n'est pas sur la cellule on le déplace puis on mine
                    if (bestMiningCell != dwarf.Cell)
                        dwarf.Move(bestMiningCell.Coords.X, bestMiningCell.Coords.Y);

                }

                // Si j'ai assez pour upgrade de pioche
                if (NetworkClient.LocalPlayer.Score > 200 && dwarf.Pickaxe == PickaxeType.Wood) {
                    dwarf.Upgrade();
                }
                // Achat un nain 250

                // Si il nous reste des actions on scan
                for (int i = 0; i <= NetworkClient.RemainingActions; i++) {

                    // On sonar
                    Sonar();

                }

                NetworkClient.DoEndTurn();

            }
            Console.ReadLine();

        }

        // Récupère la meilleur cellule en prenant en compte les sonars
        private Cell GetBestCellWithSonnared() {

            Cell bestPointCell = null;
            int bestPoint = 0;

            foreach (Cell cell in NetworkClient.Map.Get2DCells()) {

                // S'il y a un joueur sur cette cellule ou elle est pas safe
                if (cell.Player != null || !IsCellSafe(cell)) continue;

                // Récupère les points de la cellule et des trois en dessous
                int points = 0;
                for (int i = cell.Coords.Z; i <= Math.Min(NetworkClient.Map.Depth, cell.Coords.Z + 3); i++) {
                    points += NetworkClient.Map.Cells[cell.Coords.X, cell.Coords.Y, i].TotalScore;
                }

                if (bestPointCell == null || bestPoint < points) {
                    bestPointCell = cell;
                    bestPoint = points;
                }

            }

            return bestPointCell;

        }

        // Sonar le plus profond ou le plus rentable
        private void Sonar() {

            // On récupère le point le plus profond
            Cell deepestCell = NetworkClient.Map.GetDeepestCells()[0];

            // Si on a pas le plus profond alors on rescanne au plus bas
            if (!NetworkClient.Map.HasDetectedMal && deepestCell.Coords.Z == DeepestScanned() - 1) {
                NetworkClient.Sonar(deepestCell.Coords.X, deepestCell.Coords.Y);
            }
            // Sinon on scan en random
            else {

                // Je récupère une cellule aléatoire
                Cell? cell = null;
                while (cell == null) {

                    cell = NetworkClient.Map.GetCellAt(Random.Shared.Next(0, NetworkClient.Map.Size), Random.Shared.Next(0, NetworkClient.Map.Size));

                }

                NetworkClient.Sonar(cell.Coords.X, cell.Coords.Y);

            }

        }

        private int DeepestScanned() {

            Cell deepestCell = null;
            int deepest = 0;

            foreach (Cell cell in NetworkClient.Map.Get2DCells()) {

                // Récupère les points de la cellule et des trois en dessous
                for (int i = cell.Coords.Z; i <= Math.Min(NetworkClient.Map.Depth, cell.Coords.Z + 3); i++) {
                    Cell subCell = NetworkClient.Map.Cells[cell.Coords.X, cell.Coords.Y, i];
                    if (deepestCell == null || (subCell.Type == OreType.Sonarized && subCell.Coords.Z > deepest)) {
                        deepestCell = subCell;
                        deepest = subCell.Coords.Z;
                    }
                }

            }

            return deepestCell.Coords.Z;

        }

        private bool IsCellSafe(Cell cell) {
            return cell.Coords.Z < 9 || cell.Coords.Z < NetworkClient.Map.Depth;
        }

    }

}
