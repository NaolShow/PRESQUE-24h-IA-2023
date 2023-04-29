using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Algorithms
{
    public class AlgorithmSuicide : IAlgorithm
    {
        public void Start()
        {
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

                /*
                if (this.GoSuicide(2000))
                {
                    Dwarf bestDwarf = NetworkClient.LocalPlayer.Dwarves[0];
                    foreach (Dwarf dwarf in NetworkClient.LocalPlayer.Dwarves)
                    {
                        if (dwarf.Cell != null)
                        {
                            PickaxeType currentPickaxe = dwarf.Pickaxe;
                            PickaxeType bestPickaxe = bestDwarf.Pickaxe;

                            // compare la puissance des pioches actuelle et la plus puissante
                            while (currentPickaxe != bestPickaxe)
                            {
                                currentPickaxe = currentPickaxe.GetNext();
                                if (currentPickaxe == bestPickaxe)
                                {
                                    bestDwarf = dwarf;
                                    break;
                                }
                            }
                        }             
                    }

                    // On va à la case disponible la plus profonde qui est libre
                    Cell[] cells = NetworkClient.Map.GetDeepestCells();

                    foreach (Cell cell in cells)
                    {
                        if (NetworkClient.Map.GetCellAt(cell.Coords.X, cell.Coords.Y).Player == null)
                        {
                            bestDwarf.Move(cell.Coords.X, cell.Coords.Y);
                        }
                    }



                }*/

                yourTurn = false;
                //On met fin au tour
                NetworkClient.DoEndTurn();
            }
        }


        private bool GoSuicide(int diffValue)
        {
            bool res = false;
            int bestScoreNotMe = 0;

            foreach (KeyValuePair<int, Player> player in NetworkClient.Players)
            {
                if (player.Value != NetworkClient.LocalPlayer)
                {
                    if (bestScoreNotMe < player.Value.Score)
                    {
                        bestScoreNotMe = player.Value.Score;
                    }
                }
            }

            
            if((NetworkClient.LocalPlayer.Score - bestScoreNotMe) > diffValue)
            {
                res = true;
            }
            return res;
        }


        
    }
}
