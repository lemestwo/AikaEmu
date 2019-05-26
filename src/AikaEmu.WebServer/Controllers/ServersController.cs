using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AikaEmu.WebServer.Controllers
{
    public class ServersController : Controller
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        
        [HttpPost]
        public string aika_get_chrcnt(string id, string pw)
        {
            _log.Debug("GetCharacters, {0}/{1}", id, pw);
            return "CNT 0 0 0 0\n<br>\n0 0 0 0";
        }

        public string serv00()
        {
            _log.Debug("Serv00");
            return "315\r\n181\r\n-1\r\n-1\r\n207\r\n64\r\n69\r\n-1\r\n-1\r\n77\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n7\r\n3\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n6\r\n8\r\n-1\r\n-1\r\n2\r\n0\r\n19\r\n-1\r\n-1\r\n18\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n26\r\n25\r\n-1\r\n-1\r\n21\r\n21\r\n20\r\n-1\r\n-1\r\n19\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1\r\n-1";
        }
    }
}