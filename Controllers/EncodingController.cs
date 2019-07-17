using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestEncoding.Controllers
{
    [Route("{*url}")]
    [ApiController]
    public class EncodingController : ControllerBase
    {
        private static readonly IDictionary<string, Encoding> ENCODINGS = new Dictionary<string, Encoding>
        {
            { "utf8", Encoding.UTF8 },
            { "iso", Encoding.GetEncoding(28591) },
        };

        private readonly ILogger<EncodingController> m_logger;

        public EncodingController(ILogger<EncodingController> logger)
        {
            m_logger = logger;

            m_logger.LogDebug("NEW CALL");
        }

        [HttpPost]
        public IActionResult Post([FromQuery] string enc = "utf8")
        {
            var encoding = ENCODINGS[enc];
            var reader = new StreamReader(Request.Body, encoding);
            var readerEncoding = reader.CurrentEncoding;
            var readerContent = reader.ReadToEnd();

            Response.Headers.Add("Default-Encoding", Encoding.Default.BodyName);
            Response.Headers.Add("Reader-Encoding", readerEncoding.BodyName);
            Response.Headers.Add("Forced-Encoding", encoding.BodyName);

            m_logger.LogDebug($"Default-Encodin: {Encoding.Default.BodyName}");
            m_logger.LogDebug($"Reader-Encoding: {readerEncoding.BodyName}");
            m_logger.LogDebug($"Forced-Encoding: {encoding.BodyName}");
            m_logger.LogDebug($"Content: {readerContent}");

            Console.WriteLine($"Default-Encodin: {Encoding.Default.BodyName}");
            Console.WriteLine($"Reader-Encoding: {readerEncoding.BodyName}");
            Console.WriteLine($"Forced-Encoding: {encoding.BodyName}");
            Console.WriteLine($"Content: {readerContent}");

            return Content(readerContent, "text/html", encoding);
        }
    }
}
