using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathGeneratorREST.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MathGeneratorREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathProblemController : ControllerBase
    {
        private readonly IMathService _mathService;

        public MathProblemController(IMathService mathService)
        {
            _mathService = mathService;
        }

        // GET: api/MathProblem
        [HttpGet]
        public string Get()
        {
            return _mathService.Generate(4,2,null);
        }

        // GET: api/MathProblem/5
        [HttpGet("{numProblems}/{min?}/{max?}", Name = "Get")]
        public IActionResult Get(int numProblems, int? min, int? max)
        {
            var data = _mathService.Generate(numProblems, min, max);

            if (data != null)
                return Ok(data);
            return NotFound();
        }

        // POST: api/MathProblem
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MathProblem/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
