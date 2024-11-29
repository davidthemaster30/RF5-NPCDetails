using UnityEngine;
using UnityEngine.UI;
using RF5.HisaCat.NPCDetails.Utils;
using RF5.HisaCat.NPCDetails.Localization;
using BepInEx.Configuration;

namespace RF5.HisaCat.NPCDetails.NPCDetailWindow;

/// <summary>
/// Attachments for RightStatusPos
/// </summary>
internal class Attachment_RightStatusPos : MonoBehaviour
{
    internal static Attachment_RightStatusPos? Instance { get; private set; }

    private const string RightSection = "RightSection";
    private static ConfigEntry<bool> ShowBirthday;
    private static ConfigEntry<bool> ShowLikes;
    private static ConfigEntry<bool> ShowLoves;
    private static ConfigEntry<bool> ShowHates;
    private static ConfigEntry<bool> ShowTamingItems;
    private static ConfigEntry<bool> ShowDropItems;
    private static ConfigEntry<bool> ShowProduceItems;

    internal static void LoadConfig(ConfigFile Config)
    {
        ShowBirthday = Config.Bind(RightSection, nameof(ShowBirthday), true, "Set to true to enable showing the NPC's Birthday.");
        ShowBirthday.SettingChanged += ResetNPCTextCache;
        ShowLikes = Config.Bind(RightSection, nameof(ShowLikes), true, "Set to true to enable showing the NPC's liked items.");
        ShowLikes.SettingChanged += ResetNPCTextCache;
        ShowLoves = Config.Bind(RightSection, nameof(ShowLoves), true, "Set to true to enable showing the NPC's loved items.");
        ShowLoves.SettingChanged += ResetNPCTextCache;
        ShowHates = Config.Bind(RightSection, nameof(ShowHates), true, "Set to true to enable showing the NPC's hated items.");
        ShowHates.SettingChanged += ResetNPCTextCache;

        ShowTamingItems = Config.Bind(RightSection, nameof(ShowTamingItems), true, "Set to true to enable showing a monster's liked items for taming.");
        ShowTamingItems.SettingChanged += ResetMonsterTextCache;
        ShowDropItems = Config.Bind(RightSection, nameof(ShowDropItems), true, "Set to true to enable showing a monster's drop items.");
        ShowDropItems.SettingChanged += ResetMonsterTextCache;
        ShowProduceItems = Config.Bind(RightSection, nameof(ShowProduceItems), true, "Set to true to enable showing a monster's produced items.");
        ShowProduceItems.SettingChanged += ResetMonsterTextCache;
    }

    internal static void ResetNPCTextCache(object sender, EventArgs e)
    {
        detailTextNPCCache?.Clear();
    }

    internal static void ResetMonsterTextCache(object sender, EventArgs e)
    {
        detailTextMonsterCache?.Clear();
    }

    private const string PrefabPathFromBundle = "[RF5.HisaCat.NPCDetails]RightStatusPos";
    private const string AttachPathBasedFriendPageStatusDisp = "StatusObj/FriendsStatus/RightStatusPos";
    private const string EquipMenuItemDetailWindowPath = "StatusObj/FriendsStatus/EquipMenuItemDetail/OnOffWindows";

    private static class TransformPaths
    {
        internal const string Window = "Window";

        internal const string Status_NPC = "Status_NPC";
        internal const string NPC_DetailText = "TextArea/Mask/Text";

        internal const string Status_Monster = "Status_Monster";
        internal const string Monster_DetailText = "TextArea/Mask/Text";
    }

    private GameObject? m_Window_GO = null;
    private GameObject? m_Status_NPC_GO = null;
    private GameObject? m_Status_Monster_GO = null;

    private Text? m_NPC_DetailText = null;
    private Text? m_Monster_DetailText = null;

    private static Dictionary<Define.ActorID, string>? detailTextNPCCache = null;
    private static Dictionary<MonsterID, string>? detailTextMonsterCache = null;
    private UIOnOffAnimate equipMenuItemDetail = null;

    private bool PreloadPathes()
    {
        if (!this.TryFindGameObject(TransformPaths.Window, out m_Window_GO))
        {
            return false;
        }

        if (!m_Window_GO.TryFindGameObject(TransformPaths.Status_NPC, out m_Status_NPC_GO))
        {
            return false;
        }

        if (!m_Status_NPC_GO.TryFindComponent<Text>(TransformPaths.NPC_DetailText, out m_NPC_DetailText))
        {
            return false;
        }

        if (!m_Window_GO.TryFindGameObject(TransformPaths.Status_Monster, out m_Status_Monster_GO))
        {
            return false;
        }

        if (!m_Status_Monster_GO.TryFindComponent<Text>(TransformPaths.Monster_DetailText, out m_Monster_DetailText))
        {
            return false;
        }

        return true;
    }

