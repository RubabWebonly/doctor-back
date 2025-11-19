using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Doctor.Application.Services.Documents
{
    public class PatientDietDocument : IDocument
    {
        private readonly string _patientName;
        private readonly string _doctorName;
        private readonly string _phone;
        private readonly DateTime _date;
        private readonly string _diagnosis;
        private readonly List<string>? _diets;
        private readonly string _basePath;

        public PatientDietDocument(
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

                // ============================================================
                //  🔵 BACKGROUND LAYERS
                // ============================================================
                page.Background().Layers(layers =>
                {
                    // ✅ MÜTLƏQ OLMALIDIR – Primary Layer
                    layers.PrimaryLayer().Element(e => e.Container());

                    var top = Path.Combine(_basePath, "top.png");
                    var bottom = Path.Combine(_basePath, "bottom.png");
                    var watermark = Path.Combine(_basePath, "logo.png");

                    // 🔵 Yuxarı şəkil
                    if (File.Exists(top))
                        layers.Layer().Element(e =>
                            e.AlignTop().Image(top, ImageScaling.FitWidth));

                    // 🔵 Aşağı şəkil
                    if (File.Exists(bottom))
                        layers.Layer().Element(e =>
                            e.AlignBottom().Image(bottom, ImageScaling.FitWidth));

                    // 🔵 Orta şəffaf watermark
                    if (File.Exists(watermark))
                        layers.Layer().Element(e =>
                            e.AlignCenter()
                             .TranslateY(200)
                             .Image(watermark, ImageScaling.FitWidth));
                });


                // ============================================================
                //  📄 CONTENT (MIDDLE PART)
                // ============================================================
                page.Content().PaddingHorizontal(40).Column(col =>
                {
                    col.Spacing(15);

                    var logoPath = Path.Combine(_basePath, "fordiet.png");

                    col.Item().Row(row =>
                    {
                        if (File.Exists(logoPath))
                            row.ConstantItem(120).Image(logoPath, ImageScaling.FitWidth);

                        row.RelativeItem();
                    });

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

                    col.Item().PaddingTop(10).Padding(10).Column(inner =>
                    {
                        inner.Item().Text("Diaqnoz:").Bold();
                        inner.Item().Text(_diagnosis ?? "");
                    });

                    if (_diets != null && _diets.Any())
                    {
                        col.Item().PaddingTop(15).Padding(10).Column(inner =>
                        {
                            inner.Item().Text("Təyin olunan Dietlər:").Bold();
                            foreach (var diet in _diets)
                                inner.Item().Text($"• {diet}");
                        });
                    }

                    col.Item().PaddingTop(25).AlignRight().Text("İmza: ____________________");
                });

                // ============================================================
                //  🔵 FOOTER — HƏMİŞƏ AŞAĞIDA
                // ============================================================
                page.Footer().PaddingHorizontal(40).PaddingBottom(10).Row(footer =>
                {
                    footer.RelativeItem().Column(c =>
                    {
                        c.Item().Text("📞  +994 10 123 4567");
                        c.Item().Text("📞  +994 10 123 4567");
                    });

                    footer.RelativeItem().Column(c =>
                    {
                        c.Item().Text("📍  Baku, Azerbaijan");
                        c.Item().Text("      example address");
                    });
                });
            });
        }
    }
}
