using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Algorithms
{
    public class AlgorithmeFusion : IAlgorithm
    {
        private int? currentlyMiningLayer = null;
        public void Start()
        {

                while (true)
                {

                    NetworkClient.WaitForTurn();

                    // On récupère le dwarf
                    Dwarf dwarf = NetworkClient.LocalPlayer.Dwarves[0];

                    // Si on minait et que la case est minée alors on enlève le minage
                    if (currentlyMiningLayer != null && dwarf.Cell.Coords.Z > currentlyMiningLayer)
                        currentlyMiningLayer = null;

                    // Si l'on ne mine pas actuellement
                    if (currentlyMiningLayer == null)
                    {

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
                    if (NetworkClient.LocalPlayer.Score > 200 && dwarf.Pickaxe == PickaxeType.Wood)
                    {
                        dwarf.Upgrade();
                    }
                    // Achat un nain 250

                    // Si il nous reste des actions on scan
                    for (int i = 0; i <= NetworkClient.RemainingActions; i++)
                    {

                        // On sonar
                        Sonar();

                    }

                    NetworkClient.DoEndTurn();

                    if (NetworkClient.LocalPlayer.Dwarves.Count == 2)
                    {
                        break;
                    }

                }

                bool yourTurn = false;
                NetworkClient.WaitForTurn();
                yourTurn = true;
                Cell bestCell = NetworkClient.Map.GetGreatestCellFor(null);
                Console.WriteLine($"x={bestCell.Coords.X};y={bestCell.Coords.Y};darf={NetworkClient.LocalPlayer.ID}");
                NetworkClient.LocalPlayer.Dwarves[0].Move(bestCell.Coords.X, bestCell.Coords.Y);

                while (true)
                {
                    if (!yourTurn)
                    {
                        NetworkClient.WaitForTurn();
                        yourTurn = true;
                    }

                    Console.WriteLine($"TEST CELLULE LA PLUS PROFONDE : DEPTH : {NetworkClient.Map.Depth}");
                    // Vérification pour le maximum de la depth
                    if (NetworkClient.Map.Depth >= 9)
                    {

                        // Si on a déjà trouvé le monstre
                        if (NetworkClient.Map.HasDetectedMal)
                        {
                            // Si la profondeur de nos nains + 1 est égale à la couche du monstre
                            foreach (Dwarf dwarf in NetworkClient.LocalPlayer.Dwarves)
                            {
                                if (dwarf.Cell != null)
                                {
                                    if ((dwarf.Cell.Coords.Z + 1) == NetworkClient.Map.Depth)
                                    {
                                        // On deplace si possible a la meilleur place 
                                        bool findSolution = false;
                                        Cell[] cellList = NetworkClient.Map.GetGreatestCells();
                                        foreach (Cell cell in cellList)
                                        {

                                            // Si la cellule n'appartient pas à un joueur
                                            if (NetworkClient.Map.GetCellAt(cell.Coords.X, cell.Coords.Y).Player == null && cell.Coords.Z < NetworkClient.Map.Depth - 1)
                                            {
                                                dwarf.Move(cell.Coords.X, cell.Coords.Y);
                                                findSolution = true;
                                                break;
                                            }
                                        }

                                        if (!findSolution)
                                        {
                                            dwarf.Remove();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Le joueur actuelle le plus profond
                            // la case la plus profonde
                            Cell deptherCell = NetworkClient.Map.GetDeepestCells().ToArray()[0];
                            Console.WriteLine($"TEST CELLULE LA PLUS PROFONDE : x={deptherCell.Coords.X};y={deptherCell.Coords.Y};z={deptherCell.Coords.Z}; DEPTH : {NetworkClient.Map.Depth}");
                            if (deptherCell.Coords.Z == NetworkClient.Map.Depth)
                            {
                                Console.WriteLine("=================== Sonnaring from " + deptherCell.Coords.Z);
                                NetworkClient.Sonar(deptherCell.Coords.X, deptherCell.Coords.Y);
                            }

                        }
                    }

                    yourTurn = false;
                    //On met fin au tour
                    NetworkClient.DoEndTurn();

                    Console.ReadLine();


                }

            }
        


            // Récupère la meilleur cellule en prenant en compte les sonars
            private Cell GetBestCellWithSonnared()
            {

                Cell bestPointCell = null;
                int bestPoint = 0;

                foreach (Cell cell in NetworkClient.Map.Get2DCells())
                {

                    // S'il y a un joueur sur cette cellule ou elle est pas safe
                    if (cell.Player != null || !IsCellSafe(cell)) continue;

                    // Récupère les points de la cellule et des trois en dessous
                    int points = 0;
                    for (int i = cell.Coords.Z; i <= Math.Min(NetworkClient.Map.Depth, cell.Coords.Z + 3); i++)
                    {
                        points += NetworkClient.Map.Cells[cell.Coords.X, cell.Coords.Y, i].TotalScore;
                    }

                    if (bestPointCell == null || bestPoint < points)
                    {
                        bestPointCell = cell;
                        bestPoint = points;
                    }

                }

                return bestPointCell;

            }

            // Sonar le plus profond ou le plus rentable
            private void Sonar()
            {

                // On récupère le point le plus profond
                Cell deepestCell = NetworkClient.Map.GetDeepestCells()[0];

                // Si on a pas le plus profond alors on rescanne au plus bas
                if (!NetworkClient.Map.HasDetectedMal && deepestCell.Coords.Z == DeepestScanned() - 1)
                {
                    NetworkClient.Sonar(deepestCell.Coords.X, deepestCell.Coords.Y);
                }
                // Sinon on scan en random
                else
                {

                    // Je récupère une cellule aléatoire
                    Cell? cell = null;
                    while (cell == null)
                    {

                        cell = NetworkClient.Map.GetCellAt(Random.Shared.Next(0, NetworkClient.Map.Size), Random.Shared.Next(0, NetworkClient.Map.Size));

                    }

                    NetworkClient.Sonar(cell.Coords.X, cell.Coords.Y);

                }

            }

            private int DeepestScanned()
            {

                Cell deepestCell = null;
                int deepest = 0;

                foreach (Cell cell in NetworkClient.Map.Get2DCells())
                {

                    // Récupère les points de la cellule et des trois en dessous
                    for (int i = cell.Coords.Z; i <= Math.Min(NetworkClient.Map.Depth, cell.Coords.Z + 3); i++)
                    {
                        Cell subCell = NetworkClient.Map.Cells[cell.Coords.X, cell.Coords.Y, i];
                        if (deepestCell == null || (subCell.Type == OreType.Sonarized && subCell.Coords.Z > deepest))
                        {
                            deepestCell = subCell;
                            deepest = subCell.Coords.Z;
                        }
                    }

                }

                return deepestCell.Coords.Z;

            }

            private bool IsCellSafe(Cell cell)
            {
                return cell.Coords.Z < 9 || cell.Coords.Z < NetworkClient.Map.Depth;
            }

        
    }
}
