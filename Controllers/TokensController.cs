using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prog2.Models;

namespace Prog2.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string formula)
        {
            try
            {
                OdwroconaNotacjaPolska test = new OdwroconaNotacjaPolska(formula);
                string[] tokeny = test.Tokeny();
                if (!test.Walidacja(tokeny)) throw new Exception("Niepoprawna formuła");
                var wynik = new
                {
                    infix = tokeny,
                    postfix = test.ONP_postfix(tokeny)
                };
                return Ok(new OkResponse<object>(wynik));
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse(e));
            }
        }
    }
}
