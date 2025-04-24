using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CulturaBCN_API.Models;

namespace CulturaBCN_API.Controllers
{
    public class eventosController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        // GET: api/eventos
        public IQueryable<eventos> Geteventos()
        {
            return db.eventos;
        }

        // GET: api/eventos/5
        [ResponseType(typeof(eventos))]
        public async Task<IHttpActionResult> Geteventos(int id)
        {
            eventos eventos = await db.eventos.FindAsync(id);
            if (eventos == null)
            {
                return NotFound();
            }

            return Ok(eventos);
        }

        // PUT: api/eventos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puteventos(int id, eventos eventos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventos.id_evento)
            {
                return BadRequest();
            }

            db.Entry(eventos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!eventosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/eventos
        [ResponseType(typeof(eventos))]
        public async Task<IHttpActionResult> Posteventos(eventos eventos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.eventos.Add(eventos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eventos.id_evento }, eventos);
        }

        // DELETE: api/eventos/5
        [ResponseType(typeof(eventos))]
        public async Task<IHttpActionResult> Deleteeventos(int id)
        {
            eventos eventos = await db.eventos.FindAsync(id);
            if (eventos == null)
            {
                return NotFound();
            }

            db.eventos.Remove(eventos);
            await db.SaveChangesAsync();

            return Ok(eventos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool eventosExists(int id)
        {
            return db.eventos.Count(e => e.id_evento == id) > 0;
        }
    }
}