using UnityEngine;
using UnityEngine.UI;
using RF5.HisaCat.NPCDetails.Utils;
using RF5.HisaCat.NPCDetails.Localization;

namespace RF5.HisaCat.NPCDetails.NPCDetailWindow;

/// <summary>
/// Attachments for RightStatusPos
/// </summary>
internal class Attachment_RightStatusPos : MonoBehaviour
{
    internal static Attachment_RightStatusPos? Instance { get; private set; }

    internal const string PrefabPathFromBundle = "[RF5.HisaCat.NPCDetails]RightStatusPos";

    internal const string AttachPathBasedFriendPageStatusDisp = "StatusObj/FriendsStatus/RightStatusPos";
    internal const string EquipMenuItemDetailWindowPath = "StatusObj/FriendsStatus/EquipMenuItemDetail/OnOffWindows";

    internal static class TransformPaths
    {
        internal const string Window = "Window";

        internal const string Status_NPC = "Status_NPC";
        internal const string NPC_DetailText = "TextArea/Mask/Text";

        internal const string Status_Monster = "Status_Monster";
        internal const string Monster_DetailText = "TextArea/Mask/Text";
    }

    private GameObject m_Window_GO = null;

    internal GameObject m_Status_NPC_GO = null;
    internal GameObject m_Status_Monster_GO = null;

    private Text m_NPC_DetailText = null;
    private Text m_Monster_DetailText = null;
    internal bool PreloadPathes()
    {
        {
            GameObject root;
            if (!this.TryFindGameObject(TransformPaths.Window, out root))
            {
                return false;
            }

            m_Window_GO = root;

            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.Status_NPC, out parent))
                {
                    return false;
                }

                m_Status_NPC_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.NPC_DetailText, out m_NPC_DetailText))
                {
                    return false;
                }
            }
            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.Status_Monster, out parent))
                {
                    return false;
                }

                m_Status_Monster_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.Monster_DetailText, out m_Monster_DetailText))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private FriendPageStatusDisp friendPageStatusDisp = null;
    private UIOnOffAnimate equipMenuItemDetail = null;
    internal bool Init(FriendPageStatusDisp friendPageStatusDisp, UIOnOffAnimate equipMenuItemDetail)
    {
        this.friendPageStatusDisp = friendPageStatusDisp;
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
        if (!Instance.Init(friendPageStatusDisp, equipMenuItemDetail))
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

    internal void SetNPCData(NpcData npcData)
    {
        m_Status_NPC_GO.SetActive(true);
        m_Status_Monster_GO.SetActive(false);

        m_NPC_DetailText.text = GetDetailText(npcData);
    }

    private static Dictionary<Define.ActorID, string> detailTextDic = null;
    private static string GetDetailText(NpcData npcData)
    {
        detailTextDic ??= new Dictionary<Define.ActorID, string>();

        if (!detailTextDic.ContainsKey(npcData.actorId))
        {
            var text_Birthday = LocalizationManager.Load("npc.detail.title.birthday");
            var text_Loves = LocalizationManager.Load("npc.detail.title.loves");
            var text_Likes = LocalizationManager.Load("npc.detail.title.likes");
            var text_Dislikes = LocalizationManager.Load("npc.detail.title.dislikes");
            var text_Hates = LocalizationManager.Load("npc.detail.title.hates");

            var text = string.Empty;

            Define.Season birthday_season;
            int birthday_day;
            if (npcData.TryFindNPCBirthday(out birthday_season, out birthday_day))
            {
                var birthdayText = string.Empty;
                switch (birthday_season)
                {
                    case Define.Season.Spring:
                        birthdayText = $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_SPRING)} {birthday_day}";
                        break;
                    case Define.Season.Summer:
                        birthdayText = $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_SUMMER)} {birthday_day}";
                        break;
                    case Define.Season.Autumn:
                        birthdayText = $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_AUTUMN)} {birthday_day}";
                        break;
                    case Define.Season.Winter:
                        birthdayText = $"{SV.UIRes.GetSystemText(UITextDic.DICID.HUDCLOCK_WINTER)} {birthday_day}";
                        break;
                }
                text += $"<size=25>{text_Birthday}:</size> {birthdayText}\r\n\r\n";
            }

            text += $"<size=25>{text_Loves}</size>\r\n{string.Join(", ", npcData.GetVeryFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
            text += $"<size=25>{text_Likes}</size>\r\n{string.Join(", ", npcData.GetFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
            text += $"<size=25>{text_Dislikes}</size>\r\n{string.Join(", ", npcData.GetNotFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
            text += $"<size=25>{text_Hates}</size>\r\n{string.Join(", ", npcData.GetNotFavoriteBadlyItemDataTables().Select(x => $"{x.GetItemName()}"))}";

            //https://www.nexusmods.com/runefactory5/mods/34?tab=bugs
            //Hotfix for 'Text cut off' bugs
            text += "\r\n";

            detailTextDic.Add(npcData.actorId, text);
        }
        return detailTextDic[npcData.actorId];
    }

    internal void SetMonsterData(FriendMonsterStatusData friendMonsterData, MonsterDataTable monsterData)
    {
        m_Status_NPC_GO.SetActive(false);
        m_Status_Monster_GO.SetActive(true);

        var text = string.Empty;

        text += $"<size=25>{LocalizationManager.Load("monster.detail.title.origin_name")}:</size> {RF5DataExtension.GetMonsterName(monsterData.MonsterId)}\r\n\r\n";
        text += $"<size=25>{LocalizationManager.Load("monster.detail.title.favorites")}</size>\r\n{string.Join(", ", monsterData.GetFavoriteItemDataTables().Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
        //var dropItemData = MonsterDropItemDataTable.GetDataTable(monsterData.DropItemDataID);
        //if(dropItemData is not null)
        //{
        //    text += $"<size=25>{LocalizationManager.Load("")}</size>\r\n{string.Join(", ", RF5DataExtension.ItemIdArrayToItemDataTables(dropItemData.DropItemParamList.ToArray().Select(x => x.ItemID)).Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
        //    text += $"<size=25>{LocalizationManager.Load("")}</size>\r\n{string.Join(", ", RF5DataExtension.ItemIdArrayToItemDataTables(dropItemData.BonusDropItemParamList.ToArray().Select(x => x.ItemID)).Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
        //    text += $"<size=25>{LocalizationManager.Load("")}</size>\r\n{string.Join(", ", RF5DataExtension.ItemIdArrayToItemDataTables(dropItemData.HandcuffsDropItemParamList.ToArray().Select(x => x.ItemID)).Select(x => $"{x.GetItemName()}"))}\r\n\r\n";
        //}

        text += "\r\n";
        m_Monster_DetailText.text = text;
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
