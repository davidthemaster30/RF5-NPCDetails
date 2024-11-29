using BepInEx.Configuration;
using RF5.HisaCat.NPCDetails.Utils;

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
        if (EnableLogging.Value && obj is not null)
        {
            log.LogDebug($"{obj}");
        }
    }

    internal static void LogInfo(object obj)
    {
        if (EnableLogging.Value && obj is not null)
        {
            log.LogInfo($"{obj}");
        }
    }

    internal static void LogMessage(object obj)
    {
        if (EnableLogging.Value && obj is not null)
        {
            log.LogMessage($"[{obj}");
        }
    }

    internal static void LogError(object obj)
    {
        if (EnableLogging.Value && obj is not null)
        {
            log.LogError($"{obj}");
        }
    }

    internal static void LogWarning(object obj)
    {
        if (EnableLogging.Value && obj is not null)
        {
            log.LogWarning($"{obj}");
        }
    }

    internal static void LogNPCData(NpcData npcData)
    {
        if (EnableLogging.Value && npcData is not null)
        {
            log.LogDebug($"{npcData.GetNpcName()} ({npcData.NpcId})");
            log.LogDebug($"{npcData.GetNpcDiscript()}");
            log.LogDebug($" - CurrentPlace: {npcData.CurrentPlace}"); //Print current npc's place.
            log.LogDebug($" - TargetPlace: {npcData.TargetPlace}"); //Display npc's destination if it exist.
            log.LogDebug($" - Loves: {string.Join(", ", npcData.GetVeryFavoriteItemDataTables().Select(x => $"{x.GetItemName()}({(int)ItemDataTable.GetItemID(x.ItemIndex)})"))}");
            log.LogDebug($" - Likes: {string.Join(", ", npcData.GetFavoriteItemDataTables().Select(x => $"{x.GetItemName()}({(int)ItemDataTable.GetItemID(x.ItemIndex)})"))}");
            log.LogDebug($" - Dislikes: {string.Join(", ", npcData.GetNotFavoriteItemDataTables().Select(x => $"{x.GetItemName()}({(int)ItemDataTable.GetItemID(x.ItemIndex)})"))}");
            log.LogDebug($" - Hates: {string.Join(", ", npcData.GetNotFavoriteBadlyItemDataTables().Select(x => $"{x.GetItemName()}({(int)ItemDataTable.GetItemID(x.ItemIndex)})"))}");
            log.LogDebug("");
        }
    }
}

