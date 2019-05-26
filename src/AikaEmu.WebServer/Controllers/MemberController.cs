using AikaEmu.WebServer.Managers;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AikaEmu.WebServer.Controllers
{
    public class MemberController : Controller
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public string Aika_get_token(string id, string pw)
        {
            _log.Debug("Login: {0} / Pass: {1}", id, pw);
            return DataAuthManager.Instance.AuthAndUpdateAccount(id.Trim(), pw.Trim());
        }
    }
}