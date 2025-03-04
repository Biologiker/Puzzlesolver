using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;

namespace Puzzlesolver.Controllers
{
    public class SolvePuzzleController : Controller
    {
        public SolvePuzzleController() { }

        public void Solve(List<(int x, int y, int pixelX, int pixelY)> coordinates, Mat img)
        {
            int xMax = coordinates.MaxBy(x => x.Item1).Item1;
            int yMax = coordinates.MaxBy(x => x.Item2).Item2;

            // Define a crossword grid initialized with empty spaces
            char[,] crosswordGrid = new char[yMax + 1, xMax + 1];
            for (int i = 0; i < crosswordGrid.GetLength(0); i++)
            {
                for (int j = 0; j < crosswordGrid.GetLength(1); j++)
                {
                    crosswordGrid[i, j] = ' '; // Fill grid with spaces
                }
            }


            (int row, int col, char direction)[] wordPlacements =
            {
                (9, 6, 'V'), // ABER
                (0, 8, 'V'), // BERG
                (9, 2, 'V'), // NASE
                (0, 12, 'V'), // TREU
                (9, 0, 'H'), // EINES
                (3, 10, 'H'), // GRUND
                (7, 10, 'H'), // HUNDE
                (11, 10, 'H'), // LEERE
                (5, 0, 'H'), // LEGAL
                (1, 0, 'H'), // LEISE
                (0, 4, 'V'), // BEFEHL
                (7, 0, 'V'), // BLEIBE
                (7, 4, 'V'), // CASINO
                (7, 14, 'V'), // EXTREM
                (0, 0, 'V'), // GLOBAL
                (0, 14, 'V'), // GNADEN
                (7, 10, 'V'), // HOTELS
                (0, 10, 'V'), // SINGLE
                (0, 6, 'V'), // DOCUMENT
                (0, 2, 'V'), // EINZIGER
                (5, 12, 'V'), // MANIEREN
                (5, 8, 'V'),  // NATIONEN
                (9, 6, 'H'), // ADOPTIERT
                (7, 0, 'H'), // BERICHTET
                (11, 0, 'H'), // BESONDERE
                (3, 0, 'H'), // BEZIEHUNG
                (5, 6, 'H'), // EINNEHMEN
                (1, 6, 'H'), // OPERIEREN
            };


            // Define the words and their placement (row, column, direction)
            // Direction: 'H' = Horizontal, 'V' = Vertical
            string[] words =            {
                "ABER", "BERG", "NASE", "TREU",
                "EINES", "GRUND", "HUNDE", "LEERE", "LEGAL", "LEISE",
                "BEFEHL", "BLEIBE", "CASINO", "EXTREM", "GLOBAL", "GNADEN", "HOTELS", "SINGLE",
                "DOCUMENT", "EINZIGER", "MANIEREN", "NATIONEN",
                "ADOPTIERT", "BERICHTET", "BESONDERE", "BEZIEHUNG", "EINNEHMEN", "OPERIEREN"
            };

            // Place the words in the crossword grid
            for (int i = 0; i < words.Length; i++)
            {
                PlaceWord(crosswordGrid, words[i], wordPlacements[i].row, wordPlacements[i].col, wordPlacements[i].direction);
            }

            // Print the crossword grid
            img = PrintGrid(crosswordGrid, coordinates, img);

            Cv2.ImShow("image ", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            return;
        }

        static void PlaceWord(char[,] grid, string word, int row, int col, char direction)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (direction == 'H') // Horizontal
                {
                    grid[row, col + i] = word[i];
                }
                else if (direction == 'V') // Vertical
                {
                    grid[row + i, col] = word[i];
                }
            }
        }

        static Mat PrintGrid(char[,] grid, List<(int x, int y, int pixelX, int pixelY)> coordinates, Mat img)
        {
            Console.WriteLine("Crossword Grid:");
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var letter = grid[i, j];
                    var coordinate = coordinates.Find((f) =>
                    {
                        if (f.y == i && f.x == j)
                        {
                            return true;
                        }

                        return false;
                    });

                    Cv2.PutText(img, letter.ToString(), new Point(coordinate.pixelX, coordinate.pixelY), HersheyFonts.HersheySimplex, 0.35, Scalar.Red, 1, LineTypes.Link8);

                    Console.Write(grid[i, j] == ' ' ? '.' : grid[i, j]); // Empty cells as dots
                    Console.Write(' ');
                }
                Console.WriteLine();
            }

            return img;
        }
    }
}
