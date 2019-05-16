using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendTradeData : GamePacket
    {
        private readonly Trade _trade;
        private readonly bool _isTarget;

        public SendTradeData(ushort conId, Trade trade, bool isTarget)
        {
            _trade = trade;
            _isTarget = isTarget;

            Opcode = (ushort) GameOpcode.SendTradeData;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            for (byte i = 0; i < 10; i++)
            {
                if (_isTarget)
                {
                    if (_trade.OwnerItems.ContainsKey(i)) stream.Write(_trade.OwnerItems[i]);
                    else stream.Write("", 20);
                }
                else
                {
                    if (_trade.TargetItems.ContainsKey(i)) stream.Write(_trade.TargetItems[i]);
                    else stream.Write("", 20);
                }
            }

            for (byte i = 0; i < 10; i++)
            {
                if (_isTarget)
                {
                    if (_trade.OwnerItems.ContainsKey(i)) stream.Write((byte) _trade.OwnerItems[i].Slot);
                    else stream.Write(byte.MaxValue);
                }
                else
                {
                    if (_trade.TargetItems.ContainsKey(i)) stream.Write(_trade.TargetItems[i].Slot);
                    else stream.Write(byte.MaxValue);
                }
            }

            stream.Write((ushort) 0);
            stream.Write(_isTarget ? _trade.OwnerMoney : _trade.TargetMoney);
            stream.Write(_isTarget ? _trade.OwnerOk : _trade.TargetOk);
            stream.Write(_isTarget ? _trade.OwnerConfirm : _trade.TargetConfirm);
            stream.Write(_isTarget ? _trade.TargetConId : _trade.OwnerConId);
            stream.Write(0);

            return stream;
        }
    }
}