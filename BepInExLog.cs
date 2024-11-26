using BepInEx.Configuration;

namespace RF5.HisaCat.NPCDetails;

internal static class BepInExLog
{
    internal static ConfigEntry<bool> EnableLogging;
    internal readonly static BepInEx.Logging.ManualLogSource log = BepInEx.Logging.Logger.CreateLogSource("RF5.HisaCat.NPCDetails");
    /// <summary>
    /// Tips: BepInEx 'Debug' LogLevel is ignored defaults. if you want to see, edit 'LogLevels' option on '[Logging.Console]' section.
    /// </summary>
    /// <param name="obj"></param>
    internal static void LogDebug(object obj)
    {
        if (EnableLogging.Value)
        {
            log.LogDebug($"{obj}");
        }
    }

    internal static void LogInfo(object obj)
    {
        if (EnableLogging.Value)
        {
            log.LogInfo($"{obj}");
        }
    }

    internal static void LogMessage(object obj)
    {
        if (EnableLogging.Value)
        {
            log.LogMessage($"[{obj}");
        }
    }

    internal static void LogError(object obj)
    {
        if (EnableLogging.Value)
        {
            log.LogError($"{obj}");
        }
    }

    internal static void LogWarning(object obj)
    {
        if (EnableLogging.Value)
        {
            log.LogWarning($"{obj}");
        }
    }
}

