using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using MagicStorage.Components;

namespace MagicStorage
{
    public static class NetHelper
    {
        private static bool queueUpdates = false;
        private static Queue<int> updateQueue = new Queue<int>();
        private static HashSet<int> updateQueueContains = new HashSet<int>();

        public static void HandlePacket(BinaryReader reader, int sender)
        {
            MessageType type = (MessageType)reader.ReadByte();
            if (type == MessageType.SearchAndRefreshNetwork)
            {
                ReceiveSearchAndRefresh(reader);
            }
            else if (type == MessageType.TryStorageOperation)
            {
                ReceiveStorageOperation(reader, sender);
            }
            else if (type == MessageType.StorageOperationResult)
            {
                ReceiveOperationResult(reader);
            }
            else if (type == MessageType.RefreshNetworkItems)
            {
                StorageGUI.RefreshItems();
            }
            else if (type == MessageType.ClientSendTEUpdate)
            {
                ReceiveClientSendTEUpdate(reader, sender);
            }
            else if (type == MessageType.TryStationOperation)
            {
                ReceiveStationOperation(reader, sender);
            }
            else if (type == MessageType.StationOperationResult)
            {
                ReceiveStationResult(reader);
            }
            else if (type == MessageType.ResetCompactStage)
            {
                ReceiveResetCompactStage(reader);
            }
            else if (type == MessageType.CraftRequest)
            {
                ReceiveCraftRequest(reader, sender);
            }
            else if (type == MessageType.CraftResult)
            {
                ReceiveCraftResult(reader);
            }
        }

