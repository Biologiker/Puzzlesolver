using System;
using System.Collections.Generic;
using System.Linq;
using CrosswordSolver;

namespace CrosswordSolver
{
    public class Program
    {
        public static void Main()
        {
            char[,] grid = {
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' }
            };

            var words = new List<string> { "apple", "pear", "dog" };

            var solver = new CrosswordSolver();

            try
            {
                var solvedGrid = solver.Solve(words, grid);
                solver.PrintGrid(solvedGrid); // Output the solved grid
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
