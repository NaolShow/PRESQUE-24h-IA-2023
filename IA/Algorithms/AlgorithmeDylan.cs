using Network;

namespace IA.Algorithms {
    public class AlgorithmeDylan : IAlgorithm {
        public void Start() {

            // 
            while (true) {

                NetworkClient.WaitForTurn();

                bool intentionallyRemoved = false;

                // On boucle sur tous nos nains
                foreach (Dwarf dwarf in NetworkClient.LocalPlayer.Dwarves.ToList()) {
                    Console.WriteLine($"{dwarf.ID} go");

                    // Si on a fini
                    if (dwarf.Cell == null && intentionallyRemoved) continue;

                    // Si le nain n'est pas placé on le place au meilleur endroit sur la map
                    if (dwarf.Cell == null) {
                        Cell cell = NetworkClient.Map.GetGreatestCellFor(dwarf);
                        dwarf.Move(cell.Coords.X, cell.Coords.Y);
                        Console.WriteLine($"Moved dwarf n°{dwarf.ID}");
                    }

                    // Si on a assez d'argent on up en fer
                    if (NetworkClient.LocalPlayer.Score >= 200 && dwarf.Pickaxe == PickaxeType.Wood) {
                        dwarf.Upgrade();
                        NetworkClient.LocalPlayer.Score -= 200;
                    }
                    // Si on a assez d'argent pour acheter un nain
                    if (NetworkClient.LocalPlayer.Score >= 250 && NetworkClient.LocalPlayer.Dwarves.Count < 2) {
                        Console.WriteLine("Dwarf added");
                        NetworkClient.LocalPlayer.HireDwarf();
                        NetworkClient.LocalPlayer.Score -= 250;
                    }

                    // Si le nain peut être en danger
                    if (dwarf.Cell != null && dwarf.Cell.Coords.Z >= 9) {

                        // Si le mal n'a pas été détecté on sonar sur le nain
                        if (!NetworkClient.Map.HasDetectedMal) {
                            NetworkClient.Sonar(dwarf.Cell.Coords.X, dwarf.Cell.Coords.Y);
                        }

                        /// Si la case d'après possède le demon
                        if (NetworkClient.Map.HasDetectedMal) {
                            if (dwarf.Cell != null) {
                                if ((dwarf.Cell.Coords.Z + 1) == NetworkClient.Map.Depth) {
                                    // On deplace si possible a la meilleur place 
                                    bool findSolution = false;
                                    Cell[] cellList = NetworkClient.Map.GetGreatestCells();
                                    foreach (Cell cell in cellList) {

                                        // Si la cellule n'appartient pas à un joueur
                                        if (NetworkClient.Map.GetCellAt(cell.Coords.X, cell.Coords.Y).Player == null && cell.Coords.Z < NetworkClient.Map.Depth - 1) {
                                            dwarf.Move(cell.Coords.X, cell.Coords.Y);
                                            findSolution = true;
                                            break;
                                        }
                                    }

                                    // Sinon on enlève le nain pour être safe
                                    if (!findSolution) {
                                        dwarf.Remove();
                                        intentionallyRemoved = true;
                                    }
                                }
                            }
                        }

                    }

                }

                NetworkClient.DoEndTurn();

            }

            return;
            /**
            Dwarf dwarf = null;
            Cell cellule = null;
            List<Cell> cells = null;
            List<Dwarf> dwarfCells = new List<Dwarf>();
            while (true) {
                /// Attends notre tour
                NetworkClient.WaitForTurn();

                    /// Pour chaque nain
                    foreach (Dwarf d in NetworkClient.LocalPlayer.Dwarves) {
                        Console.WriteLine($"Dwarf depth: {d.Cell.Coords.Z};Depth: {NetworkClient.Map.Depth}");
                        /// Si la case d'après possède le demon
                        if (NetworkClient.Map.HasDetectedMal) {
                            if (dwarf.Cell != null) {
                                if ((dwarf.Cell.Coords.Z + 1) == NetworkClient.Map.Depth) {
                                    // On deplace si possible a la meilleur place 
                                    bool findSolution = false;
                                    Cell[] cellList = NetworkClient.Map.GetGreatestCells();
                                    foreach (Cell cell in cellList) {

                                        // Si la cellule n'appartient pas à un joueur
                                        if (NetworkClient.Map.GetCellAt(cell.Coords.X, cell.Coords.Y).Player == null && cell.Coords.Z < NetworkClient.Map.Depth - 1) {
                                            dwarf.Move(cell.Coords.X, cell.Coords.Y);
                                            findSolution = true;
                                            break;
                                        }
                                    }

                                    if (!findSolution) {
                                        dwarf.Remove();
                                    }
                                }
                            }
                        }

                    }
                }

                /// Fin de notre tour
                NetworkClient.DoEndTurn();**/
        }
    }
}
