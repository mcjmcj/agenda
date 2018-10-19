using Agenda.Dados.Models;
using Agenda.Dados.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Agenda.Controllers
{
    public class ValuesController : ApiController
    {
        

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {

            return null;
        }

        // POST api/values
        public void Post([FromBody]string value , Enuns.Tipo tipo)
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
