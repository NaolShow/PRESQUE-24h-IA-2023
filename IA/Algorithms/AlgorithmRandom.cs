using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Algorithms
{
    /// <summary>
    /// Algorithme aléatoire
    /// </summary>
    public class AlgorithmRandom : IAlgorithm
    {
        private Random random = new Random();

        public void Start()
        {
            int size = 0;
            int pa = 0;
            while (true)
            {
                //On attend notre tour
                NetworkClient.WaitForTurn();

                pa = 2;

                List<Dwarf> dwarves = NetworkClient.LocalPlayer.Dwarves;

                foreach (Dwarf dwarf in dwarves.ToList())
                {
                    if(pa>0)
                    {
                        pa -= ChooseAction(dwarf);
                    }
                }
                
                foreach(Dwarf dwarf in dwarves.ToList())
                {
                    if(pa>0)
                    {
                        ChooseMove(dwarf);
                        pa--;
                    }
                }

                Console.WriteLine(pa);

                //On met fin au tour
                NetworkClient.DoEndTurn();
            }
        }

        private int ChooseAction(Dwarf dwarf)
        {
            int score = NetworkClient.LocalPlayer.Score;
            int res = 0;
            if (score >= 250 && NetworkClient.LocalPlayer.Dwarves.Count>=3 && res < 2)
            {
                NetworkClient.LocalPlayer.HireDwarf();
            }

            if (score >= 200 && dwarf.Pickaxe == PickaxeType.Wood && res < 2)
            {
                dwarf.Upgrade();
                res++;
            }
            
            if (score >= 400 && dwarf.Pickaxe == PickaxeType.Iron && res < 2)
            {
                dwarf.Upgrade();
                res++;
            }

            return res;
        }

        private void ChooseMove(Dwarf dwarf)
        {
            int size = NetworkClient.Map.Size;

            //On initialise notre Random et 2 coordonnées aléatoires
            int c1 = random.Next(size);
            int c2 = random.Next(size);

            //On déplace notre nain aux coordonnées déterminées
            if (NetworkClient.Map.GetCellAt(c1, c2).Player == null)
            {
                dwarf.Move(c1, c2);
            }
        }
    }

}
