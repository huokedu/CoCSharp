﻿using CoCSharp.Data.Slots;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;
using System;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        public static void HandleMatchmakingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            // TODO: Check if avatar has shield or guard.
            var avatar = server.AvatarManager.GetRandomAvatar(client.Avatar.ID);
            if (avatar == null)
            {
                // Return home if their is no one to attack.
                client.NetworkManager.SendMessage(client.Avatar.OwnHomeDataMessage);
                return;
            }

            var ehdMessage = new EnemyHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(0),

                Unknown1 = null,

                Timestamp = DateTime.UtcNow,
                EnemyVillageData = new VillageMessageComponent(avatar),
                EnemyAvatarData = new AvatarMessageComponent(avatar),
                OwnAvatarData = new AvatarMessageComponent(client.Avatar),

                // This value can be either be 1,2,3.
                // When 1, the trophy and loot is not shown.
                Unknown2 = 3,
                Unknown3 = 0,
                Unknown4 = 0,
            };

            for (int i = 0; i < 18; i++)
                ehdMessage.OwnAvatarData.Units.Add(new UnitSlot(4000000 + i, 999));

            FancyConsole.WriteLine("[&(darkmagenta)Attack&(default)] Account &(darkcyan){0}&(default) attacked village &(darkcyan){1}&(default).",
                client.Avatar.Token, avatar.ID);
            client.NetworkManager.SendMessage(ehdMessage);
        }
    }
}