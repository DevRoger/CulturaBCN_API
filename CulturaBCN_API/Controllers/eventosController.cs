using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        public async Task<IHttpActionResult> Puteventos()
        {
            var file = HttpContext.Current.Request.Files["photo"];
            if (file == null || file.ContentLength == 0)
            {
                return BadRequest("No se ha proporcionado ningún archivo.");
            }

            string nombre = HttpContext.Current.Request.Form["nombre"];
            string descripcion = HttpContext.Current.Request.Form["descripcion"];
            string lugar = HttpContext.Current.Request.Form["lugar"];
            DateTime fecha = DateTime.Parse(HttpContext.Current.Request.Form["fecha"]);
            TimeSpan hora_inicio = TimeSpan.Parse(HttpContext.Current.Request.Form["hora_inicio"]);
            TimeSpan hora_fin = TimeSpan.Parse(HttpContext.Current.Request.Form["hora_fin"]);
            decimal precio = decimal.Parse(HttpContext.Current.Request.Form["precio"]);
            bool enumerado = bool.Parse(HttpContext.Current.Request.Form["enumerado"]);
            int edad_minima = int.Parse(HttpContext.Current.Request.Form["edad_minima"]);
            int id_sala = int.Parse(HttpContext.Current.Request.Form["id_sala"]);
            string foto_url = HttpContext.Current.Request.Form["foto_url"];
            int id_evento = int.Parse(HttpContext.Current.Request.Form["id_evento"]);

            eventos evento = db.eventos.Find(id_evento);

            evento.precio = precio;
            evento.fecha = fecha;
            evento.nombre = nombre;
            evento.descripcion = descripcion;
            evento.fecha = fecha;
            evento.hora_fin = hora_fin;
            evento.hora_inicio = hora_inicio;
            evento.enumerado = enumerado;
            evento.id_sala = id_sala;
            evento.edad_minima = edad_minima;
            evento.lugar = lugar;
            evento.nombre = nombre;
            file.SaveAs(foto_url);

            db.Entry(evento).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(true);
        }

        // POST: api/eventos
        [ResponseType(typeof(eventos))]
        public IHttpActionResult Postusuarios()
        {
            var file = HttpContext.Current.Request.Files["photo"];
            if (file == null || file.ContentLength == 0)
            {
                return BadRequest("No se ha proporcionado ningún archivo.");
            }

            string nombre = HttpContext.Current.Request.Form["nombre"];
            string descripcion = HttpContext.Current.Request.Form["descripcion"];
            string lugar = HttpContext.Current.Request.Form["lugar"];
            DateTime fecha = DateTime.Parse(HttpContext.Current.Request.Form["fecha"]);
            TimeSpan hora_inicio = TimeSpan.Parse(HttpContext.Current.Request.Form["hora_inicio"]);
            TimeSpan hora_fin = TimeSpan.Parse(HttpContext.Current.Request.Form["hora_fin"]);
            decimal precio = decimal.Parse(HttpContext.Current.Request.Form["precio"]);
            bool enumerado = bool.Parse(HttpContext.Current.Request.Form["enumerado"]);
            int edad_minima = int.Parse(HttpContext.Current.Request.Form["edad_minima"]);
            int id_sala = int.Parse(HttpContext.Current.Request.Form["id_sala"]);

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(descripcion) || string.IsNullOrEmpty(lugar)
                 || id_sala == null || enumerado == null || edad_minima == null || precio == null)
            {
                return BadRequest("Todos los campos son obligatorios.");
            }
            eventos evento = new eventos();

            evento.nombre = nombre;
            
            evento.descripcion = descripcion;
            evento.lugar = lugar;
            evento.fecha = fecha;
            evento.hora_inicio = hora_inicio;
            evento.hora_fin = hora_fin;
            evento.precio = precio;
            evento.enumerado = enumerado;
            evento.edad_minima = edad_minima;
            evento.id_sala = id_sala;

            db.eventos.Add(evento);
            db.SaveChanges();

            string savedFilePath = SaveFile(file, evento.id_evento);

            evento.foto_url = savedFilePath;
            db.Entry(evento).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(evento);

        }
        private string SaveFile(HttpPostedFile file, int userId)
        {
            try
            {
                string relativePath = Path.Combine("Data", "Eventos");
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);


                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new Exception($"Tipo de archivo no permitido. Extensiones válidas: {string.Join(", ", allowedExtensions)}");
                }


                string fileName = $"avatar.{userId}{fileExtension}";


                string filePath = Path.Combine(fullPath, fileName);
                file.SaveAs(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo: {ex.Message}", ex);
            }
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