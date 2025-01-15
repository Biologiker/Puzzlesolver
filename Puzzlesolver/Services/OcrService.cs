using System.Text.RegularExpressions;
using Tesseract;


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

        public List<string> GetWordsAsList(string text)
        {
            var wordList = Regex.Matches(text, @"\b[A-Za-zÄÖÜäöüß]+\b")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .Where(word => 
                                    word.Length > 3 &&                                   
                                    !Regex.IsMatch(word, @"^[A-ZÄÖÜ]{2}$") &&           
                                    word.ToLower() == word || word.ToUpper() == word) 
                                .ToList();

            return wordList;
        }

        public void SaveTextToFile(List<string> data)
        {
            try
            {
                InsertWords("words", data);
            }
            catch (Exception e)
            {
                throw new Exception("Fehler beim Speichern der Datei: " + e.Message);
            }
        }
    }
}
