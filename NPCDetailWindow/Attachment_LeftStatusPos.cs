using UnityEngine;
using UnityEngine.UI;
using RF5.HisaCat.NPCDetails.Utils;
using RF5.HisaCat.NPCDetails.Localization;

namespace RF5.HisaCat.NPCDetails.NPCDetailWindow;

/// <summary>
/// Attachments for LeftStatusPos
/// </summary>
internal class Attachment_LeftStatusPos : MonoBehaviour
{
    internal static Attachment_LeftStatusPos? Instance { get; private set; }

    internal const string PrefabPathFromBundle = "[RF5.HisaCat.NPCDetails]LeftStatusPos";

    internal const string AttachPathBasedFriendPageStatusDisp = "StatusObj/FriendsStatus/LeftStatusPos";
    private const int DaysInMonth = 30;

    internal static class TransformPaths
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


    internal GameObject m_Status_NPC_GO = null;
    internal GameObject m_Status_Monster_GO = null;

    internal GameObject m_NPC_Tips_TodayTalked_GO = null;
    internal Text m_NPC_Tips_TodayTalked_Text = null;
    internal GameObject m_NPC_Tips_TodayTalked_Checked = null;

    internal GameObject m_NPC_Tips_WasPresent_GO = null;
    internal Text m_NPC_Tips_WasPresent_Text = null;
    internal GameObject m_NPC_Tips_WasPresent_CheckedNormal;
    internal GameObject m_NPC_Tips_WasPresent_CheckedLikes;
    internal GameObject m_NPC_Tips_WasPresent_CheckedLoves;

    internal GameObject m_NPC_Tips_BirthdayLeft_GO = null;
    internal Text m_NPC_Tips_BirthdayLeft_Text = null;

    internal GameObject m_NPC_Tips_Alert_BirthdayToday_GO = null;
    internal Text m_NPC_Tips_Alert_BirthdayToday_Text = null;

    internal GameObject m_Monster_Tips_BrushingToday_GO = null;
    internal Text m_Monster_Tips_BrushingToday_Text = null;
    internal GameObject m_Monster_Tips_BrushingToday_Checked = null;

    internal GameObject m_Monster_Tips_WasPresent_GO = null;
    internal Text m_Monster_Tips_WasPresent_Text = null;
    internal GameObject m_Monster_Tips_WasPresent_Checked = null;

    internal GameObject m_Monster_Tips_HelpingFarm_GO = null;
    internal Text m_Monster_Tips_HelpingFarm_Text_Title = null;
    internal Text m_Monster_Tips_HelpingFarm_Text_Place = null;

    internal bool PreloadPathes()
    {
        {
            GameObject root;
            if (!this.TryFindGameObject(TransformPaths.Status_NPC, out root))
            {
                return false;
            }

            m_Status_NPC_GO = root;
            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.NPC_Tips_TodayTalked, out parent))
                {
                    return false;
                }

