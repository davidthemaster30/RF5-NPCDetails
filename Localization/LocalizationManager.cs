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
    private const BootOption.SystemLanguage FallbackLang = BootOption.SystemLanguage.English;
    private readonly Dictionary<BootOption.SystemLanguage, Dictionary<string, string>> dic = [];
    private readonly string Path = string.Empty;

    internal LocalizedString(string path)
    {
        Path = path;
    }

    internal static string GetLocaleStr(BootOption.SystemLanguage lang) => lang switch
    {
        BootOption.SystemLanguage.English => "en_US",
        BootOption.SystemLanguage.Japanese => "ja_JP",
        BootOption.SystemLanguage.ChineseSimplified => "zh_CN",
        BootOption.SystemLanguage.ChineseTraditional => "zh_TW",
        BootOption.SystemLanguage.Korean => "ko_KR",
        BootOption.SystemLanguage.French => "fr_FR",
        BootOption.SystemLanguage.Germen => "de_DE",
        _ => string.Empty,
    };

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

    internal string Load(string key) => Load(key, BootSystem.OptionData.SystemLanguage);
    internal string Load(string key, BootOption.SystemLanguage lang)
    {
        if (dic.TryGetValue(lang, out var value))
        {
            return value[key];
        }

        //Load language
        var datas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var curPath = System.IO.Path.Combine(BepInExLoader.GetPluginRootDirectory(), BepInExLoader.GUID, Path, $"{GetLocaleStr(lang)}.json");
        if (File.Exists(curPath))
        {
            var json = File.ReadAllText(curPath);
            datas = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        dic.Add(lang, datas);

        if (dic.TryGetValue(lang, out var value2))
        {
            if (value2.TryGetValue(key, out var value3))
            {
                return value3;
            }
            else
            {
                //Fallback
                BepInExLog.LogWarning($"Missing key for {GetLocaleStr(lang)} : {key}");
                return lang != FallbackLang ? Load(key, FallbackLang) : key;
            }
        }
        else
        {
            //Fallback
            BepInExLog.LogWarning($"Missing language files for {lang}.");
            return lang != FallbackLang ? Load(key, FallbackLang) : key;
        }
    }
}
