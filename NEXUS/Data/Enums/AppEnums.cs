using System.ComponentModel.DataAnnotations;

namespace NEXUS.Data.Enums;

public enum SuspectStatus
{
    [Display(Name = "موقوف")]
    Detained = 1,       // موقوف
    [Display(Name = "هارب")]
    Fugitive = 2,       // هارب
    [Display(Name = "مقتول")]
    Killed = 3,         // مقتول
    [Display(Name = "محكوم")]
    Sentenced = 4,      // محكوم
    [Display(Name = "مُفرج عنه")]
    Released = 5,       // مُفرج عنه
    [Display(Name = "غير معروف")]
    Unknown = 0
}

// الحالة الاجتماعية
public enum MaritalStatus
{
    [Display(Name = "أعزب")]
    Single = 1,
    [Display(Name = "متزوج")]
    Married = 2,
    [Display(Name = "مطلق")]
    Divorced = 3,
    [Display(Name = "أرمل")]
    Widower = 4
}

// نوع العنوان/المكان
public enum AddressType
{
    [Display(Name = "سكن أصلي")]
    PermanentResidence = 1, // سكن أصلي
    [Display(Name = "سكن مؤقت")]
    TemporaryResidence = 2, // سكن مؤقت
    [Display(Name = "مضافة")]
    SafeHouse = 3,          // مضافة
    [Display(Name = "مقر قيادة")]
    OrgHeadquarters = 4,    // مقر قيادة
    [Display(Name = "نقطة التقاء")]
    MeetingPoint = 5        // نقطة التقاء
}

// نوع وسيلة الاتصال
public enum ContactType
{
    [Display(Name = "هاتف محمول")]
    MobilePhone = 1,
    [Display(Name = "هاتف أرضي")]
    Landline = 2,
    [Display(Name = "تيليجرام")]
    Telegram = 3,
    [Display(Name = "واتساب")]
    WhatsApp = 4,
    [Display(Name = "فيسبوك")]
    Facebook = 5,
    [Display(Name = "بريد إلكتروني")]
    Email = 6,
    [Display(Name = "جهاز لاسلكي")]
    RadioDevice = 7 // جهاز لاسلكي
}

// صلة القرابة
public enum RelationshipType
{
    [Display(Name = "أب")]
    Father = 1,
    [Display(Name = "أم")]
    Mother = 2,
    [Display(Name = "أخ")]
    Brother = 3,
    [Display(Name = "أخت")]
    Sister = 4,
    [Display(Name = "عم")]
    Uncle = 5,
    [Display(Name = "ابن عم")]
    Cousin = 6,
    [Display(Name = "نسيب")]
    InLaw = 7,      // نسيب
    [Display(Name = "صديق")]
    Friend = 8,
    [Display(Name = "شريك/زميل")]
    Associate = 9, // شريك/زميل
    [Display(Name = "ابن")]
    Son = 10,
    [Display(Name = "ابنة")]
    Daughter = 11,
    [Display(Name = "زوج / زوجة")]
    Spouse = 12,
    [Display(Name = "خال")]
    MaternalUncle = 13,
    [Display(Name = "ابن خال")]
    MaternalCousin = 14,
    [Display(Name = "عمة")]
    Aunt = 15,
    [Display(Name = "ابنة عمة")]
    MaternalCousinFemale = 16
}

// نوع الدورة التدريبية
public enum CourseType
{
    [Display(Name = "شرعية")]
    Sharia = 1,         // شرعية
    [Display(Name = "عسكرية أساسية")]
    MilitaryBasic = 2,  // عسكرية أساسية
    [Display(Name = "هندسة وتفخيخ")]
    Explosives = 3,     // هندسة وتفخيخ
    [Display(Name = "قنص")]
    Sniping = 4,        // قنص
    [Display(Name = "أمنية/استخبارات")]
    Intelligence = 5,   // أمنية/استخبارات
    [Display(Name = "إدارية/قيادة")]
    Leadership = 6      // إدارية/قيادة
}

// المستوى التنظيمي (للهرمية)
public enum OrgUnitLevel
{
    [Display(Name = "القيادة العامة")]
    GeneralCommand = 1, // القيادة العامة
    [Display(Name = "ولاية")]
    Wilayah = 2,        // ولاية
    [Display(Name = "قاطع")]
    Sector = 3,         // قاطع
    [Display(Name = "كتيبة")]
    Battalion = 4,      // كتيبة
    [Display(Name = "مفرزة")]
    Detachment = 5,     // مفرزة
    [Display(Name = "خلية صغيرة")]
    Cell = 6            // خلية صغيرة
}

