using Network;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace IA.Algorithms
{
    public class AlgorithmeDylan : IAlgorithm
    {
        public void Start()
        {
            Dwarf dwarf = null;
            Cell cellule = null;
            List<Cell> cells = null;
            List<Dwarf> dwarfCells = new List<Dwarf>();
            while (true) 
            {
                /// Attends notre tour
                NetworkClient.WaitForTurn();

                /// Initialisation
                if (dwarf == null)
                {
                    dwarf = NetworkClient.LocalPlayer.Dwarves[0];
                    dwarfCells.Add(dwarf);
                    cellule = NetworkClient.Map.GetGreatestCellFor(dwarf);
                    NetworkClient.LocalPlayer.Dwarves[0].Move(cellule.Coords.X, cellule.Coords.Y);
                }
                
                /// Si Coordonnées plus grand ou égale à 9
                if(dwarf.Cell.Coords.Z >= 9)
                {
                    /// On verifie que le mal à été détecté pour ne pas scan dans le vide
                    if(!NetworkClient.Map.HasDetectedMal)
                    {
                        NetworkClient.Sonar(dwarf.Cell.Coords.X, dwarf.Cell.Coords.Y);
                    }
                    if(NetworkClient.LocalPlayer.Score >= 200 && dwarf.Pickaxe == PickaxeType.Wood)
                    {
                        dwarf.Upgrade();
                    }
                    /// Pour chaque nain
                    foreach(Dwarf d in dwarfCells) 
                    {
                        Console.WriteLine($"Dwarf depth: {d.Cell.Coords.Z};Depth: {NetworkClient.Map.Depth}");
                        /// Si la case d'après possède le demon
                        if (NetworkClient.Map.HasDetectedMal)
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
                }

                /// Fin de notre tour
                NetworkClient.DoEndTurn();
            }
        }
    }
}
