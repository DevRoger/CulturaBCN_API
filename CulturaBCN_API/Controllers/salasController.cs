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
    public class salasController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        // GET: api/salas
        public IQueryable<salas> Getsalas()
        {
            return db.salas;
        }

        // GET: api/salas/5
        [ResponseType(typeof(salas))]
        public async Task<IHttpActionResult> Getsalas(int id)
        {
            salas salas = await db.salas.FindAsync(id);
            if (salas == null)
            {
                return NotFound();
            }

            return Ok(salas);
        }

        // PUT: api/salas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putsalas(int id, salas salas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salas.id_sala)
            {
                return BadRequest();
            }

            db.Entry(salas).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!salasExists(id))
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

        // POST: api/salas
        [ResponseType(typeof(salas))]
        public async Task<IHttpActionResult> Postsalas(salas salas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.salas.Add(salas);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = salas.id_sala }, salas);
        }

        // DELETE: api/salas/5
        [ResponseType(typeof(salas))]
        public async Task<IHttpActionResult> Deletesalas(int id)
        {
            salas salas = await db.salas.FindAsync(id);
            if (salas == null)
            {
                return NotFound();
            }

            db.salas.Remove(salas);
            await db.SaveChangesAsync();

            return Ok(salas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool salasExists(int id)
        {
            return db.salas.Count(e => e.id_sala == id) > 0;
        }
    }
}