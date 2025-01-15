using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic; // Für die List<T>
using Tesseract;
using static Puzzlesolver.Services.GetSqliteConnection;

namespace Puzzlesolver.Services
{
    public class OcrService : GetSqliteConnection
    {

        public List<string> ExtractTextFromImage(string imagePath)
        {
            
                string tessdataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");

                using (var engine = new TesseractEngine(tessdataPath, "deu", EngineMode.Default)) // "deu" für Deutsch
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string extractedText = page.GetText();

                            // Bereinigung des extrahierten Textes
                            List<string> cleanedText = GetWordsAsList(extractedText);

                            return cleanedText;
                        }
                    }
                }
        }

        public string CleanExtractedText(string text)
        {
            var wordList = Regex.Matches(text, @"\b[A-Za-zÄÖÜäöüß]+\b")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .Where(word => 
                                    word.Length > 2 &&                                   
                                    !Regex.IsMatch(word, @"^[A-ZÄÖÜ]{2}$") &&           
                                    word.ToLower() == word || word.ToUpper() == word) 
                                .ToList();

            return string.Join(" ,", wordList);
        }

        public List<string> GetWordsAsList(string text)
        {
            var wordList = Regex.Matches(text, @"\b[A-Za-zÄÖÜäöüß]+\b")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .Where(word => 
                                    word.Length > 2 &&                                   
                                    !Regex.IsMatch(word, @"^[A-ZÄÖÜ]{2}$") &&           
                                    word.ToLower() == word || word.ToUpper() == word) 
                                .ToList();

            return wordList;
        }

        public void SaveTextToFile(List<string> data)
        {
            try
            {
                InsertWords("main.words", data);
            }
            catch (Exception e)
            {
                throw new Exception("Fehler beim Speichern der Datei: " + e.Message);
            }
        }
    }
}
