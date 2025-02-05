using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RL.Data.DataModels;
using RL.Data;
using System.Data.Entity;

namespace RL.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class ProcedureUserController : ControllerBase
    {
        private readonly ILogger<ProcedureUserController> _logger;
        private readonly RLContext _context;

        public ProcedureUserController(ILogger<ProcedureUserController> logger, RLContext context)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //[HttpGet]
        //[EnableQuery]
        //public IEnumerable<ProcedureUser> Get()
        //{
        //    return _context.ProcedureUsers.Include(pu => pu.User);
        //}

        [HttpGet("GetUsersByProcedure/{procedureId}")]
        [EnableQuery]
        public IActionResult GetUsersByProcedure(int procedureId)
        {
            // Join ProcedureUser with User to get the userId and name
            var users = _context.ProcedureUsers
                                .Where(pu => pu.ProcedureId == procedureId)
                                .Select(pu => new
                                {
                                    userId = pu.UserId,
                                    name = pu.User.Name
                                })
                                .ToList(); // Use ToList to execute the query and return the results

            return Ok(users); // Return the result as JSON
        }

        //[HttpGet("GetUsersByProcedure/{procedureId}")]
        //public async Task<IActionResult> GetUsersByProcedure(int procedureId)
        //{
        //    var users = await _context.ProcedureUsers
        //        .Where(pu => pu.ProcedureId == procedureId) // Filter by procedure ID
        //        .Include(pu => pu.User) // Include User entity
        //        .Select(pu => new
        //        {
        //            userId = pu.User.UserId, // Extract UserId
        //            name = pu.User.Name // Extract Name
        //        })
        //        .AsQueryable().ToListAsync(); // Convert to list


        //    if (!users.Any())
        //    {
        //        return NotFound(new { message = "No users found for this procedure." });
        //    }

        //    return Ok(new { users }); // Return as a JSON object with a 'users' array
        //}




    }
}
