using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Contracts;
using Serilog;
using Telemetry.Implementation;
using WebToInflux;

namespace WebToInfluxTake2.Controllers
{
    [TelemetryReporterInflux]
    public class DataController : ApiController
    {
        private readonly ILogger _logger;
        public DataController(/*ILogFactory logFactory = null*/)
        {
            //logFactory = logFactory ?? LogFactory.Current;
            //_logger = logFactory.Create<ValuesController>();
            _logger = Reporter.Default.LogFactory.Create<DataController>();

        }
        // GET api/values
        public async Task<string> Get()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            _logger.Verbose("Verbose log Data");
            _logger.Debug("Debug log Data");
            _logger.Information("Info log Data");
            _logger.Warning("Warn log Data");
            _logger.Error("Error log Data");
            return "ok";
        }

        // GET api/values/5
        public Task<string> Get(int id)
        {
            return Task.FromResult($"value {id}");
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
