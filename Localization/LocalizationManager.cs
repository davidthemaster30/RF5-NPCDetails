using System.Text.Json;

namespace RF5.HisaCat.NPCDetails.Localization;

internal static class LocalizationManager
{
    private static Dictionary<string, LocalizedString>? dic = null;

    internal const string MainPath = "Localized";
    internal static string Load(string key) => Load(key, BootSystem.OptionData.SystemLanguage);
    internal static string Load(string key, BootOption.SystemLanguage lang) => Load(MainPath, key, lang);
    internal static string Load(string path, string key) => Load(path, key, BootSystem.OptionData.SystemLanguage);
    internal static string Load(string path, string key, BootOption.SystemLanguage lang)
    {
        dic ??= new Dictionary<string, LocalizedString>(StringComparer.OrdinalIgnoreCase);

        if (!dic.ContainsKey(path))
        {
            dic.Add(path, new LocalizedString(path));
        }

        return dic[path].Load(key, lang);
    }
}

internal class LocalizedString
{
    internal static string GetLocaleStr(BootOption.SystemLanguage lang)
    {
        switch (lang)
        {
            case BootOption.SystemLanguage.English:
                return "en_US";
            case BootOption.SystemLanguage.Japanese:
                return "ja_JP";
            case BootOption.SystemLanguage.ChineseSimplified:
                return "zh_CN";
            case BootOption.SystemLanguage.ChineseTraditional:
                return "zh_TW";
            case BootOption.SystemLanguage.Korean:
                return "ko_KR";
            case BootOption.SystemLanguage.French:
                return "fr_FR";
            case BootOption.SystemLanguage.Germen:
                return "de_DE";
            default:
                return string.Empty;
        }
    }
    internal static void CreateTemplate()
    {
        var data = new Dictionary<string, string>();
        for (int i = 0; i < 3; i++)
        {
            data.Add($"key{i}", "text");
        }

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });

        for (var lang = BootOption.SystemLanguage.English; lang <= BootOption.SystemLanguage.Germen; lang++)
        {
            var path = System.IO.Path.Combine(BepInExLoader.GetPluginRootDirectory(), BepInExLoader.GUID, $"{GetLocaleStr(lang)}.json");
            File.WriteAllText(path, json);
        }
    }

    internal const BootOption.SystemLanguage FallbackLang = BootOption.SystemLanguage.English;
    private Dictionary<BootOption.SystemLanguage, Dictionary<string, string>> dic = null;
    internal readonly string Path = string.Empty;
    internal LocalizedString(string path)
    {
        Path = path;
        dic = new Dictionary<BootOption.SystemLanguage, Dictionary<string, string>>();
    }

    internal string Load(string key) => Load(key, BootSystem.OptionData.SystemLanguage);
    internal string Load(string key, BootOption.SystemLanguage lang)
    {
        if (!dic.ContainsKey(lang))
        {
            //Load.
            var datas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var curPath = System.IO.Path.Combine(BepInExLoader.GetPluginRootDirectory(), BepInExLoader.GUID, Path, $"{GetLocaleStr(lang)}.json");
            if (File.Exists(curPath))
            {
                var json = File.ReadAllText(curPath);
                datas = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            dic.Add(lang, datas);
        }

        if (dic[lang].ContainsKey(key))
        {
            return dic[lang][key];
        }
        else
        {
            //Fallback
            return lang != FallbackLang ? Load(key, FallbackLang) : key;
        }
    }
}
