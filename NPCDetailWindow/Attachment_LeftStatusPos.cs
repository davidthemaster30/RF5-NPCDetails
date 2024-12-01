using UnityEngine;
using UnityEngine.UI;
using RF5.HisaCat.NPCDetails.Utils;
using RF5.HisaCat.NPCDetails.Localization;
using BepInEx.Configuration;

namespace RF5.HisaCat.NPCDetails.NPCDetailWindow;

/// <summary>
/// Attachments for LeftStatusPos
/// </summary>
internal class Attachment_LeftStatusPos : MonoBehaviour
{
    internal static Attachment_LeftStatusPos? Instance { get; private set; }

    private const string LeftSection = "LeftSection";
    private static ConfigEntry<bool> ShowBirthdayTip;
    private static ConfigEntry<bool> ShowTalkedTip;
    private static ConfigEntry<bool> ShowNPCGiftDeliveredTip;
    private static ConfigEntry<bool> ShowMonsterGiftDeliveredTip;
    private static ConfigEntry<bool> ShowMonsterBrushedTip;
    private static ConfigEntry<bool> ShowMonsterHelpingTip;

    internal static void LoadConfig(ConfigFile Config)
    {
        ShowBirthdayTip = Config.Bind(LeftSection, nameof(ShowBirthdayTip), true, "Set to true to enable showing the NPC's Birthday tip.");
        ShowTalkedTip = Config.Bind(LeftSection, nameof(ShowTalkedTip), true, "Set to true to enable showing if the NPC was talked to tip.");
        ShowNPCGiftDeliveredTip = Config.Bind(LeftSection, nameof(ShowNPCGiftDeliveredTip), true, "Set to true to enable if the NPC was gifted tip.");
        ShowMonsterGiftDeliveredTip = Config.Bind(LeftSection, nameof(ShowMonsterGiftDeliveredTip), true, "Set to true to enable showing if a monster was gifted.");
        ShowMonsterBrushedTip = Config.Bind(LeftSection, nameof(ShowMonsterBrushedTip), true, "Set to true to enable showing if a monster was brushed.");
        ShowMonsterHelpingTip = Config.Bind(LeftSection, nameof(ShowMonsterHelpingTip), true, "Set to true to enable showing if and where a monster is helping on the farm.");
    }

    private const string PrefabPathFromBundle = "[RF5.HisaCat.NPCDetails]LeftStatusPos";
    private const string AttachPathBasedFriendPageStatusDisp = "StatusObj/FriendsStatus/LeftStatusPos";

    private static class TransformPaths
    {
        internal const string Status_NPC = "RF5ContentsArea/Status_NPC";
        internal const string NPC_Tips_TodayTalked = "TipsArea/Tips_TodayTalked";
        internal const string NPC_Tips_TodayTalked_Text = "Text";
        internal const string NPC_Tips_TodayTalked_Checked = "Checkbox/checked";

        internal const string NPC_Tips_WasPresent = "TipsArea/Tips_WasPresent";
        internal const string NPC_Tips_WasPresent_Text = "Text";
        internal const string NPC_Tips_WasPresent_CheckedNormal = "Checkbox_Normal/checked";
        internal const string NPC_Tips_WasPresent_CheckedLikes = "Checkbox_Likes/checked";
        internal const string NPC_Tips_WasPresent_CheckedLoves = "Checkbox_Loves/checked";

        internal const string NPC_Tips_BirthdayLeft = "TipsArea/Tips_BirthdayLeft";
        internal const string NPC_Tips_BirthdayLeft_Text = "Text";

        internal const string NPC_Tips_Alert_BirthdayToday = "TipsArea/Tips_Alert_BirthdayToday";
        internal const string NPC_Tips_Alert_BirthdayToday_Text = "Text";

        internal const string Status_Monster = "RF5ContentsArea/Status_Monster";
        internal const string Monster_Tips_BrushingToday = "TipsArea/Tips_BrushingToday";
        internal const string Monster_Tips_BrushingToday_Text = "Text";
        internal const string Monster_Tips_BrushingToday_Checked = "Checkbox/checked";

