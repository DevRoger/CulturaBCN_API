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
    public class usuariosController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        // GET: api/usuarios
        public IQueryable<usuarios> Getusuarios()
        {
            return db.usuarios;
        }

        // GET: api/usuarios/rol/{id}
        [HttpGet]
        [Route("api/usuarios/rol/{id:int}")]
        public IQueryable<usuarios> GetUsuariosPorRol(int id)
        {
            return db.usuarios.Where(u => u.id_rol == id);
        }

        // GET: api/usuarios/5
        [ResponseType(typeof(usuarios))]
        public async Task<IHttpActionResult> Getusuarios(int id)
        {
            usuarios usuarios = await db.usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // PUT: api/usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putusuarios(int id, usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarios.id_usuario)
            {
                return BadRequest();
            }

            db.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuariosExists(id))
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

        // POST: api/usuarios
        [ResponseType(typeof(usuarios))]
        public async Task<IHttpActionResult> Postusuarios(usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuarios.Add(usuarios);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuarios.id_usuario }, usuarios);
        }

        // DELETE: api/usuarios/5
        [ResponseType(typeof(usuarios))]
        public async Task<IHttpActionResult> Deleteusuarios(int id)
        {
            usuarios usuarios = await db.usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            db.usuarios.Remove(usuarios);
            await db.SaveChangesAsync();

            return Ok(usuarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usuariosExists(int id)
        {
            return db.usuarios.Count(e => e.id_usuario == id) > 0;
        }
    }
}