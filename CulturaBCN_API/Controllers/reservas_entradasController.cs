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
    public class reservas_entradasController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        // GET: api/reservas_entradas
        public IQueryable<reservas_entradas> Getreservas_entradas()
        {
            return db.reservas_entradas;
        }

        // GET: api/reservas_entradas/5
        [ResponseType(typeof(reservas_entradas))]
        public async Task<IHttpActionResult> Getreservas_entradas(int id)
        {
            reservas_entradas reservas_entradas = await db.reservas_entradas.FindAsync(id);
            if (reservas_entradas == null)
            {
                return NotFound();
            }

            return Ok(reservas_entradas);
        }

        // PUT: api/reservas_entradas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putreservas_entradas(int id, reservas_entradas reservas_entradas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservas_entradas.id_reserva)
            {
                return BadRequest();
            }

            db.Entry(reservas_entradas).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!reservas_entradasExists(id))
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

        // POST: api/reservas_entradas
        [ResponseType(typeof(reservas_entradas))]
        public async Task<IHttpActionResult> Postreservas_entradas(reservas_entradas reservas_entradas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.reservas_entradas.Add(reservas_entradas);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reservas_entradas.id_reserva }, reservas_entradas);
        }

        // DELETE: api/reservas_entradas/5
        [ResponseType(typeof(reservas_entradas))]
        public async Task<IHttpActionResult> Deletereservas_entradas(int id)
        {
            reservas_entradas reservas_entradas = await db.reservas_entradas.FindAsync(id);
            if (reservas_entradas == null)
            {
                return NotFound();
            }

            db.reservas_entradas.Remove(reservas_entradas);
            await db.SaveChangesAsync();

            return Ok(reservas_entradas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool reservas_entradasExists(int id)
        {
            return db.reservas_entradas.Count(e => e.id_reserva == id) > 0;
        }
    }
}