namespace RF5.HisaCat.NPCDetails.NPCDetailWindow;

internal class NPCDetailWindowManager
{
    private static NPCDetailWindowManager Instance;
    internal NPCDetailWindowManager()
    {
        Instance = this;
    }

    private static UIOnOffAnimate? equipMenuItemDetail = null;
    internal static void TryAttachIfNotExist(FriendPageStatusDisp friendPageStatusDisp)
    {
        if (Attachment_LeftStatusPos.Instance is null)
        {
            Attachment_LeftStatusPos.InstantiateAndAttach(friendPageStatusDisp);
        }

        if (Attachment_RightStatusPos.Instance is null)
        {
            Attachment_RightStatusPos.InstantiateAndAttach(friendPageStatusDisp);
        }
    }

    internal static void TrySetNPCData(NpcData npcData)
    {
        Attachment_LeftStatusPos.Instance?.SetNPCData(npcData);
        Attachment_RightStatusPos.Instance?.SetNPCData(npcData);
    }

    internal static void TrySetMonsterData(FriendMonsterStatusData friendMonsterData, MonsterDataTable monsterData)
    {
        Attachment_LeftStatusPos.Instance?.SetMonsterData(friendMonsterData, monsterData);
        Attachment_RightStatusPos.Instance?.SetMonsterData(friendMonsterData, monsterData);
    }

    internal static void TrySetShown(bool isShown)
    {
        Attachment_LeftStatusPos.Instance?.SetShown(isShown);
        Attachment_RightStatusPos.Instance?.SetShown(isShown);
    }
}
