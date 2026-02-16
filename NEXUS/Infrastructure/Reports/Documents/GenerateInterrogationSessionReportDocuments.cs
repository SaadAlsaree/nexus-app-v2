using NEXUS.Features.InterrogationSessionFeature.GetById;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NEXUS.Infrastructure.Reports.Documents;

public class GenerateInterrogationSessionReportDocuments : IDocument
{
    private readonly InterrogationSessionDto _model;

    // Modern color palette
    private static readonly string PrimaryColor = "#1E3A5F";      // Dark blue
    private static readonly string SecondaryColor = "#2E86AB";    // Accent blue
    private static readonly string AccentColor = "#E94F37";       // Red accent
    private static readonly string SuccessColor = "#28A745";      // Green
    private static readonly string BackgroundLight = "#F8F9FA";   // Light gray
    private static readonly string TextDark = "#212529";          // Dark text
    private static readonly string TextMuted = "#6C757D";         // Muted text

    public GenerateInterrogationSessionReportDocuments(InterrogationSessionDto model)
    {
        _model = model;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial").FontColor(Color.FromHex(TextDark)));
            page.ContentFromRightToLeft();

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Background(Color.FromHex(PrimaryColor)).Padding(15).Row(row =>
        {
            // Left side - System name
            row.RelativeItem().AlignMiddle().Text("جهاز الأمن الوطني")
                .FontSize(12).Bold().FontColor(Colors.White);

            // Center - Platform name
            row.RelativeItem().AlignCenter().AlignMiddle().Text("منصة نكسس")
                .FontSize(11).Bold().FontColor(Colors.White);

            // Right side - Report date
            row.RelativeItem().AlignLeft().AlignMiddle().Text($"تاريخ: {DateTime.Now:yyyy-MM-dd}")
                .FontSize(10).FontColor(Colors.White.WithAlpha(200));
        });
    }