        public static void SendComponentPlace(int i, int j, int type)
        {
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 2, 2);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, type);
            }
        }

        public static void StartUpdateQueue()
        {
            queueUpdates = true;
        }

        public static void SendTEUpdate(int id, Point16 position)
        {
            if (Main.netMode != 2)
            {
                return;
            }
            if (queueUpdates)
            {
                if (!updateQueueContains.Contains(id))
                {
                    updateQueue.Enqueue(id);
                    updateQueueContains.Add(id);
                }
            }
            else
            {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, id, position.X, position.Y);
            }
        }

        public static void ProcessUpdateQueue()
        {
            if (queueUpdates)
            {
                queueUpdates = false;
                while (updateQueue.Count > 0)
                {
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, updateQueue.Dequeue());
                }
                updateQueueContains.Clear();
            }
        }

        public static void SendSearchAndRefresh(int i, int j)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = MagicStorage.Instance.GetPacket();
                packet.Write((byte)MessageType.SearchAndRefreshNetwork);
                packet.Write((short)i);
                packet.Write((short)j);
                packet.Send();
            }
        }

        private static void ReceiveSearchAndRefresh(BinaryReader reader)
        {
            Point16 point = new Point16(reader.ReadInt16(), reader.ReadInt16());
            TEStorageComponent.SearchAndRefreshNetwork(point);
        }

        private static ModPacket PrepareStorageOperation(int ent, byte op)
        {
            ModPacket packet = MagicStorage.Instance.GetPacket();
            packet.Write((byte)MessageType.TryStorageOperation);
            packet.Write(ent);
            packet.Write(op);
            return packet;
        }

        private static ModPacket PrepareOperationResult(byte op)
        {
            ModPacket packet = MagicStorage.Instance.GetPacket();
            packet.Write((byte)MessageType.StorageOperationResult);
            packet.Write(op);
            return packet;
        }

        public static void SendDeposit(int ent, Item Item)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = PrepareStorageOperation(ent, 0);
                ItemIO.Send(Item, packet, true);
                packet.Send();
            }
        }

        public static void SendWithdraw(int ent, Item Item, bool toInventory = false)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = PrepareStorageOperation(ent, (byte)(toInventory ? 3 : 1));
                ItemIO.Send(Item, packet, true);
                packet.Send();
            }
        }

        public static void SendDepositAll(int ent, List<Item> Items)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = PrepareStorageOperation(ent, 2);
                packet.Write((byte)Items.Count);
                foreach (Item Item in Items)
                {
                    ItemIO.Send(Item, packet, true);
                }
                packet.Send();
            }
        }

        public static void ReceiveStorageOperation(BinaryReader reader, int sender)
        {
            if (Main.netMode != 2)
            {
                return;
            }
            int ent = reader.ReadInt32();
            if (!TileEntity.ByID.ContainsKey(ent) || !(TileEntity.ByID[ent] is TEStorageHeart))
            {
                return;
            }
            TEStorageHeart heart = (TEStorageHeart)TileEntity.ByID[ent];
            byte op = reader.ReadByte();
            if (op == 0)
            {
                Item Item = ItemIO.Receive(reader, true);
                heart.DepositItem(Item);
                if (!Item.IsAir)
                {
                    ModPacket packet = PrepareOperationResult(op);
                    ItemIO.Send(Item, packet, true);
                    packet.Send(sender);
                }
            }
            else if (op == 1 || op == 3)
            {
                Item Item = ItemIO.Receive(reader, true);
                Item = heart.TryWithdraw(Item);
                if (!Item.IsAir)
                {
                    ModPacket packet = PrepareOperationResult(op);
                    ItemIO.Send(Item, packet, true);
                    packet.Send(sender);
                }
            }
            else if (op == 2)
            {
                int count = reader.ReadByte();
                List<Item> Items = new List<Item>();
                StartUpdateQueue();
                for (int k = 0; k < count; k++)
                {
                    Item Item = ItemIO.Receive(reader, true);
                    heart.DepositItem(Item);
                    if (!Item.IsAir)
                    {
                        Items.Add(Item);
                    }
                }
                ProcessUpdateQueue();
                if (Items.Count > 0)
                {
                    ModPacket packet = PrepareOperationResult(op);
                    packet.Write((byte)Items.Count);
                    foreach (Item Item in Items)
                    {
                        ItemIO.Send(Item, packet, true);
                    }
                    packet.Send(sender);
                }
            }
            SendRefreshNetworkItems(ent);
        }

        public static void ReceiveOperationResult(BinaryReader reader)
        {
            if (Main.netMode != 1)
            {
                return;
            }
            Player player = Main.player[Main.myPlayer];
            byte op = reader.ReadByte();
            if (op == 0 || op == 1 || op == 3)
            {
                Item Item = ItemIO.Receive(reader, true);
                StoragePlayer.GetItem(Item, op != 3);
            }
            else if (op == 2)
            {
                int count = reader.ReadByte();
                for (int k = 0; k < count; k++)
                {
                    Item Item = ItemIO.Receive(reader, true);
                    StoragePlayer.GetItem(Item, false);
                }
            }
        }

        public static void SendRefreshNetworkItems(int ent)
        {
            if (Main.netMode == 2)
            {
                ModPacket packet = MagicStorage.Instance.GetPacket();
                packet.Write((byte)MessageType.RefreshNetworkItems);
                packet.Write(ent);
                packet.Send();
            }
        }

        public static void ClientSendTEUpdate(int id)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = MagicStorage.Instance.GetPacket();
                packet.Write((byte)MessageType.ClientSendTEUpdate);
                packet.Write(id);
                TileEntity.Write(packet, TileEntity.ByID[id], true);
                packet.Send();
            }
        }

        public static void ReceiveClientSendTEUpdate(BinaryReader reader, int sender)
        {
            if (Main.netMode == 2)
            {
                int id = reader.ReadInt32();
                TileEntity ent = TileEntity.Read(reader, true);
                ent.ID = id;
                TileEntity.ByID[id] = ent;
                TileEntity.ByPosition[ent.Position] = ent;
                if (ent is TEStorageUnit)
                {
                    TEStorageHeart heart = ((TEStorageUnit)ent).GetHeart();
                    if (heart != null)
                    {
                        heart.ResetCompactStage();
                    }
                }
                NetMessage.SendData(MessageID.TileEntitySharing, -1, sender, null, id, ent.Position.X, ent.Position.Y);
            }
        }

        private static ModPacket PrepareStationOperation(int ent, byte op)
        {
            ModPacket packet = MagicStorage.Instance.GetPacket();
            packet.Write((byte)MessageType.TryStationOperation);
            packet.Write(ent);
            packet.Write(op);
            return packet;
        }

        private static ModPacket PrepareStationResult(byte op)
        {
            ModPacket packet = MagicStorage.Instance.GetPacket();
            packet.Write((byte)MessageType.StationOperationResult);
            packet.Write(op);
            return packet;
        }

        public static void SendDepositStation(int ent, Item Item)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = PrepareStationOperation(ent, 0);
                ItemIO.Send(Item, packet, true);
                packet.Send();
            }
        }

        public static void SendWithdrawStation(int ent, int slot)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = PrepareStationOperation(ent, 1);
                packet.Write((byte)slot);
                packet.Send();
            }
        }

        public static void SendStationSlotClick(int ent, Item Item, int slot)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = PrepareStationOperation(ent, 2);
                ItemIO.Send(Item, packet, true);
                packet.Write((byte)slot);
                packet.Send();
            }
        }

        public static void ReceiveStationOperation(BinaryReader reader, int sender)
        {
            if (Main.netMode != 2)
            {
                return;
            }
            int ent = reader.ReadInt32();
            if (!TileEntity.ByID.ContainsKey(ent) || !(TileEntity.ByID[ent] is TECraftingAccess))
            {
                return;
            }
            TECraftingAccess access = (TECraftingAccess)TileEntity.ByID[ent];
            Item[] stations = access.stations;
            byte op = reader.ReadByte();
            if (op == 0)
            {
                Item Item = ItemIO.Receive(reader, true);
                access.TryDepositStation(Item);
                if (Item.stack > 0)
                {
                    ModPacket packet = PrepareStationResult(op);
                    ItemIO.Send(Item, packet, true);
                    packet.Send(sender);
                }
            }
            else if (op == 1)
            {
                int slot = reader.ReadByte();
                Item Item = access.TryWithdrawStation(slot);
                if (!Item.IsAir)
                {
                    ModPacket packet = PrepareStationResult(op);
                    ItemIO.Send(Item, packet, true);
                    packet.Send(sender);
                }
            }
            else if (op == 2)
            {
                Item Item = ItemIO.Receive(reader, true);
                int slot = reader.ReadByte();
                Item = access.DoStationSwap(Item, slot);
                if (!Item.IsAir)
                {
                    ModPacket packet = PrepareStationResult(op);
                    ItemIO.Send(Item, packet, true);
                    packet.Send(sender);
                }
            }
            Point16 pos = access.Position;
            StorageAccess ModTile = TileLoader.GetTile(Main.tile[pos.X, pos.Y].type) as StorageAccess;
            if (ModTile != null)
            {
                TEStorageHeart heart = ModTile.GetHeart(pos.X, pos.Y);
                if (heart != null)
                {
                    SendRefreshNetworkItems(heart.ID);
                }
            }
        }

        public static void ReceiveStationResult(BinaryReader reader)
        {
            if (Main.netMode != 1)
            {
                return;
            }
            Player player = Main.player[Main.myPlayer];
            byte op = reader.ReadByte();
            Item Item = ItemIO.Receive(reader, true);
            if (op == 2 && Main.playerInventory && Main.mouseItem.IsAir)
            {
                Main.mouseItem = Item;
                Item = new Item();
            }
            else if (op == 2 && Main.playerInventory && Main.mouseItem.type == Item.type)
            {
                int total = Main.mouseItem.stack + Item.stack;
                if (total > Main.mouseItem.maxStack)
                {
                    total = Main.mouseItem.maxStack;
                }
                int difference = total - Main.mouseItem.stack;
                Main.mouseItem.stack = total;
                Item.stack -= total;
            }
            if (Item.stack > 0)
            {
                Item = player.GetItem(Main.myPlayer, Item, GetItemSettings.InventoryEntityToPlayerInventorySettings);
                if (!Item.IsAir)
                {
                    player.QuickSpawnClonedItem(Item, Item.stack);
                }
            }
        }

        public static void SendResetCompactStage(int ent)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = MagicStorage.Instance.GetPacket();
                packet.Write((byte)MessageType.ResetCompactStage);
                packet.Write(ent);
                packet.Send();
            }
        }

        public static void ReceiveResetCompactStage(BinaryReader reader)
        {
            if (Main.netMode == 2)
            {
                int ent = reader.ReadInt32();
                if (TileEntity.ByID[ent] is TEStorageHeart)
                {
                    ((TEStorageHeart)TileEntity.ByID[ent]).ResetCompactStage();
                }
            }
        }

        public static void SendCraftRequest(int heart, List<Item> toWithdraw, Item result)
        {
            if (Main.netMode == 1)
            {
                ModPacket packet = MagicStorage.Instance.GetPacket();
                packet.Write((byte)MessageType.CraftRequest);
                packet.Write(heart);
                packet.Write(toWithdraw.Count);
                foreach (Item Item in toWithdraw)
                {
                    ItemIO.Send(Item, packet, true);
                }
                ItemIO.Send(result, packet, true);
                packet.Send();
            }
        }

        public static void ReceiveCraftRequest(BinaryReader reader, int sender)
        {
            if (Main.netMode != 2)
            {
                return;
            }
            int ent = reader.ReadInt32();
            if (!TileEntity.ByID.ContainsKey(ent) || !(TileEntity.ByID[ent] is TEStorageHeart))
            {
                return;
            }
            TEStorageHeart heart = (TEStorageHeart)TileEntity.ByID[ent];
            int count = reader.ReadInt32();
            List<Item> toWithdraw = new List<Item>();
            for (int k = 0; k < count; k++)
            {
                toWithdraw.Add(ItemIO.Receive(reader, true));
            }
            Item result = ItemIO.Receive(reader, true);
            List<Item> Items = CraftingGUI.DoCraft(heart, toWithdraw, result);
            if (Items.Count > 0)
            {
                ModPacket packet = MagicStorage.Instance.GetPacket();
                packet.Write((byte)MessageType.CraftResult);
                packet.Write(Items.Count);
                foreach (Item Item in Items)
                {
                    ItemIO.Send(Item, packet, true);
                }
                packet.Send(sender);
            }
            SendRefreshNetworkItems(ent);
        }

        public static void ReceiveCraftResult(BinaryReader reader)
        {
            Player player = Main.player[Main.myPlayer];
            int count = reader.ReadInt32();
            for (int k = 0; k < count; k++)
            {
                Item Item = ItemIO.Receive(reader, true);
                player.QuickSpawnClonedItem(Item, Item.stack);
            }
        }
    }

    enum MessageType : byte
    {
        SearchAndRefreshNetwork,
        TryStorageOperation,
        StorageOperationResult,
        RefreshNetworkItems,
        ClientSendTEUpdate,
        TryStationOperation,
        StationOperationResult,
        ResetCompactStage,
        CraftRequest,
        CraftResult
    }
}