using System.Text.RegularExpressions;

namespace NEXUS.Helpers;

public static class ArabicTextHelper
{
    public static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";

        // 1. إزالة المسافات الزائدة من البداية والنهاية
        var text = input.Trim();

        // 2. تقليص المسافات المتعددة في الوسط إلى مسافة واحدة
        // (مثال: "محمد    علي" تصبح "محمد علي")
        text = Regex.Replace(text, @"\s+", " ");

        // 3. إزالة التشكيل (الضمة، الفتحة، الكسرة...)
        text = Regex.Replace(text, @"[\u064B-\u065F]", "");

        // 4. إزالة التطويل (كـشـيـدة) مثل "مــحــمــد"
        text = text.Replace("ـ", "");

        // 5. توحيد الألف (أ، إ، آ -> ا)
        text = text.Replace("أ", "ا").Replace("إ", "ا").Replace("آ", "ا");

        // 6. توحيد الياء والألف المقصورة (ى -> ي) 
        // ملاحظة: في بعض الأنظمة يفضلون العكس، لكن في العراق/الخليج الياء (ي) هي الغالبة
        text = text.Replace("ى", "ي");

        // 7. توحيد التاء المربوطة والهاء (ة -> هـ)
        // هذا خيار استراتيجي: لأن الناس تخطئ وتكتب "فاطمه" بدل "فاطمة" كثيراً
        text = text.Replace("ة", "ه");

        return text;
    }
}