                m_NPC_Tips_TodayTalked_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.NPC_Tips_TodayTalked_Text, out m_NPC_Tips_TodayTalked_Text))
                {
                    return false;
                }

                if (!parent.TryFindGameObject(TransformPaths.NPC_Tips_TodayTalked_Checked, out m_NPC_Tips_TodayTalked_Checked))
                {
                    return false;
                }
            }

            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent, out parent))
                {
                    return false;
                }

                m_NPC_Tips_WasPresent_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.NPC_Tips_WasPresent_Text, out m_NPC_Tips_WasPresent_Text))
                {
                    return false;
                }

                if (!parent.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent_CheckedNormal, out m_NPC_Tips_WasPresent_CheckedNormal))
                {
                    return false;
                }

                if (!parent.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent_CheckedLikes, out m_NPC_Tips_WasPresent_CheckedLikes))
                {
                    return false;
                }

                if (!parent.TryFindGameObject(TransformPaths.NPC_Tips_WasPresent_CheckedLoves, out m_NPC_Tips_WasPresent_CheckedLoves))
                {
                    return false;
                }
            }

            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.NPC_Tips_BirthdayLeft, out parent))
                {
                    return false;
                }

                m_NPC_Tips_BirthdayLeft_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.NPC_Tips_BirthdayLeft_Text, out m_NPC_Tips_BirthdayLeft_Text))
                {
                    return false;
                }
            }
            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.NPC_Tips_Alert_BirthdayToday, out parent))
                {
                    return false;
                }

                m_NPC_Tips_Alert_BirthdayToday_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.NPC_Tips_Alert_BirthdayToday_Text, out m_NPC_Tips_Alert_BirthdayToday_Text))
                {
                    return false;
                }
            }
        }
        {
            GameObject root;
            if (!this.TryFindGameObject(TransformPaths.Status_Monster, out root))
            {
                return false;
            }

            m_Status_Monster_GO = root;

            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.Monster_Tips_BrushingToday, out parent))
                {
                    return false;
                }

                m_Monster_Tips_BrushingToday_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.Monster_Tips_BrushingToday_Text, out m_Monster_Tips_BrushingToday_Text))
                {
                    return false;
                }

                if (!parent.TryFindGameObject(TransformPaths.Monster_Tips_BrushingToday_Checked, out m_Monster_Tips_BrushingToday_Checked))
                {
                    return false;
                }
            }
            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.Monster_Tips_WasPresent, out parent))
                {
                    return false;
                }

                m_Monster_Tips_WasPresent_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.Monster_Tips_WasPresent_Text, out m_Monster_Tips_WasPresent_Text))
                {
                    return false;
                }

                if (!parent.TryFindGameObject(TransformPaths.Monster_Tips_WasPresent_Checked, out m_Monster_Tips_WasPresent_Checked))
                {
                    return false;
                }
            }
            {
                GameObject parent;
                if (!root.TryFindGameObject(TransformPaths.Monster_Tips_HelpingFarm, out parent))
                {
                    return false;
                }

                m_Monster_Tips_HelpingFarm_GO = parent;
                if (!parent.TryFindComponent<Text>(TransformPaths.Monster_Tips_HelpingFarm_Text_Title, out m_Monster_Tips_HelpingFarm_Text_Title))
                {
                    return false;
                }

                if (!parent.TryFindComponent<Text>(TransformPaths.Monster_Tips_HelpingFarm_Text_Place, out m_Monster_Tips_HelpingFarm_Text_Place))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private FriendPageStatusDisp friendPageStatusDisp = null;
    private string npc_Tips_BirthdayLeft_Text_Format = string.Empty;
    internal bool Init(FriendPageStatusDisp friendPageStatusDisp)
    {
        this.friendPageStatusDisp = friendPageStatusDisp;
        if (!PreloadPathes())
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
        if (!Instance.Init(friendPageStatusDisp))
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

    internal bool GetShown()
    {
        return gameObject.activeSelf;
    }

    internal void SetNPCData(NpcData npcData)
    {
        m_Status_NPC_GO.SetActive(true);
        m_Status_Monster_GO.SetActive(false);

        m_NPC_Tips_TodayTalked_GO.SetActive(false);
        m_NPC_Tips_WasPresent_GO.SetActive(false);
        m_NPC_Tips_BirthdayLeft_GO.SetActive(false);
        m_NPC_Tips_Alert_BirthdayToday_GO.SetActive(false);
        {
            m_NPC_Tips_TodayTalked_GO.SetActive(true);

            bool wasTodayTalked = npcData.TodayTalkCount > 0;
            m_NPC_Tips_TodayTalked_Checked.SetActive(wasTodayTalked);
        }
        {
            m_NPC_Tips_WasPresent_GO.SetActive(true);

            var presentItemTypesArray = npcData.PresentItemTypes.ToArray();
            bool wasPresentNormal = presentItemTypesArray.Any(x => x == LovePointManager.FavoriteType.Normal);
            bool wasPresentFavorite = presentItemTypesArray.Any(x => x == LovePointManager.FavoriteType.Favorite);
            bool wasPresentVeryFavorite = presentItemTypesArray.Any(x => x == LovePointManager.FavoriteType.VeryFavorite);

            m_NPC_Tips_WasPresent_CheckedNormal.SetActive(wasPresentNormal);
            m_NPC_Tips_WasPresent_CheckedLikes.SetActive(wasPresentFavorite);
            m_NPC_Tips_WasPresent_CheckedLoves.SetActive(wasPresentVeryFavorite);
        }

        var isBirthday = NpcDataManager.Instance.LovePointManager.IsBirthDay(npcData.NpcId);
        {
            if (isBirthday)
            {
                m_NPC_Tips_BirthdayLeft_GO.SetActive(false);
            }
            else
            {
                m_NPC_Tips_BirthdayLeft_GO.SetActive(true);

                Define.Season birthday_season;
                int birthday_day;
                if (npcData.TryFindNPCBirthday(out birthday_season, out birthday_day))
                {
                    var local_Today = ((int)TimeManager.Instance.Season * DaysInMonth) + TimeManager.Instance.Day;
                    var local_Birthday = ((int)birthday_season * DaysInMonth) + birthday_day;
                    int leftDay = 0;
                    if (local_Today < local_Birthday) //Birthday not spend yet
                    {
                        leftDay = local_Birthday - local_Today;
                    }
                    else
                    {
                        leftDay = (4 * DaysInMonth) - local_Today + local_Birthday;
                    }

                    m_NPC_Tips_BirthdayLeft_Text.text = string.Format(npc_Tips_BirthdayLeft_Text_Format, leftDay);
                }
                else
                {
                    m_NPC_Tips_BirthdayLeft_GO.SetActive(false);
                    BepInExLog.LogError($"[Attachment_LeftStatusPos] Cannot find {npcData.actorId}'s birthday");
                }
            }
        }
        {
            m_NPC_Tips_Alert_BirthdayToday_GO.SetActive(isBirthday);
        }
    }

    internal void SetMonsterData(FriendMonsterStatusData friendMonsterData, MonsterDataTable monsterData)
    {
        m_Status_NPC_GO.SetActive(false);
        m_Status_Monster_GO.SetActive(true);

        m_Monster_Tips_BrushingToday_Checked.SetActive(friendMonsterData.IsBrushed);
        m_Monster_Tips_WasPresent_Checked.SetActive(friendMonsterData.IsPresent);
        //bool wasGetFood = friendMonsterData.EsaGet;

        bool isHelpingFarm = friendMonsterData.FarmFieldWorkArea != Define.FarmFieldWorkArea.None;
        m_Monster_Tips_HelpingFarm_GO.SetActive(isHelpingFarm);
        if (isHelpingFarm)
        {
            string farmName = RF5DataExtension.GetFarmName(friendMonsterData.FarmId);
            string workAreaStr = string.Empty;
            switch (friendMonsterData.FarmFieldWorkArea)
            {
                case Define.FarmFieldWorkArea.Left:
                    workAreaStr = LocalizationManager.Load("monster.tips.workarea.left");
                    break;
                case Define.FarmFieldWorkArea.Center:
                    workAreaStr = LocalizationManager.Load("monster.tips.workarea.center");
                    break;
                case Define.FarmFieldWorkArea.Right:
                    workAreaStr = LocalizationManager.Load("monster.tips.workarea.right");
                    break;
            }

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
