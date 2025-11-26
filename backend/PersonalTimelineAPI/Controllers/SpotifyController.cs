using Microsoft.AspNetCore.Mvc;
using PersonalTimelineAPI.DTOs;
using PersonalTimelineAPI.Services;

namespace PersonalTimelineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : ControllerBase
    {
        private readonly SpotifyService _spotifyService;

        public SpotifyController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpPost("connect")]
        public async Task<ActionResult<SpotifyConnectResult>> Connect([FromBody] ConnectRequest request)
        {
            var result = await _spotifyService.ConnectAsync(request.Code, request.UserId);
            return Ok(result);
        }

        [HttpPost("sync")]
        public async Task<ActionResult<SpotifySyncResult>> Sync([FromBody] UserIdRequest request)
        {
            var result = await _spotifyService.SyncAsync(request.UserId);
            return Ok(result);
        }

        [HttpGet("status")]
        public async Task<ActionResult<SpotifyStatus>> GetStatus([FromQuery] int userId)
        {
            var status = await _spotifyService.GetStatusAsync(userId);
            return Ok(status);
        }

        [HttpDelete("disconnect")]
        public async Task<IActionResult> Disconnect([FromQuery] int userId)
        {
            await _spotifyService.DisconnectAsync(userId);
            return NoContent();
        }

        public class ConnectRequest
        {
            public string Code { get; set; } = string.Empty;
            public int UserId { get; set; }
        }

        public class UserIdRequest
        {
            public int UserId { get; set; }
        }
    }
}
