using System;
using AikaEmu.GameServer.Models.CharacterM;
using AikaEmu.GameServer.Models.NpcM.Dialog;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.GameServer.Managers
{
    public class NpcDialogManager : Singleton<NpcDialogManager>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void StartDialog(Character character, uint npcId, DialogType optionId, int unk)
        {
            var npc = WorldManager.Instance.GetNpc(npcId);
            if (npc == null) return;

            if (!MathUtils.CheckInRange(character.Position, npc.Position, 5)) return;

            if (optionId <= 0)
            {
                if (npc.DialogList == null || npc.DialogList.Count <= 0) return;
                character.SendPacket(new OpenNpcChat(npc.Id));
                character.SendPacket(new PlaySound(npc.SoundId, npc.SoundType));
                character.SendPacket(new ResetChatOptions());

                foreach (var dialog in npc.DialogList)
                {
                    if (dialog.SubOptionId == 0)
                    {
                        var temp = new NpcDialog(dialog.OptionId, 0, dialog.Text);
                        character.SendPacket(new SendNpcOption(temp));
                    }
                    else
                    {
                        // TODO - IMPLEMENT SUB TYPE
                        character.SendPacket(new CloseNpcChat());
                    }
                }
            }
            else
            {
                character.SendPacket(new CloseNpcChat());
                switch (optionId)
                {
                    case DialogType.Talk:
                        break;
                    case DialogType.Quest:
                        break;
                    case DialogType.Teleport:
                        break;
                    case DialogType.Store:
                        break;
                    case DialogType.Bank:
                        break;
                    case DialogType.ChatClose:
                        break;
                    case DialogType.GuildCreate:
                        break;
                    case DialogType.GuildBank:
                        break;
                    case DialogType.DialogEnd:
                        break;
                    case DialogType.PranStation:
                        break;
                    case DialogType.ChangeNation:
                        break;
                    case DialogType.QuestMenu:
                        break;
                    case DialogType.QuestAccept:
                        break;
                    case DialogType.QuestReward:
                        break;
                    case DialogType.SaveLocation:
                        break;
                    case DialogType.DungeonRanking:
                        break;
                    case DialogType.Repair:
                        break;
                    case DialogType.RepairAll:
                        break;
                    case DialogType.BlessFree:
                        break;
                    case DialogType.Upgrade:
                        break;
                    case DialogType.QuestReward2:
                        break;
                    case DialogType.NationStatus:
                        break;
                    case DialogType.GuildSkills:
                        break;
                    case DialogType.UpgradeCostume:
                        break;
                    case DialogType.BlessPaid:
                        break;
                    case DialogType.Fortification:
                        break;
                    case DialogType.Evolution:
                        break;
                    default:
                        return;
                }
            }
        }
    }
}