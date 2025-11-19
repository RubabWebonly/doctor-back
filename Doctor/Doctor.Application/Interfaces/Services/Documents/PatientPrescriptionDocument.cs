using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Doctor.Application.Services.Documents
{
    public class PatientPrescriptionDocument : IDocument
    {
        private readonly string _patientName;
        private readonly string _doctorName;
        private readonly string _phone;
        private readonly DateTime _date;
        private readonly string _diagnosis;
        private readonly List<string>? _items;
        private readonly string _basePath;

        public PatientPrescriptionDocument(
            string patientName,
            string doctorName,
            string phone,
            DateTime date,
            string diagnosis,
            List<string>? items,
            string basePath)
        {
            _patientName = patientName;
            _doctorName = doctorName;
            _phone = phone;
            _date = date;
            _diagnosis = diagnosis;
            _items = items;
            _basePath = basePath;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                // ========================================================
                // BACKGROUND LAYERS
                // ========================================================
                page.Background().Layers(layers =>
                {
                    layers.PrimaryLayer().Element(e => e.Container());

                    var topPath = Path.Combine(_basePath, "top.png");
                    var bottomPath = Path.Combine(_basePath, "bottom.png");
                    var watermarkPath = Path.Combine(_basePath, "logo.png");

                    if (File.Exists(topPath))
                        layers.Layer().Element(e =>
                            e.AlignTop().Image(topPath, ImageScaling.FitWidth));

                    if (File.Exists(bottomPath))
                        layers.Layer().Element(e =>
                            e.AlignBottom().Image(bottomPath, ImageScaling.FitWidth));

                    // WATERMARK
                    if (File.Exists(watermarkPath))
                    {
                        layers.Layer().Element(e =>
                            e.AlignCenter()
                             .TranslateY(100)
                             .Image(watermarkPath, ImageScaling.FitWidth));
                    }
                });

                // ========================================================
                // CONTENT (ORGANIZED)
                // ========================================================
                page.Content().PaddingHorizontal(40).Column(col =>
                {
                    col.Spacing(18);

                    // HEADER LOGO
                    var logoPath = Path.Combine(_basePath, "fordiet.png");
                    col.Item().Row(row =>
                    {
                        if (File.Exists(logoPath))
                            row.ConstantItem(120).Image(logoPath, ImageScaling.FitWidth);

                        row.RelativeItem();
                    });

                    // PATIENT INFO
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Pasiyent: {_patientName}");
                        row.RelativeItem().AlignRight().Text($"Tarix: {_date:dd.MM.yyyy}");
                    });

                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"Həkim: {_doctorName}");
                        row.RelativeItem().AlignRight().Text($"Əlaqə: {_phone}");
                    });

                    // DIAGNOSIS (NO BORDER)
                    col.Item()
                        .PaddingTop(10)
                        .Column(inner =>
                        {
                            inner.Item().Text("Diaqnoz:").Bold();
                            inner.Item().Text(_diagnosis ?? "");
                        });

                    // DIETS LIST
                    if (_items != null && _items.Any())
                    {
                        col.Item()
                            .PaddingTop(15)
                            .Column(inner =>
                            {
                                inner.Item().Text("Təyin olunan dərmanlar və dietlər:").Bold();
                                foreach (var item in _items)
                                    inner.Item().Text($"• {item}");
                            });
                    }

                    // SIGN
                    col.Item().PaddingTop(25)
                        .AlignRight()
                        .Text("İmza: ____________________");
                });

                // ========================================================
                // FOOTER — ALWAYS AT THE BOTTOM (NEVER MOVES)
                // ========================================================
                page.Footer().PaddingTop(20).Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("📞 +994 10 123 4567");
                            c.Item().Text("📞 +994 10 123 4567");
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("📍 Baku, Azerbaijan");
                            c.Item().Text("example address");
                        });
                    });
                });
            });
        }

    }
}
