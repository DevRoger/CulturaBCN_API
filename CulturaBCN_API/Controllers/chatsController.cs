using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;
using System.Web.Http;
using CulturaBCN_API.Models;

namespace CulturaBCN_API.Controllers
{
    public class chatsController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        [HttpGet]
        [Route("api/chats/{id}")]
        [ResponseType(typeof(List<chats>))]
        public async Task<IHttpActionResult> GetChats(int id)
        {
            List<chats> list = db.chats.Where(c => c.id_usuario_1 == id || c.id_usuario_2 == id).ToList();
            return Ok(list);
        }
    }
}
