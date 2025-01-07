using Microsoft.AspNetCore.Mvc;
using Puzzlesolver.Services;

namespace Puzzlesolver.Controllers;

public class ImageUploadController : Controller
{
    private readonly OcrService _ocrService;

    public ImageUploadController()
    {
        _ocrService = new OcrService(); // Initialisierung des OCR-Dienstes
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        if (file != null && file.Length > 0 && (file.ContentType == "image/jpeg" || file.ContentType == "image/png"))
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var filePath = Path.Combine(uploads, file.FileName);

            // Datei speichern
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Text mit Tesseract aus dem Bild extrahieren
            var extractedText = _ocrService.ExtractTextFromImage(filePath);

            // Definiere den Pfad, wo die Textdatei gespeichert wird
            var textFilePath = Path.Combine(uploads, Path.GetFileNameWithoutExtension(file.FileName) + ".txt");

            // Speichern des extrahierten Textes in einer Datei
            _ocrService.SaveTextToFile(extractedText, textFilePath);

            // Rückmeldung an den Benutzer
            ViewBag.ExtractedText = extractedText;
            ViewBag.Message = $"Upload und Texterkennung erfolgreich! Text gespeichert unter: {textFilePath}";

                //Square erkennung
                ImageRecognition imageRecognition = new ImageRecognition();

                imageRecognition.ReadFile(filePath);

                return View("Index");
            }

        ViewBag.Message = "Upload fehlgeschlagen! Nur Bilddateien (JPEG, PNG) sind erlaubt!";
        return View("Index");
    }
}