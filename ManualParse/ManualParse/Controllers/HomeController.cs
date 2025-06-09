using System.Diagnostics;
using ManualParse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ManualParse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Submit(Strings model)
        {
            if (ModelState.IsValid)
            {
                return View("Index", model);
            }

            return View(model);
        }

        public IActionResult Json()
        {
            return View();
        }

        public IActionResult UploadJson(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var json = reader.ReadToEnd();
                    var model = JsonConvert.DeserializeObject<Strings>(json);
                    return View("Json", model);
                }
            }
            return View("Json");
        }

        public IActionResult Pdf()
        {
            return View();
        }

        public IActionResult UploadPdf(IFormFile file)
        {
            string extractedText = string.Empty;

            if (file != null && file.Length > 0) 
            {
                using (var pdf = PdfDocument.Open(file.OpenReadStream())) 
                {
                    foreach (Page page in pdf.GetPages())
                    {
                        var lines = page.Text.Split(new[] { '\n' }, StringSplitOptions.None);

                        foreach (var line in lines)
                        {
                            extractedText += line + "\n\n";
                        }
                    }
                }

                ViewBag.ExtractedText = extractedText;
            }

            return View("PDF");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