// الرتبة/الدور التنظيمي
public enum OrgRole
{
    [Display(Name = "والي")]
    Wali = 1,           // والي
    [Display(Name = "أمير")]
    Emir = 2,           // أمير
    [Display(Name = "جندي")]
    Soldier = 3,        // جندي
    [Display(Name = "ناقل")]
    Transporter = 4,    // ناقل
    [Display(Name = "إداري")]
    Administrator = 5,  // إداري
    [Display(Name = "شرعي/منسّب")]
    Recruiter = 6,      // شرعي/منسّب
    [Display(Name = "انغماسي/استشهادي")]
    SuicideBomber = 7   // انغماسي/استشهادي
}

// حالة القضية
public enum CaseStatus
{
    [Display(Name = "قيد التحقيق")]
    UnderInvestigation = 1, // قيد التحقيق
    [Display(Name = "محالة للمحكمة")]
    ReferredToCourt = 2,    // محالة للمحكمة
    [Display(Name = "صدر حكم")]
    Sentenced = 3,          // صدر حكم
    [Display(Name = "مغلقة لعدم كفاية الأدلة")]
    ClosedInsufficientEvidence = 4, // مغلقة لعدم كفاية الأدلة
    [Display(Name = "أرشيف")]
    Archived = 5
}

// أولوية القضية
public enum PriorityLevel
{
    [Display(Name = "منخفضة")]
    Low = 1,
    [Display(Name = "متوسطة")]
    Medium = 2,
    [Display(Name = "عالية")]
    High = 3,
    [Display(Name = "خطيرة جداً")]
    Critical = 4 // خطيرة جداً (أمن قومي)
}

// نوع الملف المرفق
public enum FileType
{
    [Display(Name = "صوت")]
    Audio = 1,
    [Display(Name = "فيديو")]
    Video = 2,
    [Display(Name = "صورة")]
    Image = 3,
    [Display(Name = "مستند PDF")]
    Document_PDF = 4,
    [Display(Name = "مستند Word")]
    Document_Word = 5,
    [Display(Name = "تقرير جنائي")]
    ForensicReport = 6 // تقرير جنائي
}

// مستوى التنبيه (لقائمة المراقبة)
public enum AlertLevel
{
    [Display(Name = "منخفض")]
    Low = 1,
    [Display(Name = "متوسط")]
    Medium = 2,
    [Display(Name = "عالي")]
    High = 3,
    [Display(Name = "إشعار فوري")]
    RedAlert = 4 // إشعار فوري للقيادة
}

// نوع العملية أو النشاط الإرهابي
public enum OperationType
{
    [Display(Name = "هجوم / صولة")]
    Offensive = 1,          // هجوم / صولة
    [Display(Name = "صد تعرض / رباط")]
    Defensive = 2,          // صد تعرض / رباط
    [Display(Name = "زرع عبوات ناسفة / تفخيخ طرق")]
    IED_Planting = 3,       // زرع عبوات ناسفة / تفخيخ طرق
    [Display(Name = "هجوم بعجلة مفخخة")]
    VBIED_Attack = 4,       // هجوم بعجلة مفخخة
    [Display(Name = "اغتيال (كواتم / عبوات لاصقة)")]
    Assassination = 5,      // اغتيال (كواتم / عبوات لاصقة)
    [Display(Name = "قنص")]
    SniperAttack = 6,       // قنص
    [Display(Name = "عملية انغماسية / استشهادية")]
    SuicideMission = 7,     // عملية انغماسية / استشهادية
    [Display(Name = "رصد / استطلاع")]
    Reconnaissance = 8,     // رصد / استطلاع
    [Display(Name = "دعم لوجستي / نقل مؤن وسلاح")]
    LogisticalSupport = 9,  // دعم لوجستي / نقل مؤن وسلاح
    [Display(Name = "خطف")]
    Kidnapping = 10,        // خطف
    [Display(Name = "تصفية / إعدام")]
    Execution = 11,         // تصفية / إعدام
    [Display(Name = "قصف هاون / صواريخ")]
    MortarShelling = 12     // قصف هاون / صواريخ
}


