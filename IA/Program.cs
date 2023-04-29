﻿using IA.Algorithms;
using Network;

namespace IA {

    internal class Program {
        static IAlgorithm algorithm;
        static void Main(string[] args) {

            NetworkClient.IsDebugMode = true;

            // On attend la connexion
            NetworkClient.WaitForConnection();

            Console.WriteLine("Connecté");

            algorithm = new AlgorithmRandom();
            algorithm.Start();
        }

    }

}