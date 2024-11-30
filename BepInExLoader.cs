using BepInEx;
using HarmonyLib;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using RF5.HisaCat.NPCDetails.Utils;
using BepInEx.Configuration;
using RF5.HisaCat.NPCDetails.NPCDetailWindow;
using AsmResolver.PE.DotNet.Cil;

namespace RF5.HisaCat.NPCDetails;

[BepInPlugin(GUID, MODNAME, VERSION)]
public class BepInExLoader : BasePlugin
{
    internal const string
        MODNAME = "NPCDetails",
        AUTHOR = "HisaCat",
        GUID = "RF5." + AUTHOR + "." + MODNAME,
        VERSION = "1.4.0";

    internal static string GetPluginRootDirectory()
    {
        return Path.GetDirectoryName(IL2CPPChainloader.Instance.Plugins[GUID].Location);
    }

    internal void LoadConfigs(){
        
        BepInExLog.EnableLogging = Config.Bind("Logging", nameof(BepInExLog.EnableLogging), true, "Set to true to enable logging of changed values by the mod.");
        Attachment_LeftStatusPos.LoadConfig(Config);
        Attachment_RightStatusPos.LoadConfig(Config);
    }

    public override void Load()
    {
        //Config
        LoadConfigs();

        try
        {
            //Register components
            ClassInjector.RegisterTypeInIl2Cpp<Attachment_LeftStatusPos>();
            ClassInjector.RegisterTypeInIl2Cpp<Attachment_RightStatusPos>();
        }
        catch (Exception e)
        {
            BepInExLog.LogError($"Harmony - FAILED to Register Il2Cpp Types! {e}");
        }
        BepInExLog.LogInfo("[Harmony] RegisterTypeInIl2Cpp succeed.");

        try
        {
            //Patches
            Harmony.CreateAndPatchAll(typeof(RF5FontHelper.FontLoader));
            Harmony.CreateAndPatchAll(typeof(SVPatcher));
            Harmony.CreateAndPatchAll(typeof(UIPatcher));
        }
        catch (Exception e)
        {
            BepInExLog.LogError($"Harmony - FAILED to Apply Patch's! {e}");
        }
        BepInExLog.LogInfo("[Harmony] Patch succeed.");
    }


    [HarmonyPatch]
    internal static class SVPatcher
    {
        [HarmonyPatch(typeof(SV), nameof(SV.CreateUIRes))]
        [HarmonyPostfix]
        private static void CreateUIResPostfix(SV __instance)
        {
            //Localization.LocalizedString.CreateTemplate();
            if (BundleLoader.MainBundle is null)
            {
                BundleLoader.LoadBundle();
            }
        }
    }

