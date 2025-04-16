using LogWebAppMvc.Models;
using LogWebAppMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogWebAppMvc.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<ActionResult<LogModel>> Index()
        {
            var logs = await _logService.GetAll();
            return View(logs);
        }
    }
}
