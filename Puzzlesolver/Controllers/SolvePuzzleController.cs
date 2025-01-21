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

            List<(int row, int col, char direction, int length)> wordPlacements = new List<(int row, int col, char direction, int length)>();

            foreach (var coordinate in coordinates)
            {
                var y = coordinate.y;
                var x = coordinate.x;

                if (coordinates.FindAll((c) => {
                    if(c.y == y + 1 && c.x == x)
                    {
                        return true;
                    }
                    
                    return false; 
                }).Count > 0 && coordinates.FindAll((c) => {
                    if (c.y == y - 1 && c.x == x)
                    {
                        return true;
                    }

                    return false;
                }).Count == 0)
                {
                    int length = 1;
                    int i = y + 1;

                    while (true)
                    {
                        if(coordinates.FindAll((c) => {
                            if (c.y == i && c.x == x)
                            {
                                return true;
                            }

                            return false;
                        }).Count > 0)
                        {
                            length++;
                        } else
                        {
                            break;
                        }

                        i++;
                    }

                    wordPlacements.Add((y, x, 'V', length));
                }


                if (coordinates.FindAll((c) => {
                    if (c.x == x + 1 && c.y == y)
                    {
                        return true;
                    }

                    return false;
                }).Count > 0 && coordinates.FindAll((c) => {
                    if (c.x == x - 1 && c.y == y)
                    {
                        return true;
                    }

                    return false;
                }).Count == 0)
                {
                    int length = 1;
                    int i = x + 1;

                    while (true)
                    {
                        if (coordinates.FindAll((c) => {
                            if (c.y == y && c.x == i)
                            {
                                return true;
                            }

                            return false;
                        }).Count > 0)
                        {
                            length++;
                        }
                        else
                        {
                            break;
                        }

                        i++;
                    }

                    wordPlacements.Add((y, x, 'H', length));
                }
            }

            wordPlacements.Sort((x, y) => string.Compare(x.length.ToString(), y.length.ToString()));

            var test = string.Join(',', wordPlacements);

            // Define a crossword grid initialized with empty spaces
            char[,] crosswordGrid = new char[yMax + 1, xMax + 1];
            for (int i = 0; i < crosswordGrid.GetLength(0); i++)
            {
                for (int j = 0; j < crosswordGrid.GetLength(1); j++)
                {
                    crosswordGrid[i, j] = ' '; // Fill grid with spaces
                }
            }

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
