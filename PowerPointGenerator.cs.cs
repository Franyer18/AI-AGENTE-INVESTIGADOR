using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;

namespace Agente_Investigador
{
    public static class PowerPointGenerator
    {
        public static string GenerarPowerPoint(string prompt, string resultado, string carpetaDestino)
        {
            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            string nombreArchivo = Path.Combine(carpetaDestino, $"Presentacion_{DateTime.Now:yyyyMMdd_HHmmss}.pptx");

            using (PresentationDocument presentationDoc = PresentationDocument.Create(nombreArchivo, DocumentFormat.OpenXml.PresentationDocumentType.Presentation))
            {
                // Crear SlideMaster y SlideLayout
                PresentationPart presentationPart = presentationDoc.AddPresentationPart();
                presentationPart.Presentation = new Presentation();

                SlideMasterPart slideMasterPart = presentationPart.AddNewPart<SlideMasterPart>();
                slideMasterPart.SlideMaster = new SlideMaster(
                    new CommonSlideData(new ShapeTree()),
                    new SlideLayoutIdList(),
                    new TextStyles()
                );
                slideMasterPart.SlideMaster.Save();

                SlideLayoutPart slideLayoutPart = slideMasterPart.AddNewPart<SlideLayoutPart>();
                slideLayoutPart.SlideLayout = new SlideLayout(
                    new CommonSlideData(new ShapeTree()),
                    new TextStyles()
                );
                slideLayoutPart.SlideLayout.Save();

                if (slideMasterPart.SlideMaster.SlideLayoutIdList == null)
                    slideMasterPart.SlideMaster.SlideLayoutIdList = new SlideLayoutIdList();

                slideMasterPart.SlideMaster.SlideLayoutIdList.Append(
                    new SlideLayoutId() { Id = 1, RelationshipId = slideMasterPart.GetIdOfPart(slideLayoutPart) }
                );

                presentationPart.Presentation.SlideMasterIdList = new SlideMasterIdList(
                    new SlideMasterId() { Id = 1, RelationshipId = presentationPart.GetIdOfPart(slideMasterPart) }
                );

                SlideIdList slideIdList = presentationPart.Presentation.AppendChild(new SlideIdList());
                uint slideId = 256U;

                // Diapositiva 1: Título con fondo y fuente personalizada
                SlidePart slidePart1 = presentationPart.AddNewPart<SlidePart>();
                slidePart1.AddPart(slideLayoutPart);

                var titleRunProps = new A.RunProperties() { FontSize = 5400, Bold = true };
                titleRunProps.Append(new A.SolidFill(new A.RgbColorModelHex() { Val = "2E74B5" }));

                var titleShape = new Shape(
                    new NonVisualShapeProperties(
                        new NonVisualDrawingProperties() { Id = 2, Name = "Title" },
                        new NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                        new ApplicationNonVisualDrawingProperties(new PlaceholderShape() { Type = PlaceholderValues.Title })
                    ),
                    new ShapeProperties(),
                    new TextBody(
                        new A.BodyProperties(),
                        new A.ListStyle(),
                        new A.Paragraph(
                            new A.Run(
                                titleRunProps,
                                new A.Text(prompt)
                            ),
                            new A.ParagraphProperties()
                        )
                    )
                );

                // Fondo real de la diapositiva 1
                var background1 = new Background(
                    new BackgroundProperties(
                        new A.SolidFill(
                            new A.RgbColorModelHex() { Val = "D9E1F2" } // Azul claro
                        )
                    )
                );

                slidePart1.Slide = new Slide(
                    new CommonSlideData(
                        new ShapeTree(
                            new NonVisualGroupShapeProperties(
                                new NonVisualDrawingProperties() { Id = 1, Name = "" },
                                new NonVisualGroupShapeDrawingProperties(),
                                new ApplicationNonVisualDrawingProperties()
                            ),
                            new GroupShapeProperties(),
                            titleShape
                        )
                    )
                );
                slidePart1.Slide.InsertAt(background1, 0); // El fondo debe ir al inicio
                slideIdList.Append(new SlideId() { Id = slideId++, RelationshipId = presentationPart.GetIdOfPart(slidePart1) });

                // Diapositiva 2: Resultado con viñetas y fuente personalizada
                SlidePart slidePart2 = presentationPart.AddNewPart<SlidePart>();
                slidePart2.AddPart(slideLayoutPart);

                var parrafos = resultado
                    .Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(linea =>
                    {
                        var runProps = new A.RunProperties() { FontSize = 3200 };
                        runProps.Append(new A.SolidFill(new A.RgbColorModelHex() { Val = "44546A" }));
                        var parrafo = new A.Paragraph(
                            new A.Run(
                                runProps,
                                new A.Text(linea.Trim())
                            )
                        );
                        parrafo.ParagraphProperties = new A.ParagraphProperties() { Level = 0 };
                        return parrafo as OpenXmlElement;
                    })
                    .ToArray();

                var textBody = new TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle()
                );
                textBody.Append(parrafos);

                // Fondo real de la diapositiva 2
                var background2 = new Background(
                    new BackgroundProperties(
                        new A.SolidFill(
                            new A.RgbColorModelHex() { Val = "F2F2F2" } // Gris claro
                        )
                    )
                );

                slidePart2.Slide = new Slide(
                    new CommonSlideData(
                        new ShapeTree(
                            new NonVisualGroupShapeProperties(
                                new NonVisualDrawingProperties() { Id = 1, Name = "" },
                                new NonVisualGroupShapeDrawingProperties(),
                                new ApplicationNonVisualDrawingProperties()
                            ),
                            new GroupShapeProperties(),
                            new Shape(
                                new NonVisualShapeProperties(
                                    new NonVisualDrawingProperties() { Id = 2, Name = "Content" },
                                    new NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                                    new ApplicationNonVisualDrawingProperties(new PlaceholderShape() { Type = PlaceholderValues.Body })
                                ),
                                new ShapeProperties(),
                                textBody
                            )
                        )
                    )
                );
                slidePart2.Slide.InsertAt(background2, 0); // El fondo debe ir al inicio
                slideIdList.Append(new SlideId() { Id = slideId++, RelationshipId = presentationPart.GetIdOfPart(slidePart2) });

                presentationPart.Presentation.Save();
            }
            return nombreArchivo;
        }
    }
}









