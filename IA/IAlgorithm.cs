using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    /// <summary>
    /// Interface gérant un algorithme
    /// </summary>
    public interface IAlgorithm
    {
        /// <summary>
        /// Démarre l'algorithme si la partie est commencé
        /// </summary>
        public void Start();
    }
}
