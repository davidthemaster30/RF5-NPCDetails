using RF5.HisaCat.NPCDetails.NPCDetailWindow;

namespace RF5.HisaCat.NPCDetails.Utils;

internal static class RF5DataExtension
{
    internal static string GetItemName(this ItemDataTable dataTable)
    {
        return SV.UIRes.GetText(SysTextGroup.ItemUINameData, dataTable.ItemIndex);
    }

    internal static string GetNpcName(this NpcData data)
    {
        return SV.UIRes.GetText(SysTextGroup.NPCNameData, data.NpcId);
    }

    internal static string GetNpcDiscript(this NpcData data)
    {
        return SV.UIRes.GetText(SysTextGroup.NPCDiscriptData, data.NpcId);
    }

    internal static List<ItemDataTable> ItemIdArrayToItemDataTables(IEnumerable<ItemID> itemIds)
    {
        return ItemIdArrayToItemDataTables(itemIds.Select(x => (int)x));
    }

    internal static List<ItemDataTable> ItemIdArrayToItemDataTables(IEnumerable<int> itemIds)
    {
        List<ItemDataTable> items = [];
        if (itemIds is null)
        {
            return items;
        }

        foreach (var itemIdInt in itemIds)
        {
            var itemData = ItemDataTable.GetDataTable((ItemID)itemIdInt);
            if (itemData is null)
            {
                continue;
            }

            items.Add(itemData);
        }
        return items;
    }

    internal static IEnumerable<ItemDataTable> RemoveTrashItems(IEnumerable<ItemDataTable> items)
    {
        return items.Where(x => x.ItemType != ItemType.Trash);
    }

    internal static List<ItemDataTable> GetVeryFavoriteItemDataTables(this NpcData npcData)
    {
        //Loves
        return npcData?.statusData is null ? [] : ItemIdArrayToItemDataTables(npcData.statusData.VeryFavoriteItem);
    }

    internal static List<ItemDataTable> GetFavoriteItemDataTables(this MonsterDataTable monsterData)
    {
        return monsterData?.FavoriteItemData is null ? [] : ItemIdArrayToItemDataTables(monsterData.FavoriteItemData.ItemIDArray);
    }

    internal static List<ItemDataTable> GetFavoriteItemDataTables(this NpcData npcData)
    {
        //Likes
        return npcData?.statusData is null ? [] : ItemIdArrayToItemDataTables(npcData.statusData.FavoriteItem);
    }

    internal static List<ItemDataTable> GetNotFavoriteItemDataTables(this NpcData npcData, bool exceptAlmostHates = false)
    {
        //Dislikes
        if (npcData?.statusData is null)
        {
            return [];
        }

        if (!exceptAlmostHates)
        {
            return ItemIdArrayToItemDataTables(npcData.statusData.NotFavoriteItem);
        }

        return ItemIdArrayToItemDataTables(npcData.statusData.NotFavoriteItem).Where(x =>
        {
            switch (ItemDataTable.GetItemID(x.ItemIndex))
            {
                case ItemID.Item_Buttaix: //물체 X(2011)
                case ItemID.Item_Kuzutetsu: //고철(2151)
                case ItemID.Item_Zasso: //잡초(195)
                case ItemID.Item_Karekusa: //마른 풀(196)
                case ItemID.Item_Shippaisaku: //실패작(200)
                case ItemID.Item_Choshippaisaku: //완전 실패작(201)
                case ItemID.Item_Ishi: //돌(1500)
                case ItemID.Item_Eda: //나뭇가지(1501)
                case ItemID.Item_Akikan: //빈 캔(1900)
                case ItemID.Item_Nagagutsu: //장화(1901)
                case ItemID.Item_Reanaakikan: //희귀한 빈 캔(1902)
                    return false;
                default:
                    return true;
            }
        }).ToList();
    }

    internal static List<ItemDataTable> GetNotFavoriteBadlyItemDataTables(this NpcData npcData)
    {
        //Hates
        return npcData?.statusData is null ? [] : ItemIdArrayToItemDataTables(npcData.statusData.NotFavoriteBadlyItem);
    }

