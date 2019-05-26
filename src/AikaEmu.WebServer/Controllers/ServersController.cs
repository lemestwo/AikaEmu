using System.Text;
using AikaEmu.WebServer.Managers;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AikaEmu.WebServer.Controllers
{
    public class ServersController : Controller
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public string Aika_get_chrcnt(string id, string pw)
        {
            _log.Debug("GetCharacters: {0} / {1}", id, pw);
            return DataGameManager.Instance.GetCharactersInfo(id.Trim(), pw.Trim());
        }

        public string Serv00()
        {
            _log.Debug("Serv00");
            var infos = new StringBuilder();

            // channel players online SL.bin first 17 values
            // only nation 3 and 4 active
            infos.AppendLine("-1");
            infos.AppendLine("0"); // 3 pvp
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0"); // 4 pvp
            infos.AppendLine("-1");
            infos.AppendLine("0"); // 3 pve
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0"); // 4 pve
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0"); // leopold
            infos.AppendLine("0"); // karena

            // unk
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0");
            infos.AppendLine("0");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0");
            infos.AppendLine("0");
            infos.AppendLine("0");

            // unk
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0");
            infos.AppendLine("0");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0");
            infos.AppendLine("0");
            infos.AppendLine("0");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("0");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");
            infos.AppendLine("-1");

            return infos.ToString();
        }
    }
}