        internal const string Monster_Tips_WasPresent = "TipsArea/Tips_WasPresent";
        internal const string Monster_Tips_WasPresent_Text = "Text";
        internal const string Monster_Tips_WasPresent_Checked = "Checkbox/checked";

        internal const string Monster_Tips_HelpingFarm = "TipsArea/Tips_HelpingFarm";
        internal const string Monster_Tips_HelpingFarm_Text_Title = "Text_Title";
        internal const string Monster_Tips_HelpingFarm_Text_Place = "Text_Place";
    }


    private GameObject? m_Status_NPC_GO = null;
    private GameObject? m_Status_Monster_GO = null;

    private GameObject? m_NPC_Tips_TodayTalked_GO = null;
    private Text? m_NPC_Tips_TodayTalked_Text = null;
    private GameObject? m_NPC_Tips_TodayTalked_Checked = null;

    private GameObject? m_NPC_Tips_WasPresent_GO = null;
    private Text? m_NPC_Tips_WasPresent_Text = null;
    private GameObject? m_NPC_Tips_WasPresent_CheckedNormal = null;
    private GameObject? m_NPC_Tips_WasPresent_CheckedLikes = null;
    private GameObject? m_NPC_Tips_WasPresent_CheckedLoves = null;

    private GameObject? m_NPC_Tips_BirthdayLeft_GO = null;
    private Text? m_NPC_Tips_BirthdayLeft_Text = null;

    private GameObject? m_NPC_Tips_Alert_BirthdayToday_GO = null;
    private Text? m_NPC_Tips_Alert_BirthdayToday_Text = null;

    private GameObject? m_Monster_Tips_BrushingToday_GO = null;
    private Text? m_Monster_Tips_BrushingToday_Text = null;
    private GameObject? m_Monster_Tips_BrushingToday_Checked = null;

    private GameObject? m_Monster_Tips_WasPresent_GO = null;
    private Text? m_Monster_Tips_WasPresent_Text = null;
    private GameObject? m_Monster_Tips_WasPresent_Checked = null;

    private GameObject? m_Monster_Tips_HelpingFarm_GO = null;
    private Text? m_Monster_Tips_HelpingFarm_Text_Title = null;
    private Text? m_Monster_Tips_HelpingFarm_Text_Place = null;

    private bool PreloadPaths()
    {
        return PreloadNPCPaths() && PreloadMonsterPaths();
    }

    private bool PreloadNPCPaths()
    {
        if (!ShowBirthdayTip.Value && !ShowTalkedTip.Value && !ShowNPCGiftDeliveredTip.Value)
        {
            //dont preload if nothing to show
            return true;
        }

        if (!this.TryFindGameObject(TransformPaths.Status_NPC, out m_Status_NPC_GO))
        {
            return false;
        }

        if (!m_Status_NPC_GO.TryFindGameObject(TransformPaths.NPC_Tips_TodayTalked, out m_NPC_Tips_TodayTalked_GO))
        {
            return false;
        }

        if (!m_NPC_Tips_TodayTalked_GO.TryFindComponent<Text>(TransformPaths.NPC_Tips_TodayTalked_Text, out m_NPC_Tips_TodayTalked_Text))
        {
            return false;
        }

        if (!m_NPC_Tips_TodayTalked_GO.TryFindGameObject(TransformPaths.NPC_Tips_TodayTalked_Checked, out m_NPC_Tips_TodayTalked_Checked))
        {
            return false;
        }

        if (!m_Status_NPC_GO.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent, out m_NPC_Tips_WasPresent_GO))
        {
            return false;
        }

        if (!m_NPC_Tips_WasPresent_GO.TryFindComponent<Text>(TransformPaths.NPC_Tips_WasPresent_Text, out m_NPC_Tips_WasPresent_Text))
        {
            return false;
        }

