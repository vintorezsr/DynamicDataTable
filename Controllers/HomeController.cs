using DynamicDataTable;
using DynamicLinq.ApiController;
using DynamicLinq.Models;
using DynamicLinq.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DynamicLinq.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserAccountService _userAccountService;

        public HomeController(ILogger<HomeController> logger, IUserAccountService userAccountService)
        {
            _logger = logger;
            _userAccountService = userAccountService;
        }

        public IActionResult Index()
        {
            var userAccounts = _userAccountService.GetAll();
            var columns = new ExpressionCollection<UserAccount>
            {
                { nameof(UserAccount.Name), x => x.Name },
                { nameof(UserAccount.Email), x => x.Email },
            };
            userAccounts = userAccounts.OrderByDescending(columns["Name"]);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}