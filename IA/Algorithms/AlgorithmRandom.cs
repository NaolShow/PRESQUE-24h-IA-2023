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
        public void Start()
        {
            Random random = new Random();
            while (true)
            {
                //On attend notre tour
                NetworkClient.WaitForTurn();

                //On initialise notre Random et 2 coordonnées aléatoires
                int c1 = random.Next(5);
                int c2 = random.Next(5);

                //On déplace notre nain aux coordonnées déterminées
                if (NetworkClient.Map.GetCellAt(c1, c2).Player == null)
                {
                    NetworkClient.LocalPlayer.Dwarves[0].Move(c1, c2);
                }

                //On met fin au tour
                NetworkClient.DoEndTurn();
            }
        }
    }
}