        if (!m_NPC_Tips_WasPresent_GO.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent_CheckedNormal, out m_NPC_Tips_WasPresent_CheckedNormal))
        {
            return false;
        }

        if (!m_NPC_Tips_WasPresent_GO.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent_CheckedLikes, out m_NPC_Tips_WasPresent_CheckedLikes))
        {
            return false;
        }

        if (!m_NPC_Tips_WasPresent_GO.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent_CheckedLoves, out m_NPC_Tips_WasPresent_CheckedLoves))
        {
            return false;
        }

        if (!m_Status_NPC_GO.TryFindGameObject(TransformPaths.NPC_Tips_BirthdayLeft, out m_NPC_Tips_BirthdayLeft_GO))
        {
            return false;
        }

        if (!m_NPC_Tips_BirthdayLeft_GO.TryFindComponent<Text>(TransformPaths.NPC_Tips_BirthdayLeft_Text, out m_NPC_Tips_BirthdayLeft_Text))
        {
            return false;
        }

        if (!m_Status_NPC_GO.TryFindGameObject(TransformPaths.NPC_Tips_Alert_BirthdayToday, out m_NPC_Tips_Alert_BirthdayToday_GO))
        {
            return false;
        }

        if (!m_NPC_Tips_Alert_BirthdayToday_GO.TryFindComponent<Text>(TransformPaths.NPC_Tips_Alert_BirthdayToday_Text, out m_NPC_Tips_Alert_BirthdayToday_Text))
        {
            return false;
        }

        return true;
    }

    private bool PreloadMonsterPaths()
    {
        if (!ShowMonsterGiftDeliveredTip.Value && !ShowMonsterGiftDeliveredTip.Value)
        {
            //dont preload if nothing to show
            return true;
        }

        if (!this.TryFindGameObject(TransformPaths.Status_Monster, out m_Status_Monster_GO))
        {
            return false;
        }

        if (!m_Status_Monster_GO.TryFindGameObject(TransformPaths.Monster_Tips_BrushingToday, out m_Monster_Tips_BrushingToday_GO))
        {
            return false;
        }

        if (!m_Monster_Tips_BrushingToday_GO.TryFindComponent<Text>(TransformPaths.Monster_Tips_BrushingToday_Text, out m_Monster_Tips_BrushingToday_Text))
        {
            return false;
        }

        if (!m_Monster_Tips_BrushingToday_GO.TryFindGameObject(TransformPaths.Monster_Tips_BrushingToday_Checked, out m_Monster_Tips_BrushingToday_Checked))
        {
            return false;
        }

        if (!m_Status_Monster_GO.TryFindGameObject(TransformPaths.Monster_Tips_WasPresent, out m_Monster_Tips_WasPresent_GO))
        {
            return false;
        }

        if (!m_Monster_Tips_WasPresent_GO.TryFindComponent<Text>(TransformPaths.Monster_Tips_WasPresent_Text, out m_Monster_Tips_WasPresent_Text))
        {
            return false;
        }

        if (!m_Monster_Tips_WasPresent_GO.TryFindGameObject(TransformPaths.Monster_Tips_WasPresent_Checked, out m_Monster_Tips_WasPresent_Checked))
        {
            return false;
        }

        if (!m_Status_Monster_GO.TryFindGameObject(TransformPaths.Monster_Tips_HelpingFarm, out m_Monster_Tips_HelpingFarm_GO))
        {
            return false;
        }

        if (!m_Monster_Tips_HelpingFarm_GO.TryFindComponent<Text>(TransformPaths.Monster_Tips_HelpingFarm_Text_Title, out m_Monster_Tips_HelpingFarm_Text_Title))
        {
            return false;
        }

        if (!m_Monster_Tips_HelpingFarm_GO.TryFindComponent<Text>(TransformPaths.Monster_Tips_HelpingFarm_Text_Place, out m_Monster_Tips_HelpingFarm_Text_Place))
        {
            return false;
        }

        return true;
    }

    private string npc_Tips_BirthdayLeft_Text_Format = string.Empty;
    private bool Init()
    {
        if (!PreloadPaths())
        {
            return false;
        }

        m_NPC_Tips_TodayTalked_Text.text = LocalizationManager.Load("npc.tips.title.talk_today");
        m_NPC_Tips_WasPresent_Text.text = LocalizationManager.Load("npc.tips.title.was_present");
        npc_Tips_BirthdayLeft_Text_Format = LocalizationManager.Load("npc.tips.title.birthday_left");
        m_NPC_Tips_Alert_BirthdayToday_Text.text = LocalizationManager.Load("npc.tips.title.birthday_today");

        m_Monster_Tips_BrushingToday_Text.text = LocalizationManager.Load("monster.tips.title.brushing_today");
        m_Monster_Tips_WasPresent_Text.text = LocalizationManager.Load("monster.tips.title.was_present");
        m_Monster_Tips_HelpingFarm_Text_Title.text = LocalizationManager.Load("monster.tips.title.helping_farm");

        return true;
    }

    internal static bool InstantiateAndAttach(FriendPageStatusDisp friendPageStatusDisp)
    {
        if (Instance is not null)
        {
            BepInExLog.LogDebug("[Attachment_LeftStatusPos] InstantiateAndAttach: instance already exist");
            return true;
        }

        var attachTarget = friendPageStatusDisp.transform.Find(AttachPathBasedFriendPageStatusDisp);
        if (attachTarget is null)
        {
            BepInExLog.LogError("[Attachment_LeftStatusPos] InstantiateAndAttach: Cannot find attachTarget");
            return false;
        }

        var prefab = BundleLoader.MainBundle.LoadIL2CPP<GameObject>(PrefabPathFromBundle);
        if (prefab is null)
        {
            BepInExLog.LogError("[Attachment_LeftStatusPos] InstantiateAndAttach: Cannot load prefab");
            return false;
        }

        var InstanceGO = GameObject.Instantiate(prefab, attachTarget.transform);
        if (InstanceGO is null)
        {
            BepInExLog.LogError("[Attachment_LeftStatusPos] InstantiateAndAttach: Cannot instantiate window");
            return false;
        }

        RF5FontHelper.SetFontGlobal(InstanceGO);

        Instance = InstanceGO.AddComponent<Attachment_LeftStatusPos>();
        if (!Instance.Init())
        {
            BepInExLog.LogError("[Attachment_LeftStatusPos] InstantiateAndAttach: PreloadPathes failed");
            Instance = null;
            Destroy(InstanceGO);
            return false;
        }

        BepInExLog.LogInfo("[Attachment_LeftStatusPos] Attached");
        return true;
    }

    internal void SetShown(bool isShown)
    {
        gameObject.SetActive(isShown);
    }

    private bool GetShown()
    {
        return gameObject.activeSelf;
    }

    private void ResetShown()
    {
        m_Status_NPC_GO?.SetActive(false);
        m_Status_Monster_GO?.SetActive(false);

        m_NPC_Tips_TodayTalked_GO?.SetActive(false);
        m_NPC_Tips_TodayTalked_Checked?.SetActive(false);
        m_NPC_Tips_WasPresent_GO?.SetActive(false);
        m_NPC_Tips_BirthdayLeft_GO?.SetActive(false);
        m_NPC_Tips_Alert_BirthdayToday_GO?.SetActive(false);
        m_NPC_Tips_WasPresent_CheckedNormal?.SetActive(false);
        m_NPC_Tips_WasPresent_CheckedLikes?.SetActive(false);
        m_NPC_Tips_WasPresent_CheckedLoves?.SetActive(false);

        m_Monster_Tips_BrushingToday_GO?.SetActive(false);
        m_Monster_Tips_BrushingToday_Checked?.SetActive(false);
        m_Monster_Tips_WasPresent_GO?.SetActive(false);
        m_Monster_Tips_WasPresent_Checked?.SetActive(false);
        m_Monster_Tips_HelpingFarm_GO?.SetActive(false);
    }

    internal void SetNPCData(NpcData npcData)
    {
        ResetShown();

        if ((!ShowBirthdayTip.Value && !ShowTalkedTip.Value && !ShowNPCGiftDeliveredTip.Value) || npcData is null)
        {
            return;
        }

        m_Status_NPC_GO.SetActive(true);

        if (ShowTalkedTip.Value)
        {
            m_NPC_Tips_TodayTalked_GO.SetActive(true);
            m_NPC_Tips_TodayTalked_Checked.SetActive(npcData.TodayTalkCount > 0);
        }

        if (ShowNPCGiftDeliveredTip.Value)
        {
            m_NPC_Tips_WasPresent_GO.SetActive(true);

            var presentItemTypesArray = npcData.PresentItemTypes.ToArray();
            m_NPC_Tips_WasPresent_CheckedNormal.SetActive(presentItemTypesArray.Any(x => x == LovePointManager.FavoriteType.Normal));
            m_NPC_Tips_WasPresent_CheckedLikes.SetActive(presentItemTypesArray.Any(x => x == LovePointManager.FavoriteType.Favorite));
            m_NPC_Tips_WasPresent_CheckedLoves.SetActive(presentItemTypesArray.Any(x => x == LovePointManager.FavoriteType.VeryFavorite));
        }

        if (ShowBirthdayTip.Value)
        {
            if (NpcDataManager.Instance.LovePointManager.IsBirthDay(npcData.NpcId))
            {
                m_NPC_Tips_Alert_BirthdayToday_GO.SetActive(true);
                return;
            }

            if (npcData.TryFindNPCBirthday(out Define.Season birthday_season, out int birthday_day))
            {
                m_NPC_Tips_BirthdayLeft_GO.SetActive(true);
                var local_Today = ((int)TimeManager.Instance.Season * RF5CalendarConstants.DaysInMonth) + TimeManager.Instance.Day;
                var local_Birthday = ((int)birthday_season * RF5CalendarConstants.DaysInMonth) + birthday_day;
                int leftDay = 0;
                if (local_Today > local_Birthday)
                {
                    //Birthday has already passed
                    leftDay = RF5CalendarConstants.DaysInYear;
                }

                leftDay += local_Birthday - local_Today;

                m_NPC_Tips_BirthdayLeft_Text.text = string.Format(npc_Tips_BirthdayLeft_Text_Format, leftDay);
            }
            else
            {
                BepInExLog.LogError($"[Attachment_LeftStatusPos] Cannot find {npcData.actorId}'s birthday");
            }
        }
    }

    internal void SetMonsterData(FriendMonsterStatusData friendMonsterData)
    {
        ResetShown();

        if ((!ShowMonsterGiftDeliveredTip.Value && !ShowMonsterHelpingTip.Value && !ShowMonsterBrushedTip.Value) || friendMonsterData is null)
        {
            return;
        }

        m_Status_Monster_GO.SetActive(true);

        m_Monster_Tips_BrushingToday_GO.SetActive(ShowMonsterBrushedTip.Value);
        m_Monster_Tips_BrushingToday_Checked.SetActive(friendMonsterData.IsBrushed);
        m_Monster_Tips_WasPresent_GO.SetActive(ShowMonsterGiftDeliveredTip.Value);
        m_Monster_Tips_WasPresent_Checked.SetActive(friendMonsterData.IsPresent);

        if (ShowMonsterHelpingTip.Value && friendMonsterData.FarmFieldWorkArea != Define.FarmFieldWorkArea.None)
        {
            m_Monster_Tips_HelpingFarm_GO.SetActive(true);
            string farmName = RF5DataExtension.GetFarmName(friendMonsterData.FarmId);
            string workAreaStr = friendMonsterData.FarmFieldWorkArea switch
            {
                Define.FarmFieldWorkArea.Left => LocalizationManager.Load("monster.tips.workarea.left"),
                Define.FarmFieldWorkArea.Center => LocalizationManager.Load("monster.tips.workarea.center"),
                Define.FarmFieldWorkArea.Right => LocalizationManager.Load("monster.tips.workarea.right"),
                _ => string.Empty,
            };

            m_Monster_Tips_HelpingFarm_Text_Place.text = $"{farmName} ({workAreaStr})";
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            BepInExLog.LogDebug("[Attachment_LeftStatusPos] Destroyed");
            Instance = null;
        }
    }
}
