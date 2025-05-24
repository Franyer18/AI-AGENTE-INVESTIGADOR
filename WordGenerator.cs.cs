using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace Agente_Investigador
{
    public static class WordGenerator
    {
        public static string GenerarWord(string prompt, string resultado, string carpetaDestino)
        {
            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            string nombreArchivo = Path.Combine(carpetaDestino, $"Informe_{DateTime.Now:yyyyMMdd_HHmmss}.docx");

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(nombreArchivo, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                body.AppendChild(new Paragraph(new Run(new Text("Tema: " + prompt))));
                body.AppendChild(new Paragraph(new Run(new Text("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")))));
                body.AppendChild(new Paragraph(new Run(new Text("Resultado:"))));
                body.AppendChild(new Paragraph(new Run(new Text(resultado))));

                mainPart.Document.Save();
            }
            return nombreArchivo;
        }
    }
}

