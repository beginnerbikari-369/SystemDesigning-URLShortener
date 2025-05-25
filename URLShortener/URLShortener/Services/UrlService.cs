using UrlShortener.Data;
using UrlShortener.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace UrlShortener.Services
{
    public class UrlService
    {
        private readonly AppDbContext _context;
        private const string BaseUrl = "https://short.ly/";

        public UrlService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> ShortenAsync(string longUrl)
        {
            var mapping = new UrlMapping { LongUrl = longUrl };
            _context.UrlMappings.Add(mapping);
            await _context.SaveChangesAsync();

            var code = Base62Encode(mapping.Id);
            mapping.ShortCode = code;
            await _context.SaveChangesAsync();

            return BaseUrl + code;
        }

        public async Task<string?> ExpandAsync(string shortCode)
        {
            var record = await _context.UrlMappings.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
            return record?.LongUrl;
        }

        private static string Base62Encode(int id)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var sb = new StringBuilder();

            while (id > 0)
            {
                sb.Insert(0, chars[id % 62]);
                id /= 62;
            }

            return sb.ToString().PadLeft(6, '0');
        }
    }
}
