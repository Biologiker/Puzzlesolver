using Tesseract;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace Puzzlesolver.Services
{
    public class OcrService
    {
        // Extrahiert Text aus einem Bild mit Tesseract
        public string ExtractTextFromImage(string imagePath)
        {
            try
            {
                string tessdataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");
                using (var engine = new TesseractEngine(tessdataPath, "deu", EngineMode.Default)) // "deu" für Deutsch
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string extractedText = page.GetText();

                            // Bereinigung des extrahierten Textes, um nur Wörter zu behalten
                            string cleanedText = CleanExtractedText(extractedText);

                            return cleanedText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Fehler bei der Texterkennung: " + ex.Message;
            }
        }

        // Bereinigt den extrahierten Text von allem außer alphabetischen Wörtern
        public string CleanExtractedText(string text)
        {
            // Verwende Regex, um nur alphabetische Wörter zu extrahieren
            var wordList = Regex.Matches(text, @"\b[A-Za-zÄÖÜäöüß]+\b")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .ToList();

            // Füge die gefilterten Wörter zu einem String zusammen
            return string.Join(" ", wordList);
        }

        // Speichert den bereinigten Text in einer Datei
        public void SaveTextToFile(string text, string outputFilePath)
        {
            try
            {
                File.WriteAllText(outputFilePath, text);
            }
            catch (Exception e)
            {
                throw new Exception("Fehler beim Speichern der Datei: " + e.Message);
            }
        }
    }
}
