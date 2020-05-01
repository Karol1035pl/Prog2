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
    public class CalculateController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string formula, double x)
        {
            OdwroconaNotacjaPolska test = new OdwroconaNotacjaPolska(formula);
            if (x.ToString() != null)
            {
                test.X = x;
                string[] tokeny = test.Tokeny();
                string[] postfix = test.ONP_postfix(tokeny);
                double wynik = test.ONP_postfix_oblicz(postfix);
                return Ok(new OkResponse<double>(wynik));
            }
            else
            {
                string error = "Nie wpisano zmiennej x";
                return BadRequest(new ErrorResponse(error));
            }
        }
        [HttpGet]
        [Route("xy")]
        public IActionResult Get(string formula, double from, double to, int n)
        {
            OdwroconaNotacjaPolska test = new OdwroconaNotacjaPolska(formula);
            if (from.ToString() != null || to.ToString() != null || n.ToString() != null)
            {
                test.X = Convert.ToDouble(n);
                string[] tokeny = test.Tokeny();
                string[] postfix = test.ONP_postfix(tokeny);
                try
                {
                    List<Punkty> wyniki = test.ONP_postfix_przedzial(postfix, from, to, n);
                    return Ok(new OkResponse<List<Punkty>>(wyniki));
                }
                catch (Exception e)
                {
                    return BadRequest(new ErrorResponse(e));
                }
            }
            else
            {
                string error = "Nie wpisano wszystkich zmiennych";
                return BadRequest(new ErrorResponse(error));
            }
        }
    }
}