    [HarmonyPatch]
    internal static class UIPatcher
    {
        [HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.Update))]
        [HarmonyPostfix]
        private static void UpdatePostfix(CampMenuMain __instance)
        {
            //FOR DEBUG
            //if (BepInEx.IL2CPP.UnityEngine.Input.GetKeyInt(BepInEx.IL2CPP.UnityEngine.KeyCode.F1) && UnityEngine.Event.current.type == UnityEngine.EventType.KeyDown)
            //{
            //    //ItemStorageManager.GetStorage(Define.StorageType.Rucksack).PushItemIn(ItemData.Instantiate(ItemID.Item_Rabunomidorinku, 1));
            //    foreach(var status in FriendMonsterManager.FriendMonsterStatusDatas)
            //    {
            //        status.LovePoint.Add(1000);
            //    }
            //    //TimeManager.Instance.ChangeTimeNextDay(12, 30);
            //    //ItemStorageManager.GetStorage(Define.StorageType.Rucksack).PushItemIn(ItemData.Instantiate(ItemID.Item_Tendon, 1));
            //    //ItemStorageManager.GetStorage(Define.StorageType.Rucksack).PushItemIn(ItemData.Instantiate(ItemID.Item_Yakionigiri, 1));
            //    //ItemStorageManager.GetStorage(Define.StorageType.Rucksack).PushItemIn(ItemData.Instantiate(ItemID.Item_Yakiimo, 1));
            //}
        }

        [HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
        [HarmonyPostfix]
        private static void StartCampPostfix(CampMenuMain __instance)
        {
            //It called when camp window is newly opened.
            //In normal case, first page equals with active page on window opened newly.
            var activePage = (CampPage)__instance.campPageSwitcher.nowPage;
            var firstPage = __instance.firstPage;
            //BepInExLog.Log($"StartCampPostfix. activePage:{activePage} firstPage: {firstPage}");
        }

        internal delegate void OnCampPageChangedDelegate(CampPage page);
        internal static OnCampPageChangedDelegate OnCampPageChanged = null;

        //[HarmonyPatch(typeof(CampPageSwitcher), nameof(CampPageSwitcher.OpenPage), new Type[] { typeof(int) })]
        [HarmonyPatch(typeof(CampPageSwitcher), nameof(CampPageSwitcher.OpenPage), new Type[] { typeof(int) })]
        [HarmonyPostfix]
        //private static void OpenPagePostfix_int(CampMenuMain __instance, int nextPage)
        private static void OpenPagePostfix_CampPage(CampMenuMain __instance, CampPage nextPage)
        {
            //It called when camp window page(tabs on tops) changed (and also window newly opened)
            //Also in this step, activePage always 'Max' (seems like dose not initialized with nextPage yet)
            //* OpenPage(int nextPage), OpenPage(CampPage nextPage) is called in same time.
            var activePage = (CampPage)__instance.campPageSwitcher.nowPage;
            //BepInExLog.Log($"OpenPagePostfix_CampPage. nextPage: {nextPage} activePage: {activePage}");

            OnCampPageChanged?.Invoke(nextPage);
        }

        [HarmonyPatch(typeof(GenerateFriendlistButton), nameof(GenerateFriendlistButton.GenerateFriendData))]
        [HarmonyPostfix]
        private static void GenerateFriendDataPostfix(GenerateFriendlistButton __instance)
        {
            //It called when the friend list page is opened, and friend tab has (Friends, Monsters) changed.
            //BepInExLog.Log($"GenerateFriendDataPostfix. friendType: {__instance.friendType}");
        }

        [HarmonyPatch(typeof(FriendPageStatusDisp), nameof(FriendPageStatusDisp.SetStatusNPC))]
        [HarmonyPostfix]
        private static void SetStatusNPCPostfix(FriendPageStatusDisp __instance, int pageId, GenerateFriendlistButton _generateFriendlistButton)
        {
            //Function called when friend detail opened and switched to another firend.
            //and '__instance.actorId' is equals with '_generateFriendlistButton.GetActorID(pageId)'
            //and 'pageId' is index of friend list button (starts with 0)
            BepInExLog.LogDebug($"SetStatusNPCPostfix. pageId: {pageId}, actorId: {__instance.actorId}");

            NPCDetailWindow.NPCDetailWindowManager.TryAttachIfNotExist(__instance);

            NPCDetailWindow.NPCDetailWindowManager.TrySetShown(true);
            var npcData = NpcDataManager.Instance.GetNpcData(__instance.actorId);
            NPCDetailWindow.NPCDetailWindowManager.TrySetNPCData(npcData);

            #region DEBUG
            BepInExLog.LogNPCData(npcData);

            //Print all npc's detials
            foreach (Define.ActorID actorId in typeof(Define.ActorID).GetEnumValues())
            {
                var actorData = NpcDataManager.Instance.GetNpcData(actorId);
                if (actorData is null)
                {
                    continue;
                }

                BepInExLog.LogNPCData(actorData);
            }
            #endregion DEBUG
        }
        [HarmonyPatch(typeof(FriendPageStatusDisp), nameof(FriendPageStatusDisp.SetStatusMonster))]
        [HarmonyPostfix]
        private static void SetStatusMonsterPostfix(FriendPageStatusDisp __instance, int pageId, GenerateFriendlistButton _generateFriendlistButton)
        {
            //Same with 'SetStatusNPCPostfix' but in monter case
            //But in this case, trust 'monsterDataID' instead 'actorId'
            //BepInExLog.Log($"SetStatusMonsterPostfix. pageId: {pageId}, monsterDataID: {__instance.monsterDataID}");
            NPCDetailWindow.NPCDetailWindowManager.TryAttachIfNotExist(__instance);

            NPCDetailWindow.NPCDetailWindowManager.TrySetShown(true);

            var monsterData = MonsterDataTable.GetDataTable(__instance.monsterDataID);
            var friendMonsterData = RF5DataExtension.GetFriendMonsterDataFromIndex(_generateFriendlistButton.GetMonsterIndex(pageId), monsterData);
            if (friendMonsterData is null)
            {
                BepInExLog.LogError("Cannot find friend monster");
                NPCDetailWindow.NPCDetailWindowManager.TrySetShown(false);
            }
            else
            {
                BepInExLog.LogInfo($"Friend monster data found. name: {friendMonsterData.Name}");
                NPCDetailWindow.NPCDetailWindowManager.TrySetMonsterData(friendMonsterData, monsterData);
            }
        }
    }
}
