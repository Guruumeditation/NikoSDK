using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NikoSDK;
using NikoSDK.Interfaces.Data;

namespace NikoRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private NikoClient _nikoClient;
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            _nikoClient = new NikoClient("192.168.0.27");
            _nikoClient.OnValueChanged += NikoClientOnOnValueChanged;
            _nikoClient.StartClient();
            await _nikoClient.StartEvents();
            var actions = await _nikoClient.GetActions();
            var myaction = actions.Data.Actions.First(d => d.Id == 31);
            var response = await _nikoClient.ExecuteCommand(31, myaction.Value == 0 ? 100 : 0);

            //await Task.Delay(2000);
            return new string[] { "value1", "value2" };
        }

        private void NikoClientOnOnValueChanged(object sender, IEvent e)
        {
            Debug.WriteLine($"Event raised. ID : {e.Data.FirstOrDefault()?.Id}, value : {e.Data.FirstOrDefault()?.Value}");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
