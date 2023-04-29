using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Algorithms
{
    public class AlgorithmTest : IAlgorithm
    {
        public void Start()
        {
            NetworkClient.WaitForTurn();

            // On déplace notre nain initial
            NetworkClient.LocalPlayer.Dwarves[0].Move(4, 4);

            Console.ReadLine();
        }
    }
}
