using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using URLSortner.Model;

namespace URLSortner.Controllers
{
    //[ApiController]
    [Route("api/[Controller]/")]
    [EnableCors("Public_Origin")]

    public class URLShortController : ControllerBase
    {
        private ApplicationDBContext _dbContext;
        public URLShortController(ApplicationDBContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        [EnableCors("Public_Origin")]
        [HttpPost("Resolve/p")]
        public async Task<ActionResult> Atuhorize_RedirectToSource([FromBody] Redirect_Password body)
        {
            var variable = await GetExistingShortID(body.shortID);
            if (string.Equals(variable.Password, body.password))
            {
                return Ok(new RedirectSuccess(variable.OriginalURL));
            }
            else
            {
                return Unauthorized(new RedirectFail(body.shortID, "Password incorrect"));
            }


        }
        [EnableCors("Public_Origin")]
        [HttpGet("Resolve/{shortID}")]

        public async Task<ActionResult> RedirectToSource(string shortID)
        {
            var variable = await GetExistingShortID(shortID);

            if (variable == null)
            {
                return NotFound("URL not found");
            }
            else
            {
                if (variable.Password != null)
                {
                    return Unauthorized(new RedirectFail(shortID));
                }
                else
                {
                    return Ok(new RedirectSuccess(variable.OriginalURL));
                }
            }
        }

        [EnableCors("Public_Origin")]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] CreateRequest createRequest)
        {
            if (string.IsNullOrWhiteSpace(createRequest.URL))
                return BadRequest("URL is required");
            var oldOne = await _dbContext.URLShort.AsNoTracking().FirstOrDefaultAsync(x => x.OriginalURL == createRequest.URL);
            if (oldOne != null)
            {
                return Ok(new ShortResponse(oldOne.ShortID));
            }
            else
            {
                var entity = new Model.URLShort(createRequest.URL);

                try
                {
                    _dbContext.URLShort.Add(entity);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ShortResponse(entity.ShortID));
                }
                catch (DbUpdateException ex)
                {
                    // Collision → retry with new ShortID
                    return StatusCode(500, ex.Message);
                }
            }

            return StatusCode(500, "Failed to generate unique short URL");
        }

        private bool IsShortIdCollision(DbUpdateException ex)
        {
            return ex.InnerException is PostgresException pg &&
                   pg.SqlState == PostgresErrorCodes.UniqueViolation &&
                   pg.ConstraintName == "URLShort_ShortID_key";
        }
        private async Task<Model.URLShort> GetExistingShortID(string shortID)
        {
            var variable = await _dbContext.URLShort.AsNoTracking().Where(x => x.ShortID == shortID).FirstOrDefaultAsync(); // the as no tracking is done for a specific reason so that it wont track for changes but only gonna read
            return variable;
        }
    }
}
