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
    public class rolesController : ApiController
    {
        private CulturaBCNEntities db = new CulturaBCNEntities();

        // GET: api/roles
        public IQueryable<roles> Getroles()
        {
            return db.roles;
        }

        // GET: api/roles/5
        [ResponseType(typeof(roles))]
        public async Task<IHttpActionResult> Getroles(int id)
        {
            roles roles = await db.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }

            return Ok(roles);
        }

        // PUT: api/roles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putroles(int id, roles roles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roles.id_rol)
            {
                return BadRequest();
            }

            db.Entry(roles).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rolesExists(id))
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

        // POST: api/roles
        [ResponseType(typeof(roles))]
        public async Task<IHttpActionResult> Postroles(roles roles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.roles.Add(roles);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = roles.id_rol }, roles);
        }

        // DELETE: api/roles/5
        [ResponseType(typeof(roles))]
        public async Task<IHttpActionResult> Deleteroles(int id)
        {
            roles roles = await db.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }

            db.roles.Remove(roles);
            await db.SaveChangesAsync();

            return Ok(roles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool rolesExists(int id)
        {
            return db.roles.Count(e => e.id_rol == id) > 0;
        }
    }
}