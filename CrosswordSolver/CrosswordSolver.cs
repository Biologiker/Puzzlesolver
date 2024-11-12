using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordSolver
{
    public class CrosswordSolver
    {
        
        public char[,] Solve(List<string> words, char[,] grid)
        {
            
            return Solve(words, grid, new HashSet<(int, int)>());
        }

        
        private char[,] Solve(List<string> words, char[,] grid, HashSet<(int, int)> filledPositions)
        {
            if (words.Count == 0)
            {
                if (IsSolved(grid))
                {
                    return grid;
                }
                else
                {
                    throw new Exception("No solution.");
                }
            }

            foreach (var position in GetPossiblePositions(grid, filledPositions))
            {
                foreach (var word in words)
                {
                    var gridCopy = (char[,])grid.Clone();

                    try
                    {
                        
                        FillGrid(gridCopy, word, position);

                        var newWords = new List<string>(words);
                        newWords.Remove(word);

                        var newFilledPositions = new HashSet<(int, int)>(filledPositions);
                        newFilledPositions.Add(position);

                        return Solve(newWords, gridCopy, newFilledPositions);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            throw new Exception("No solution.");
        }

        private List<(int row, int col)> GetPossiblePositions(char[,] grid, HashSet<(int, int)> filledPositions)
        {
            var possiblePositions = new List<(int, int)>();

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (!filledPositions.Contains((row, col)) && grid[row, col] == ' ')
                    {
                        possiblePositions.Add((row, col));
                    }
                }
            }

            return possiblePositions;
        }

        private bool IsSolved(char[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void FillGrid(char[,] grid, string word, (int row, int col) position)
        {
            int row = position.row;
            int col = position.col;

            if (col + word.Length > grid.GetLength(1))
            {
                throw new Exception("Cannot place the word outside grid boundaries.");
            }

            for (int i = 0; i < word.Length; i++)
            {
                if (grid[row, col + i] != ' ' && grid[row, col + i] != word[i])
                {
                    throw new Exception("Cannot place the word at the given position.");
                }
                grid[row, col + i] = word[i];
            }
        }

        public void PrintGrid(char[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    Console.Write(grid[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
