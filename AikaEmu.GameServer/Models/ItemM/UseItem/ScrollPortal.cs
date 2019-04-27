using NLog;

namespace AikaEmu.GameServer.Models.ItemM.UseItem
{
    public class ScrollPortal : IUseItem
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Init(int data)
        {
            _log.Debug("ScrollPortal");
        }
    }
}