    internal static bool TryFindNPCBirthday(this NpcData npcData, out Define.Season season, out int day)
    {
        return TryFindNPCBirthday(npcData.NpcId, out season, out day);
    }

    internal static bool TryFindNPCBirthday(int npcID, out Define.Season season, out int day)
    {
        for (season = Define.Season.Spring; season <= Define.Season.Winter; season++)
        {
            for (day = 1; day <= RF5CalendarConstants.DaysInMonth; day++)
            {
                if (NpcDataManager.Instance.LovePointManager.IsBirthDayByDate(npcID, season, day))
                {
                    return true;
                }
            }
        }

        BepInExLog.LogError($"Could not find NPC's {npcID} BirthDay!");
        season = Define.Season.None;
        day = -1;
        return false;
    }

    internal static FriendMonsterStatusData? GetFriendMonsterDataFromIndex(int monsterIdx, MonsterDataTable monsterData)
    {
        if (monsterIdx < 0 || monsterIdx >= FriendMonsterManager.FriendMonsterStatusDatas.Count)
        {
            BepInExLog.LogError($"Cannot find FriendMonsterStatusDatas because monsterIdx is invalid! monsterIdx: {monsterIdx}, statusDatasCount: {FriendMonsterManager.FriendMonsterStatusDatas.Count}");
            return null;
        }

        var data = FriendMonsterManager.FriendMonsterStatusDatas[monsterIdx];
        if (data is null)
        {
            BepInExLog.LogError($"Cannot find friend monster from statusId {monsterIdx}");
            return null;
        }

        if (data.MonsterDataID != monsterData.DataID)
        {
            BepInExLog.LogError($"Friend monster found at {monsterIdx} but dataId missmatched! found: {data.MonsterDataID} req: {monsterData.DataID}");
            return null;
        }

        return data;
    }

    internal static string GetFarmName(Farm.FarmManager.FARM_ID farmId) => farmId switch
    {
        Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Soil => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_01_soil),
        Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Fire => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_02_fire),
        Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Ice => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_03_ice),
        Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Wind => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_04_wind),
        Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Ground => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_05_ground),
        //Unknowns
        Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Village => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_FIELD_Town),
        _ => farmId.ToString(),
    };

    internal static string GetMonsterName(MonsterID monsterId)
    {
        return SV.UIRes.MonsterName(monsterId);
    }

    internal static string GetLocalizedPlaceName(Define.Place place) => place switch
    {
        Define.Place.Police => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_01_Lv1_police),
        Define.Place.Hospital => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_02_Hospital),
        Define.Place.BlackSmith => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_03_Blacksmith),
        Define.Place.Shop => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_04_Shop),
        Define.Place.Restaurant => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_05_Restaurant_2),
        Define.Place.Hotel => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_06_Hotel),
        Define.Place.Bakery => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_07_Bakery),
        Define.Place.Carpenter => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_08_Carpenter),
        Define.Place.Detective => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_09_Detective),
        Define.Place.CrystalShop => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_10_CrystalShop),
        Define.Place.Monsterhut => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_11_Monsterhut),
        Define.Place.FlowerShop => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_13_FlowerShop),
        Define.Place.Relics => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_14_Relics),
        Define.Place.Home1 => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_17_home01),
        Define.Place.Home2 => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_18_home02),
        Define.Place.Home3 => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_19_home03),
        Define.Place.Home4 => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_20_home04),

        #region Unknowns
        Define.Place.FreeHome => SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_21_home_empty),
        Define.Place.Beach => SV.UIRes.GetSystemText(UITextDic.DICID.NIGHT_BEACH),
        var x when x == Define.Place.Lake ||
                    x == Define.Place.Dungeon1 ||
                    x == Define.Place.BigTreePlaza ||
                    x == Define.Place.Bridge ||
                    x == Define.Place.Field => $"{SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_FIELD_Town)} ({place})",
        _ => place.ToString()
        #endregion Unknowns
    };
}

