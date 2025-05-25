using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _urlService;

        public UrlController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> Shorten([FromBody] string longUrl)
        {
            var shortUrl = await _urlService.ShortenAsync(longUrl);
            return Ok(new { shortUrl });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToLongUrl(string shortCode)
        {
            var longUrl = await _urlService.ExpandAsync(shortCode);
            if (longUrl == null) return NotFound();
            return Redirect(longUrl);
        }
    }
}