    void ComposeSessionProfile(IContainer container)
    {
        container.Background(Color.FromHex(PrimaryColor)).Padding(15).Column(column =>
        {
            column.Item().Text("تقرير جلسة التحقيق")
                .FontSize(12).FontColor(Colors.White.WithAlpha(180));

            column.Item().PaddingTop(3).Text($"جلسة تحقيق - {_model.SessionTypeName}")
                .FontSize(24).Bold().FontColor(Colors.White);

            column.Item().PaddingTop(5).Text($"المشتبه به: {_model.Suspect.FullName}")
                .FontSize(14).FontColor(Colors.White.WithAlpha(220));

            column.Item().PaddingTop(12).Row(statusRow =>
            {
                var outcomeColor = _model.Outcome == Data.Enums.InterrogationOutcome.FullConfession
                    ? Color.FromHex(SuccessColor)
                    : _model.Outcome == Data.Enums.InterrogationOutcome.PartialAdmission
                        ? Color.FromHex(SecondaryColor)
                        : Color.FromHex(AccentColor);

                statusRow.AutoItem().Background(outcomeColor).PaddingVertical(6).PaddingHorizontal(14)
                    .Text(_model.OutcomeName ?? "غير محدد")
                    .FontSize(12).Bold().FontColor(Colors.White);

                statusRow.ConstantItem(12);

                statusRow.AutoItem().Background(Colors.White.WithAlpha(40)).PaddingVertical(6).PaddingHorizontal(14)
                    .Text($"رقم: {_model.Id.ToString()[..8].ToUpper()}")
                    .FontSize(11).FontColor(Colors.White);

                statusRow.ConstantItem(12);

                statusRow.AutoItem().Background(Colors.White.WithAlpha(40)).PaddingVertical(6).PaddingHorizontal(14)
                    .Text($"تاريخ الجلسة: {_model.SessionDate:yyyy-MM-dd}")
                    .FontSize(11).FontColor(Colors.White);
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(10).Column(column =>
        {
            column.Spacing(12);

            // Session profile appears only once at the top
            column.Item().Element(ComposeSessionProfile);
            column.Item().PaddingTop(5).Element(ComposeSessionInfo);
            column.Item().Element(ComposeSuspectInfo);
            column.Item().Element(ComposeSessionContent);
            column.Item().Element(ComposeInvestigatorNotes);
        });
    }

    void SectionHeader(IContainer container, string title, string icon = "●")
    {
        container.BorderBottom(2).BorderColor(Color.FromHex(SecondaryColor)).PaddingBottom(5).Row(row =>
        {
            row.AutoItem().Text(icon).FontSize(14).FontColor(Color.FromHex(SecondaryColor));
            row.ConstantItem(8);
            row.RelativeItem().Text(title).FontSize(14).Bold().FontColor(Color.FromHex(PrimaryColor));
        });
    }

    void ComposeSessionInfo(IContainer container)
    {
        container.Background(Color.FromHex(BackgroundLight)).Padding(12).Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "معلومات الجلسة", "◆"));

            col.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Element(c => InfoCard(c, new[]
                {
                    ("تاريخ الجلسة", _model.SessionDate.ToString("yyyy-MM-dd")),
                    ("نوع الجلسة", _model.SessionTypeName),
                    ("المحقق", _model.InterrogatorName),
                }));

                row.ConstantItem(15);

                row.RelativeItem().Element(c => InfoCard(c, new[]
                {
                    ("الموقع", _model.Location),
                    ("النتيجة", _model.OutcomeName),
                    ("مصادق من القاضي", _model.IsRatifiedByJudge ? "نعم" : "لا"),
                }));
            });
        });
    }

    void InfoCard(IContainer container, (string Label, string Value)[] items)
    {
        container.Background(Colors.White).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(col =>
        {
            foreach (var (label, value) in items)
            {
                col.Item().PaddingBottom(6).Row(row =>
                {
                    row.RelativeItem().Text(label).FontSize(9).FontColor(Color.FromHex(TextMuted));
                    row.RelativeItem().AlignLeft().Text(value).FontSize(10).Bold();
                });
            }
        });
    }

    void ComposeSuspectInfo(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "معلومات المشتبه به", "◆"));

            col.Item().PaddingTop(10).Background(Color.FromHex(BackgroundLight)).Padding(12).Row(row =>
            {
                row.RelativeItem().Element(c => InfoCard(c, new[]
                {
                    ("الاسم الكامل", _model.Suspect.FullName),
                    ("الكنية", _model.Suspect.Kunya ?? "-"),
                    ("اسم الأم", _model.Suspect.MotherName ?? "-"),
                    ("تاريخ الميلاد", _model.Suspect.DateOfBirth?.ToString("yyyy-MM-dd") ?? "-"),
                }));

                row.ConstantItem(15);

                row.RelativeItem().Element(c => InfoCard(c, new[]
                {
                    ("مكان الميلاد", _model.Suspect.PlaceOfBirth ?? "-"),
                    ("القبيلة", _model.Suspect.Tribe ?? "-"),
                    ("الحالة الاجتماعية", _model.Suspect.MaritalStatusName ?? "-"),
                    ("الحالة الحالية", _model.Suspect.CurrentStatusName ?? "-"),
                }));
            });
        });
    }

    void ComposeSessionContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "محتوى الجلسة", "◆"));

            col.Item().PaddingTop(10).Background(Colors.White).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12).Column(contentCol =>
            {
                contentCol.Item().Text("المحتوى الكامل").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                contentCol.Item().PaddingTop(5).Text(_model.Content)
                    .FontSize(10).LineHeight(1.5f);

                if (!string.IsNullOrEmpty(_model.SummaryContent))
                {
                    contentCol.Item().PaddingTop(15).Text("الملخص").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                    contentCol.Item().PaddingTop(5).Background(Color.FromHex(BackgroundLight)).Padding(10)
                        .Text(_model.SummaryContent)
                        .FontSize(10).LineHeight(1.5f);
                }
            });
        });
    }

    void ComposeInvestigatorNotes(IContainer container)
    {
        if (string.IsNullOrEmpty(_model.InvestigatorNotes)) return;

        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "ملاحظات المحقق", "◆"));

            col.Item().PaddingTop(10).Background(Color.FromHex(AccentColor).WithAlpha(20)).Border(1)
                .BorderColor(Color.FromHex(AccentColor)).Padding(12)
                .Text(_model.InvestigatorNotes)
                .FontSize(10).LineHeight(1.5f).FontColor(Color.FromHex(TextDark));
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.Background(Color.FromHex(PrimaryColor)).Padding(10).Row(row =>
        {
            row.RelativeItem().Text(x =>
            {
                x.Span("صفحة ").FontColor(Colors.White);
                x.CurrentPageNumber().FontColor(Colors.White);
                x.Span(" من ").FontColor(Colors.White);
                x.TotalPages().FontColor(Colors.White);
            });

            row.RelativeItem().AlignCenter().Text($"رقم التسجيل: {_model.Id.ToString()[..8].ToUpper()}")
                .FontSize(8).FontColor(Colors.White.WithAlpha(180));

            row.RelativeItem().AlignLeft().Text("منصة نكسس الأمنية - سري للغاية")
                .FontSize(9).Bold().FontColor(Colors.White);
        });
    }
}
