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
        var items = new List<ItemDataTable>();
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

    internal static List<ItemDataTable> RemoveTrashItems(List<ItemDataTable> items)
    {
        return items.Where(x => x.ItemType != ItemType.Trash).ToList();
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

        if (exceptAlmostHates)
        {
            return ItemIdArrayToItemDataTables(npcData.statusData.NotFavoriteItem).Where(x =>
            {
                var itemID = ItemDataTable.GetItemID(x.ItemIndex);
                switch (itemID)
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
        else
        {
            return ItemIdArrayToItemDataTables(npcData.statusData.NotFavoriteItem);
        }
    }

    internal static List<ItemDataTable> GetNotFavoriteBadlyItemDataTables(this NpcData npcData)
    {
        //Hates
        if (npcData?.statusData is null)
        {
            return new List<ItemDataTable>();
        }

        return ItemIdArrayToItemDataTables(npcData.statusData.NotFavoriteBadlyItem);
    }

    internal static bool TryFindNPCBirthday(this NpcData npcData, out Define.Season season, out int day)
    {
        return TryFindNPCBirthday(npcData.NpcId, out season, out day);
    }

    internal static bool TryFindNPCBirthday(int npcID, out Define.Season season, out int day)
    {
        for (season = Define.Season.Spring; season <= Define.Season.Winter; season++)
        {
            for (day = 1; day <= 30; day++)
            {
                if (NpcDataManager.Instance.LovePointManager.IsBirthDayByDate(npcID, season, day))
                {
                    return true;
                }
            }
        }

        season = Define.Season.None;
        day = -1;
        return false;
    }

    internal static FriendMonsterStatusData? GetFriendMonsterDataFromIndex(int monsterIdx, MonsterDataTable monsterData)
    {
        if (monsterIdx < 0 || monsterIdx >= FriendMonsterManager.FriendMonsterStatusDatas.Count)
        {
            BepInExLog.LogError($"Cannot find FriendMonsterStatusDatas cause monsterIdx Invalid! monsterIdx: {monsterIdx}, statusDatasCount: {FriendMonsterManager.FriendMonsterStatusDatas.Count}");
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
            BepInExLog.LogError($"Friend monster finded at {monsterIdx} but dataId missmatched! finded: {data.MonsterDataID} req: {monsterData.DataID}");
            return null;
        }

        return data;
    }

    internal static string GetFarmName(Farm.FarmManager.FARM_ID farmId)
    {
        switch (farmId)
        {
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Soil:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_01_soil);
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Fire:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_02_fire);
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Ice:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_03_ice);
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Wind:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_04_wind);
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Ground:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_FarmDragon_05_ground);
            //Unknowns
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Village:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_FIELD_Town);
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_Vision:
            case Farm.FarmManager.FARM_ID.RF4_FREEFARM_ID_MAX:
            default:
                return farmId.ToString();
        }
    }

    internal static string GetMonsterName(MonsterID monsterId)
    {
        return SV.UIRes.MonsterName(monsterId);
    }

    internal static string GetLocalizedPlaceName(Define.Place place)
    {
        switch (place)
        {
            case Define.Place.Police:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_01_Lv1_police);
            case Define.Place.Hospital:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_02_Hospital);
            case Define.Place.BlackSmith:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_03_Blacksmith);
            case Define.Place.Shop:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_04_Shop);
            case Define.Place.Restaurant:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_05_Restaurant_2);
            case Define.Place.Hotel:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_06_Hotel);
            case Define.Place.Bakery:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_07_Bakery);
            case Define.Place.Carpenter:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_08_Carpenter);
            case Define.Place.Detective:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_09_Detective);
            case Define.Place.CrystalShop:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_10_CrystalShop);
            case Define.Place.Monsterhut:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_11_Monsterhut);
            case Define.Place.FlowerShop:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_13_FlowerShop);
            case Define.Place.Relics:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_14_Relics);
            case Define.Place.Home1:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_17_home01);
            case Define.Place.Home2:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_18_home02);
            case Define.Place.Home3:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_19_home03);
            case Define.Place.Home4:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_20_home04);

            #region Unknowns
            case Define.Place.FreeHome:
                return SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_SCENE_RF5Room_21_home_empty);
            case Define.Place.Beach:
                return SV.UIRes.GetSystemText(UITextDic.DICID.NIGHT_BEACH);
            case Define.Place.Lake:
            case Define.Place.Dungeon1:
            case Define.Place.BigTreePlaza:
            case Define.Place.Bridge:
            case Define.Place.Field:
                return $"{SV.UIRes.GetSystemText(UITextDic.DICID.MAPNAME_FIELD_Town)} ({place.ToString()})";
            case Define.Place.None:
            default:
                return $"({place.ToString()})";
                #endregion Unknowns
        }
    }
}