    internal bool Init(UIOnOffAnimate equipMenuItemDetail)
    {
        detailTextNPCCache ??= [];
        detailTextMonsterCache ??= [];

        this.equipMenuItemDetail = equipMenuItemDetail;
        return PreloadPathes();
    }

    internal static bool InstantiateAndAttach(FriendPageStatusDisp friendPageStatusDisp)
    {
        if (Instance is not null)
        {
            BepInExLog.LogDebug("[Attachment_RightStatusPos] InstantiateAndAttach: instance already exist");
            return true;
        }

        var attachTarget = friendPageStatusDisp.transform.Find(AttachPathBasedFriendPageStatusDisp);
        if (attachTarget is null)
        {
            BepInExLog.LogError("[Attachment_RightStatusPos] InstantiateAndAttach: Cannot find attachTarget");
            return false;
        }

        var equipMenuItemDetail = friendPageStatusDisp.FindComponent<UIOnOffAnimate>(EquipMenuItemDetailWindowPath);
        if (equipMenuItemDetail is null)
        {
            BepInExLog.LogError("[Attachment_RightStatusPos] InstantiateAndAttach: Cannot find equipMenuItemDetailWindow");
            return false;
        }

        var prefab = BundleLoader.MainBundle.LoadIL2CPP<GameObject>(PrefabPathFromBundle);
        if (prefab is null)
        {
            BepInExLog.LogError("[Attachment_RightStatusPos] InstantiateAndAttach: Cannot load prefab");
            return false;
        }

        var InstanceGO = GameObject.Instantiate(prefab, attachTarget.transform);
        if (InstanceGO is null)
        {
            BepInExLog.LogError("[Attachment_RightStatusPos] InstantiateAndAttach: Cannot instantiate window");
            return false;
        }
        RF5FontHelper.SetFontGlobal(InstanceGO);

        Instance = InstanceGO.AddComponent<Attachment_RightStatusPos>();
        if (!Instance.Init(equipMenuItemDetail))
        {
            BepInExLog.LogError("[Attachment_RightStatusPos] InstantiateAndAttach: Initialize failed");
            Instance = null;
            Destroy(InstanceGO);
            return false;
        }

        BepInExLog.LogInfo("[Attachment_RightStatusPos] Attached");
        return true;
    }

    internal void SetShown(bool isShown)
    {
        gameObject.SetActive(isShown);
    }

    internal bool GetShown()
    {
        return gameObject.activeSelf;
    }

    private void ResetShown()
    {
        m_Window_GO?.SetActive(false);
        m_Status_NPC_GO?.SetActive(false);
        m_Status_Monster_GO?.SetActive(false);
    }

    internal void SetNPCData(NpcData npcData)
    {
        ResetShown();

        if ((!ShowBirthday.Value && !ShowLikes.Value && !ShowLoves.Value && !ShowHates.Value) || npcData is null)
        {
            return;
        }

        m_Status_NPC_GO.SetActive(true);

        m_NPC_DetailText.text = GetDetailText(npcData);
    }

