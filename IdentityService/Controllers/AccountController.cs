using IdentityService.Models;
using IdentityService.Services.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [HttpGet("Test/{name}")]
        public ActionResult<string> Test(string name)
        {
            return Ok($"hello {name}, welcome!");
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult<string>> CreateAccount([FromBody] CreateAccountForm account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resut= await this.accountService.CreateUser(account);    

            return Ok($"Account create {account.Email} ");
        }
    }
}
