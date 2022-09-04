using DynamicDataTable;
using DynamicLinq.Models;
using DynamicLinq.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace DynamicLinq.ApiController
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class UserAccountApiController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountApiController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpGet]
        public IActionResult Gets([ModelBinder(typeof(DataTableRequestModelBinder))] DataTableRequest request)
        {
            var userAccounts = _userAccountService.GetAll();
            return Ok(userAccounts.ToDataTableResponse(request));
        }
    }
}