    private static string GetDetailText(NpcData npcData)
    {
        if (detailTextNPCCache.TryGetValue(npcData.actorId, out var detailText))
        {
            return detailText;
        }

        var text_Birthday = LocalizationManager.Load("npc.detail.title.birthday");
        var text_Loves = LocalizationManager.Load("npc.detail.title.loves");
        var text_Likes = LocalizationManager.Load("npc.detail.title.likes");
        var text_Dislikes = LocalizationManager.Load("npc.detail.title.dislikes");
        var text_Hates = LocalizationManager.Load("npc.detail.title.hates");

        var text = string.Empty;

        if (npcData.TryFindNPCBirthday(out Define.Season birthday_season, out int birthday_day))
        {
            var birthdayText = birthday_season switch
            {
                Define.Season.Spring => $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_SPRING)} {birthday_day}",
                Define.Season.Summer => $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_SUMMER)} {birthday_day}",
                Define.Season.Autumn => $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_AUTUMN)} {birthday_day}",
                Define.Season.Winter => $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_WINTER)} {birthday_day}",
                _ => string.Empty
            };
            text += $"<size=25>{text_Birthday}:</size> {birthdayText}{Environment.NewLine}{Environment.NewLine}";
        }

        text += $"<size=25>{text_Loves}</size>{Environment.NewLine}{string.Join(", ", npcData.GetVeryFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}{Environment.NewLine}{Environment.NewLine}";
        text += $"<size=25>{text_Likes}</size>{Environment.NewLine}{string.Join(", ", npcData.GetFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}{Environment.NewLine}{Environment.NewLine}";
        text += $"<size=25>{text_Dislikes}</size>{Environment.NewLine}{string.Join(", ", npcData.GetNotFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}{Environment.NewLine}{Environment.NewLine}";
        text += $"<size=25>{text_Hates}</size>{Environment.NewLine}{string.Join(", ", npcData.GetNotFavoriteBadlyItemDataTables().Select(x => $"{x.GetItemName()}"))}{Environment.NewLine}";

        detailTextNPCCache.Add(npcData.actorId, text);
        return text;
    }

    internal void SetMonsterData(MonsterDataTable monsterData)
    {
        ResetShown();

        if ((!ShowTamingItems.Value && !ShowDropItems.Value) || monsterData is null)
        {
            return;
        }

        m_Status_Monster_GO.SetActive(true);

        m_Monster_DetailText.text = GetDetailText(monsterData);
    }

    private static string GetDetailText(MonsterDataTable monsterData)
    {
        if (detailTextMonsterCache.TryGetValue(monsterData.MonsterId, out var detailText))
        {
            return detailText;
        }

        var text = string.Empty;

        text += $"<size=25>{LocalizationManager.Load("monster.detail.title.origin_name")}:</size> {RF5DataExtension.GetMonsterName(monsterData.MonsterId)}{Environment.NewLine}{Environment.NewLine}";

        if (ShowTamingItems.Value)
        {
            text += $"<size=25>{LocalizationManager.Load("monster.detail.title.favorites")}</size>{Environment.NewLine}{string.Join(", ", monsterData.GetFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}{Environment.NewLine}{Environment.NewLine}";
        }

        if (ShowProduceItems.Value && monsterData.YieldItemIDArray.Count > 0)
        {
            text += $"<size=25>Monster Produce</size>{Environment.NewLine}{string.Join(", ", RF5DataExtension.ItemIdArrayToItemDataTables(monsterData.YieldItemIDArray.ToArray()).Select(x => $"{x.GetItemName()}"))}{Environment.NewLine}{Environment.NewLine}";
        }

        if (ShowDropItems.Value)
        {
            var dropItemData = MonsterDropItemDataTable.GetDataTable(monsterData.DropItemDataID);
            if (dropItemData is not null)
            {
                if (dropItemData.DropItemParamList.Count > 0)
                {
                    text += $"<size=25>Monster Drops</size>{Environment.NewLine}{GetNameAndDropRate(dropItemData.DropItemParamList.ToArray())}{Environment.NewLine}{Environment.NewLine}";
                }

                if (dropItemData.BonusDropItemParamList.Count > 0)
                {
                    text += $"<size=25>Monster Bonus Drops</size>{Environment.NewLine}{GetNameAndDropRate(dropItemData.BonusDropItemParamList.ToArray())}{Environment.NewLine}{Environment.NewLine}";
                }

                if (dropItemData.HandcuffsDropItemParamList.Count > 0)
                {
                    text += $"<size=25>Monster Seal Drops</size>{Environment.NewLine}{GetNameAndDropRate(dropItemData.HandcuffsDropItemParamList.ToArray())}{Environment.NewLine}{Environment.NewLine}";
                }
            }
        }

        detailTextMonsterCache.Add(monsterData.MonsterId, text);
        return text;
    }

    internal static string GetNameAndDropRate(IEnumerable<DropItemParam> items)
    {
        var text = string.Empty;
        if (items.Any())
        {
            text = $"{string.Join(", ", items.Select(x => $"{ItemDataTable.GetDataTable(x.ItemID).GetItemName()} ({string.Format("{0:F}", x.DropPercent / 10.0)}%)"))}";
        }

        return text;
    }

    private void Update()
    {
        bool equipMenuItemDetailOpened = equipMenuItemDetail.gameObject.activeInHierarchy && equipMenuItemDetail.isOpen;
        if (!equipMenuItemDetailOpened != m_Window_GO.gameObject.activeSelf)
        {
            m_Window_GO.gameObject.SetActive(!equipMenuItemDetailOpened);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            BepInExLog.LogDebug("[Attachment_RightStatusPos] Destroyed");
            Instance = null;
        }
    }
}
