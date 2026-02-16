using NEXUS.Features.SuspectFeature.GetById;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NEXUS.Infrastructure.Reports.Documents;

public class GenerateSuspectReportDocuments : IDocument
{
    private readonly GetByIdDto _model;

    // Modern color palette
    private static readonly string PrimaryColor = "#1E3A5F";      // Dark blue
    private static readonly string SecondaryColor = "#2E86AB";    // Accent blue
    private static readonly string AccentColor = "#E94F37";       // Red accent
    private static readonly string SuccessColor = "#28A745";      // Green
    private static readonly string BackgroundLight = "#F8F9FA";   // Light gray
    private static readonly string TextDark = "#212529";          // Dark text
    private static readonly string TextMuted = "#6C757D";         // Muted text

    public GenerateSuspectReportDocuments(GetByIdDto model)
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

            // Center - Suspect name
            row.RelativeItem().AlignCenter().AlignMiddle().Text("منصة نكسس")
                .FontSize(11).Bold().FontColor(Colors.White);

            // Right side - Report date
            row.RelativeItem().AlignLeft().AlignMiddle().Text($"تاريخ: {DateTime.Now:yyyy-MM-dd}")
                .FontSize(10).FontColor(Colors.White.WithAlpha(200));
        });
    }

    void ComposeSuspectProfile(IContainer container)
    {
        container.Background(Color.FromHex(PrimaryColor)).Padding(15).Row(row =>
        {
            // Photo section
            row.ConstantItem(100).Column(col =>
            {
                bool isWebUrl = _model.PhotoUrl?.StartsWith("http", StringComparison.OrdinalIgnoreCase) ?? false;

                if (!string.IsNullOrEmpty(_model.PhotoUrl) && !isWebUrl)
                {
                    try
                    {
                        col.Item().Height(100).Width(100)
                            .Border(3).BorderColor(Colors.White)
                            .Image(_model.PhotoUrl).FitArea();
                    }
                    catch
                    {
                        col.Item().Height(100).Width(100)
                            .Border(3).BorderColor(Colors.White)
                            .Background(Color.FromHex(SecondaryColor))
                            .AlignCenter().AlignMiddle()
                            .Text("لا صورة").FontColor(Colors.White).FontSize(14);
                    }
                }
                else
                {
                    // Fallback for empty or web URL (until downloaded)
                    col.Item().Height(100).Width(100)
                        .Border(3).BorderColor(Colors.White)
                        .Background(Color.FromHex(SecondaryColor))
                        .AlignCenter().AlignMiddle()
                        .Text(isWebUrl ? "صورة ويب" : "لا صورة").FontColor(Colors.White).FontSize(14);
                }
            });

            row.ConstantItem(20); // Spacer

            // Title and status section
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("تقرير المشتبه به")
                    .FontSize(12).FontColor(Colors.White.WithAlpha(180));

                column.Item().PaddingTop(3).Text(_model.FullName)
                    .FontSize(24).Bold().FontColor(Colors.White);

                column.Item().PaddingTop(5).Text($"الكنية: {_model.Kunya ?? "-"}")
                    .FontSize(13).FontColor(Colors.White.WithAlpha(220));

                column.Item().PaddingTop(12).Row(statusRow =>
                {
                    var statusColor = _model.CurrentStatus == Data.Enums.SuspectStatus.Fugitive
                        ? Color.FromHex(AccentColor)
                        : Color.FromHex(SuccessColor);

                    statusRow.AutoItem().Background(statusColor).PaddingVertical(6).PaddingHorizontal(14)
                        .Text(_model.CurrentStatusName ?? "غير محدد")
                        .FontSize(12).Bold().FontColor(Colors.White);

                    statusRow.ConstantItem(12);

                    statusRow.AutoItem().Background(Colors.White.WithAlpha(40)).PaddingVertical(6).PaddingHorizontal(14)
                        .Text($"رقم: {_model.Id.ToString()[..8].ToUpper()}")
                        .FontSize(11).FontColor(Colors.White);

                    statusRow.ConstantItem(12);

                    statusRow.AutoItem().Background(Colors.White.WithAlpha(40)).PaddingVertical(6).PaddingHorizontal(14)
                        .Text($"تاريخ التقرير: {DateTime.Now:yyyy-MM-dd}")
                        .FontSize(11).FontColor(Colors.White);
                });
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(10).Column(column =>
        {
            column.Spacing(12);

            // Suspect profile appears only once at the top
            column.Item().Element(ComposeSuspectProfile);
            column.Item().PaddingTop(5).Element(ComposePersonalInfo);
            column.Item().Element(ComposeContactInfo);
            column.Item().Element(ComposeRelations);
            column.Item().Element(ComposeLifeHistory);
            column.Item().Element(ComposeOrganizationalData);
            column.Item().Element(ComposeCases);
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

    void ComposePersonalInfo(IContainer container)
    {
        container.Background(Color.FromHex(BackgroundLight)).Padding(12).Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "البيانات الشخصية", "◆"));

            col.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Element(c => InfoCard(c, new[]
                {
                    ("الاسم الكامل", _model.FullName),
                    ("اسم الأم", _model.MotherName ?? "-"),
                    ("تاريخ الميلاد", _model.DateOfBirth?.ToString("yyyy-MM-dd") ?? "-"),
                    ("مكان الميلاد", _model.PlaceOfBirth ?? "-"),
                    ("اللقب", _model.Tribe ?? "-"),
                }));

                row.ConstantItem(15);

                row.RelativeItem().Element(c => InfoCard(c, new[]
                {
                    ("الحالة الاجتماعية", _model.MaritalStatusName ?? "-"),
                    ("عدد الزوجات", _model.WivesCount.ToString()),
                    ("عدد الأبناء", _model.ChildrenCount.ToString()),
                    ("الحالة الصحية", _model.HealthStatus ?? "-"),
                    ("المادة القانونية", _model.LegalArticle ?? "-"),
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

    void ComposeContactInfo(IContainer container)
    {
        var hasAddresses = _model.Addresses.Any();
        var hasContacts = _model.Contacts.Any();
        if (!hasAddresses && !hasContacts) return;

        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "معلومات الاتصال والسكن", "◆"));

            if (hasAddresses)
            {
                col.Item().PaddingTop(10).Text("العناوين").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                col.Item().PaddingTop(5).Element(c => ModernTable(c,
                    new[] { "النوع", "المدينة / الحي", "التفاصيل", "الإحداثيات" },
                    _model.Addresses.Select(a => new[] { a.TypeName, $"{a.City} - {a.District}", a.Details ?? "-", a.GPSCoordinates ?? "-" }).ToList()
                ));
            }

            if (hasContacts)
            {
                col.Item().PaddingTop(12).Text("جهات الاتصال").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                col.Item().PaddingTop(5).Element(c => ModernTable(c,
                    new[] { "النوع", "القيمة", "المالك" },
                    _model.Contacts.Select(c => new[] { c.TypeName, c.Value, c.OwnerName ?? "-" }).ToList()
                ));
            }
        });
    }

    void ModernTable(IContainer container, string[] headers, List<string[]> rows)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                foreach (var _ in headers)
                    columns.RelativeColumn();
            });

            // Header
            foreach (var header in headers)
            {
                table.Cell().Background(Color.FromHex(PrimaryColor)).Padding(8)
                    .Text(header).FontSize(9).Bold().FontColor(Colors.White);
            }

            // Rows
            var isAlt = false;
            foreach (var row in rows)
            {
                var bgColor = isAlt ? Color.FromHex(BackgroundLight) : Colors.White;
                foreach (var cell in row)
                {
                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(7)
                        .Text(cell).FontSize(9);
                }
                isAlt = !isAlt;
            }
        });
    }

    void ComposeRelations(IContainer container)
    {
        if (!_model.RelativesAndAssociates.Any()) return;

        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "الأقارب والمرتبطون", "◆"));
            col.Item().PaddingTop(8).Element(c => ModernTable(c,
                new[] { "الاسم", "العلاقة", "الحالة الحالية", "ملاحظات" },
                _model.RelativesAndAssociates.Select(r => new[] { r.FullName, r.RelationshipName, r.CurrentStatus ?? "-", r.Notes ?? "-" }).ToList()
            ));
        });
    }

    void ComposeLifeHistory(IContainer container)
    {
        if (!_model.LifeHistories.Any()) return;

        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "السجل الحياتي", "◆"));

            foreach (var hist in _model.LifeHistories)
            {
                col.Item().PaddingTop(8).Background(Color.FromHex(BackgroundLight)).Padding(10).Column(c =>
                {
                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Column(rc =>
                        {
                            rc.Item().Text($"المستوى التعليمي: {hist.EducationLevel ?? "-"}").FontSize(10);
                            rc.Item().PaddingTop(3).Text($"المدارس/الجامعات: {hist.SchoolsAttended ?? "-"}").FontSize(10);
                        });
                        r.RelativeItem().Column(rc =>
                        {
                            rc.Item().Text($"الوظائف المدنية: {hist.CivilianJobs ?? "-"}").FontSize(10);
                        });
                    });
                    if (!string.IsNullOrEmpty(hist.RadicalizationStory))
                    {
                        c.Item().PaddingTop(8).Background(Color.FromHex(AccentColor).WithAlpha(20)).Padding(8)
                            .Text($"قصة التطرف: {hist.RadicalizationStory}").FontSize(9).FontColor(Color.FromHex(AccentColor));
                    }
                });
            }
        });
    }

    void ComposeOrganizationalData(IContainer container)
    {
        var hasBayah = _model.BayahDetails.Any();
        var hasTraining = _model.TrainingCourses.Any();
        var hasOps = _model.Operations.Any();
        var hasAssign = _model.OrganizationalAssignments.Any();

        if (!hasBayah && !hasTraining && !hasOps && !hasAssign) return;

        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "البيانات التنظيمية", "◆"));

            if (hasBayah)
            {
                col.Item().PaddingTop(10).Text("تفاصيل البيعة").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                col.Item().PaddingTop(5).Element(c => ModernTable(c,
                    new[] { "التاريخ", "المكان", "المبايع", "نص البيعة" },
                    _model.BayahDetails.Select(b => new[] { b.Date?.ToString("yyyy-MM-dd") ?? "-", b.Location ?? "-", b.RecruiterName ?? "-", b.TextOfPledge ?? "-" }).ToList()
                ));
            }

            if (hasAssign)
            {
                col.Item().PaddingTop(12).Text("التكليفات التنظيمية").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                col.Item().PaddingTop(5).Element(c => ModernTable(c,
                    new[] { "المهمة", "الوحدة", "القائد المباشر", "الفترة" },
                    _model.OrganizationalAssignments.Select(a => new[] { a.RoleTitleName, a.OrgUnitName, a.DirectCommanderName ?? "-", $"{a.StartDate:yyyy-MM-dd} - {a.EndDate?.ToString("yyyy-MM-dd") ?? "حالياً"}" }).ToList()
                ));
            }

            if (hasOps)
            {
                col.Item().PaddingTop(12).Text("العمليات").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                col.Item().PaddingTop(5).Element(c => ModernTable(c,
                    new[] { "النوع", "التاريخ", "المكان", "الدور", "المشاركون" },
                    _model.Operations.Select(o => new[] { o.OperationTypeName, o.Date?.ToString("yyyy-MM-dd") ?? "-", o.Location ?? "-", o.RoleInOp ?? "-", o.Associates ?? "-" }).ToList()
                ));
            }

            if (hasTraining)
            {
                col.Item().PaddingTop(12).Text("الدورات التدريبية").FontSize(11).SemiBold().FontColor(Color.FromHex(SecondaryColor));
                col.Item().PaddingTop(5).Element(c => ModernTable(c,
                    new[] { "نوع الدورة", "المكان", "المدرب", "الزملاء" },
                    _model.TrainingCourses.Select(t => new[] { t.CourseTypeName, t.Location ?? "-", t.TrainerName ?? "-", t.Classmates ?? "-" }).ToList()
                ));
            }
        });
    }

    void ComposeCases(IContainer container)
    {
        if (!_model.CaseInvolvements.Any()) return;

        container.Column(col =>
        {
            col.Item().Element(c => SectionHeader(c, "القضايا المرتبطة", "◆"));
            col.Item().PaddingTop(8).Element(c => ModernTable(c,
                new[] { "رقم الملف", "عنوان القضية", "التهمة", "الحالة القانونية", "تاريخ الاعتراف" },
                _model.CaseInvolvements.Select(ci => new[] { ci.CaseFileNumber, ci.CaseTitle, ci.AccusationTypeName, ci.LegalStatusName, ci.ConfessionDate?.ToString("yyyy-MM-dd") ?? "-" }).ToList()
            ));
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
