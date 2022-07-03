using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Tools;
using MoonSharp.Interpreter;

namespace MapleServer2.Types;

public class Gemstone
{
    public int Id;
    public long OwnerId = 0;
    public string OwnerName = "";
    public bool IsLocked;
    public long UnlockTime;
    public ItemStats Stats;
    public ItemAdditionalEffectMetadata AdditionalEffects;
}

public class GemSocket
{
    public bool IsUnlocked;
    public Gemstone Gemstone;
}

public class GemSockets
{
    public List<GemSocket> Sockets;

    public byte Count { get => (byte) Sockets.Count; }

    public GemSockets()
    {
        Sockets = new();
    }

    public GemSockets(Item parent)
    {
        Sockets = new();

        CreateSockets(parent);
        OpenSockets();
    }

    public GemSocket this[int i] => Sockets[i];

    public void CreateSockets(Item parent)
    {

        if (!ItemMetadataStorage.IsValid(parent.Id))
        {
            return;
        }

        float optionLevelFactor = ItemMetadataStorage.GetOptionMetadata(parent.Id).OptionLevelFactor;

        // Check for predefined sockets
        int socketId = ItemMetadataStorage.GetPropertyMetadata(parent.Id).SocketDataId;
        if (socketId != 0)
        {
            ItemSocketRarityData socketData = ItemSocketMetadataStorage.GetMetadata(socketId, parent.Rarity);
            if (socketData is not null)
            {
                while (Sockets.Count > socketData.MaxCount)
                {
                    Sockets.RemoveAt(Sockets.Count - 1);
                }

                for (int i = Sockets.Count; i < socketData.MaxCount; i++)
                {
                    Sockets.Add(new());
                }

                for (int j = 0; j < socketData.FixedOpenCount; j++)
                {
                    Sockets[j].IsUnlocked = true;
                }
                return;
            }
        }

        if (parent.Type == ItemType.None)
        {
            parent.Type = parent.GetItemType();
        }

        Script script = ScriptLoader.GetScript("Functions/calcItemSocketMaxCount");
        DynValue dynValue = script.RunFunction("calcItemSocketMaxCount", (int) parent.Type, parent.Rarity, optionLevelFactor, (int) parent.InventoryTab);
        int slotAmount = (int) dynValue.Number;
        if (slotAmount <= 0)
        {
            return;
        }

        while (Sockets.Count > slotAmount)
        {
            Sockets.RemoveAt(Sockets.Count - 1);
        }

        // add sockets
        for (int i = Sockets.Count; i < slotAmount; i++)
        {
            GemSocket socket = new();
            Sockets.Add(socket);
        }
    }

    public void OpenSockets()
    {
        // roll to unlock sockets
        for (int i = 0; i < Sockets.Count; i++)
        {
            int successNumber = Random.Shared.Next(0, 100);

            // 5% success rate to unlock a gemsocket
            if (successNumber < 95)
            {
                break;
            }

            Sockets[i].IsUnlocked = true;
        }

        return;
    }
}
