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
    public class asientosController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        // GET: api/asientos
        public IQueryable<asientos> Getasientos()
        {
            return db.asientos;
        }

        // GET: api/asientos/5
        [ResponseType(typeof(asientos))]
        public async Task<IHttpActionResult> Getasientos(int id)
        {
            asientos asientos = await db.asientos.FindAsync(id);
            if (asientos == null)
            {
                return NotFound();
            }

            return Ok(asientos);
        }

        [HttpGet]
        [Route("api/asientos/eventoasientoscounts/{id}")]
        [ResponseType(typeof(int))] 
        public async Task<IHttpActionResult> GetAsientoCountForEvent(int id) 
        {
            try
            {
                var eventExists = await db.eventos.AnyAsync(e => e.id_evento == id);

                if (!eventExists)
                {
                    return NotFound();
                }


                var seatCount = await db.asientos
                                        .Where(a => a.id_evento == id)
                                        .CountAsync();


                return Ok(seatCount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error executing GetAsientoCountForEvent for Event ID {id}: " + ex.Message);
                return InternalServerError(ex); // Retorna un error 500 Internal Server Error
            }
        }

        // GET: api/asientos/eventoasientoscounts
        [HttpGet]
        [Route("api/asientos/eventoasientoscounts")]
        public IHttpActionResult GetEventoAsientosCounts()
        {
            try
            {
                var result = db.eventos
                    .GroupJoin(db.asientos, 
                               evento => evento.id_evento, 
                               asiento => asiento.id_evento, 
                               (evento, asientosGrupo) => new 
                               {
                                   IdEvento = evento.id_evento,
                                   NombreEvento = evento.nombre,
                                   NumeroAsientos = asientosGrupo.Count() 
                               })
                    .ToList(); 

                return Ok(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error executing GetEventoAsientosCountsLinq: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        // PUT: api/asientos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putasientos(int id, asientos asientos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asientos.id_asiento)
            {
                return BadRequest();
            }

            db.Entry(asientos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!asientosExists(id))
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

        // POST: api/asientos
        [ResponseType(typeof(asientos))]
        public async Task<IHttpActionResult> Postasientos(asientos asientos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.asientos.Add(asientos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = asientos.id_asiento }, asientos);
        }

        // DELETE: api/asientos/5
        [ResponseType(typeof(asientos))]
        public async Task<IHttpActionResult> Deleteasientos(int id)
        {
            asientos asientos = await db.asientos.FindAsync(id);
            if (asientos == null)
            {
                return NotFound();
            }

            db.asientos.Remove(asientos);
            await db.SaveChangesAsync();

            return Ok(asientos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool asientosExists(int id)
        {
            return db.asientos.Count(e => e.id_asiento == id) > 0;
        }
    }
}