using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
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

        /*
         * NO se puede utilizar porque tenemos dos clases, una para cada ID. Entonces al hacer llamada a la API en Android, da error por la clase definida en la Call.
         * 
        // GET: api/usuarios/rol/{id}
        [HttpGet]
        [Route("api/usuarios/rol/{id:int}")]
        public IQueryable<usuarios> GetUsuariosPorRol(int id)
        {
            return db.usuarios.Where(u => u.id_rol == id);
        }
        */

        // GET: api/usuarios/rol/1
        [HttpGet]
        [Route("api/usuarios/rol/1")]
        public IQueryable<usuarios> GetUsuariosRol1()
        {
            return db.usuarios.Where(u => u.id_rol == 1);
        }

        // GET: api/usuarios/rol/2
        [HttpGet]
        [Route("api/usuarios/rol/2")]
        public IQueryable<usuarios> GetUsuariosRol2()
        {
            return db.usuarios.Where(u => u.id_rol == 2);
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
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> Putusuarios()
        {
            var file = HttpContext.Current.Request.Files["photo"];
            if (file == null || file.ContentLength == 0)
            {
                return BadRequest("No se ha proporcionado ningún archivo.");
            }
            int id_usuario = int.Parse(HttpContext.Current.Request.Form["id_usuario"]);
            string nombre = HttpContext.Current.Request.Form["nombre"];
            string apellidos = HttpContext.Current.Request.Form["apellidos"];
            string correo = HttpContext.Current.Request.Form["correo"];
            string foto_url = HttpContext.Current.Request.Form["foto_url"];
            string contrasena_hash = HttpContext.Current.Request.Form["contrasena_hash"];
            DateTime fecha_nacimiento = DateTime.Parse(HttpContext.Current.Request.Form["fecha_nacimiento"]);
            String telefono = HttpContext.Current.Request.Form["telefono"];
            int id_rol = int.Parse(HttpContext.Current.Request.Form["id_rol"]);

            usuarios usu = db.usuarios.Find(id_usuario);

            usu.nombre = nombre;
            usu.correo = correo;
            usu.apellidos = apellidos;
            usu.id_rol = id_rol;
            usu.contrasena_hash=contrasena_hash;
            usu.telefono = telefono;
            usu.fecha_nacimiento= fecha_nacimiento;

            file.SaveAs(foto_url);
            
            db.Entry(usu).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(true);
        }
        // GET: obtener el rol del usuario
        [HttpGet]
        [Route("api/usuarios/rol/{id_user}")]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> GetRolUser(int id_user)
        {
            int id_rol = int.Parse(db.usuarios.Where(u => u.id_usuario == id_user).Select(u => new { u.id_rol }).FirstOrDefault().ToString());
            return Ok(id_rol);
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


        // POST: api/usuarios
        [ResponseType(typeof(usuarios))]
        public IHttpActionResult Postusuarios()
        {
            var file = HttpContext.Current.Request.Files["photo"];
            if (file == null || file.ContentLength == 0)
            {
                return BadRequest("No se ha proporcionado ningún archivo.");
            }

            string nombre = HttpContext.Current.Request.Form["nombre"];
            string apellidos = HttpContext.Current.Request.Form["apellidos"];
            string correo = HttpContext.Current.Request.Form["correo"];
            string contrasena_hash = HttpContext.Current.Request.Form["contrasena_hash"];
            DateTime fecha_nacimiento = DateTime.Parse(HttpContext.Current.Request.Form["fecha_nacimiento"]);
            String telefono = HttpContext.Current.Request.Form["telefono"];
            int id_rol = int.Parse(HttpContext.Current.Request.Form["id_rol"]);

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellidos) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena_hash) || fecha_nacimiento == null || string.IsNullOrEmpty(telefono)
                 || id_rol == null)
            {
                return BadRequest("Todos los campos son obligatorios.");
            }
            usuarios usu = new usuarios();

            usu.nombre = nombre;
            usu.foto_url = "";
            usu.apellidos = apellidos; 
            usu.correo = correo;
            usu.contrasena_hash = contrasena_hash;
            usu.telefono = telefono;
            usu.fecha_nacimiento = fecha_nacimiento;
            usu.id_rol = id_rol;
            
            db.usuarios.Add(usu);
            db.SaveChanges();

            string savedFilePath = SaveFile(file, usu.id_usuario);

            usu.foto_url = savedFilePath;

            db.Entry(usu).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(usu);

        }
        // POST: api/usuarios/imagen
        [HttpPost]
        [Route("api/usuarios/imagen")]
        public IHttpActionResult ObtenerImagenPost([FromBody] RutaImagenDto datos)
        {

            
            if (datos == null || string.IsNullOrWhiteSpace(datos.Foto_url))
                return BadRequest("Ruta no válida.");

            string rutaImagen = datos.Foto_url;

            if (!System.IO.File.Exists(rutaImagen))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(rutaImagen);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };

            string extension = Path.GetExtension(rutaImagen).ToLowerInvariant();
            string mime;
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    mime = "image/jpeg"; break;
                case ".png":
                    mime = "image/png"; break;
                case ".gif":
                    mime = "image/gif"; break;
                case ".bmp":
                    mime = "image/bmp"; break;
                default:
                    mime = "application/octet-stream"; break;
            }

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mime);
            return ResponseMessage(result);
        }
        private string SaveFile(HttpPostedFile file, int userId)
        {
            try
            {
                string relativePath = Path.Combine("Data", "Usuarios");
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
    }
}