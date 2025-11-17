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
        private readonly List<string>? _diets;
        private readonly string _basePath;

        public PatientPrescriptionDocument(
            string patientName,
            string doctorName,
            string phone,
            DateTime date,
            string diagnosis,
            List<string>? diets,
            string basePath)
        {
            _patientName = patientName;
            _doctorName = doctorName;
            _phone = phone;
            _date = date;
            _diagnosis = diagnosis;
            _diets = diets;
            _basePath = basePath;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                // ✅ Arxa plan şəkilləri (top və bottom)
                page.Background().Layers(layers =>
                {
                    layers.PrimaryLayer().Element(e => e.Container());

                    var topPath = Path.Combine(_basePath, "top.png");
                    var bottomPath = Path.Combine(_basePath, "bottom.png");

                    if (File.Exists(topPath))
                        layers.Layer().Element(e => e.AlignTop().Image(topPath, ImageScaling.FitWidth));

                    if (File.Exists(bottomPath))
                        layers.Layer().Element(e => e.AlignBottom().Image(bottomPath, ImageScaling.FitWidth));
                });

                // 🔸 Əsas məzmun
                page.Content().PaddingHorizontal(40).Column(col =>
                {
                    col.Spacing(18);

                    // Başlıq
                    col.Item().AlignCenter().Text("💊 Resept").Bold().FontSize(16);

                    // Əsas məlumatlar
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

                    // Diaqnoz
                    col.Item()
                        .PaddingTop(10)
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten1)
                        .Padding(10)
                        .Column(inner =>
                        {
                            inner.Item().Text("Diaqnoz:").Bold();
                            inner.Item().Text(_diagnosis ?? "Diaqnoz qeyd olunmayıb.");
                        });

                    // Diet / təyinatlar
                    if (_diets != null && _diets.Any())
                    {
                        col.Item()
                            .PaddingTop(15)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(10)
                            .Column(inner =>
                            {
                                inner.Item().Text("Təyin olunan dərmanlar və ya dietlər:").Bold();
                                foreach (var diet in _diets)
                                    inner.Item().Text($"• {diet}");
                            });
                    }

                    // İmza
                    col.Item().PaddingTop(25).AlignRight().Text("İmza: ____________________");
                });
            });
        }
    }
}