// الوضع القانوني للمتهم داخل قضية محددة
// (يختلف عن وضعه العام، فقد يكون محكوماً في قضية وموقوفاً قيد التحقيق في أخرى)
public enum LegalStatusInCase
{
    [Display(Name = "موقوف على ذمة التحقيق")]
    DetainedPendingInvestigation = 1, // موقوف على ذمة التحقيق
    [Display(Name = "مُكفَل (مفرج عنه بكفالة)")]
    ReleasedOnBail = 2,               // مُكفَل (مفرج عنه بكفالة)
    [Display(Name = "هارب من العدالة في هذه القضية")]
    Fugitive = 3,                     // هارب من العدالة في هذه القضية
    [Display(Name = "صدر عليه حكم (محكوم)")]
    Sentenced = 4,                    // صدر عليه حكم (محكوم)
    [Display(Name = "مُبرأ (إفراج)")]
    Acquitted = 5,                    // مُبرأ (إفراج)
    [Display(Name = "مفرج عنه لعدم كفاية الأدلة (غلق الدعوى)")]
    ReleasedInsufficientEvidence = 6, // مفرج عنه لعدم كفاية الأدلة (غلق الدعوى)
    [Display(Name = "تحولت صفته إلى شاهد (مخبر سري / شاهد ملك)")]
    Witness = 7,                      // تحولت صفته إلى شاهد (مخبر سري / شاهد ملك)
    [Display(Name = "محال إلى محكمة أخرى/اختصاص آخر")]
    TransferedToOtherCourt = 8        // محال إلى محكمة أخرى/اختصاص آخر
}

// نوع التهمة الموجهة
public enum AccusationType
{
    // الجرائم الأساسية (المادة 4/1 إرهاب)
    [Display(Name = "الانتماء لتنظيم إرهابي")]
    Membership = 1,              // الانتماء لتنظيم إرهابي
    [Display(Name = "المشاركة في عمل إرهابي (تنفيذ)")]
    DirectParticipation = 2,     // المشاركة في عمل إرهابي (تنفيذ)

    // جرائم الدعم (المادة 4/2 وما يتبعها)
    [Display(Name = "تمويل الإرهاب")]
    Funding = 3,                 // تمويل الإرهاب
    [Display(Name = "التستر والإيواء (مضايف)")]
    Harboring = 4,               // التستر والإيواء (مضايف)
    [Display(Name = "الدعم اللوجستي (نقل مؤن، نقل انتحاريين)")]
    LogisticalSupport = 5,       // الدعم اللوجستي (نقل مؤن، نقل انتحاريين)
    [Display(Name = "تصنيع وتطوير أسلحة/متفجرات")]
    Manufacturing = 6,           // تصنيع وتطوير أسلحة/متفجرات
    [Display(Name = "التجنيد والتحريض")]
    Recruitment = 7,             // التجنيد والتحريض

    // جرائم محددة
    [Display(Name = "الخطف")]
    Kidnapping = 8,              // الخطف
    [Display(Name = "زرع العبوات")]
    PlantingIEDs = 9,            // زرع العبوات
    [Display(Name = "الاغتيال")]
    Assassination = 10,          // الاغتيال
    [Display(Name = "الإرهاب الإلكتروني (إدارة قنوات، نشر إصدارات)")]
    CyberTerrorism = 11          // الإرهاب الإلكتروني (إدارة قنوات، نشر إصدارات)
}


public enum InterrogationType
{
    [Display(Name = "استجواب أولي")]
    InitialInquiry = 1,       // استجواب أولي
    [Display(Name = "تدوين إفادة تفصيلية")]
    DetailedConfession = 2,   // تدوين إفادة تفصيلية
    [Display(Name = "مواجهة مع متهم آخر")]
    Confrontation = 3,        // مواجهة مع متهم آخر
    [Display(Name = "كشف دلالة (تمثيل الجريمة)")]
    CrimeSceneReenactment = 4,// كشف دلالة (تمثيل الجريمة)
    [Display(Name = "تصديق أقوال أمام القاضي")]
    JudicialRatification = 5  // تصديق أقوال أمام القاضي
}

public enum InterrogationOutcome
{
    [Display(Name = "اعتراف كامل")]
    FullConfession = 1,       // اعتراف كامل
    [Display(Name = "إقرار جزئي")]
    PartialAdmission = 2,     // إقرار جزئي
    [Display(Name = "إنكار التهمة")]
    Denial = 3,               // إنكار التهمة
    [Display(Name = "أدلى بمعلومات استخبارية (تعاون)")]
    ProvidedIntelligence = 4, // أدلى بمعلومات استخبارية (تعاون)
    [Display(Name = "التزم الصمت")]
    Silent = 5,               // التزم الصمت
    [Display(Name = "أقوال متناقضة")]
    Contradictory = 6         // أقوال متناقضة
}