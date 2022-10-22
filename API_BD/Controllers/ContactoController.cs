using API_BD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_BD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactoController : ControllerBase
    {
        // OBLIGATORIO PARA TODOS LOS CONTROLADORES

        /*
         * Objeto de Contexto
         * Se crea una instancia del contexto de la BD
         */
        public readonly WEB_APIContext _dbcontext;

        /*
         * Cuando se llame el controlador, el constructor asigna
         * automáticamente el parámetro de contexto de la BD
         */
        public ContactoController(WEB_APIContext _context)
        {
            _dbcontext = _context;
        }
        // ----------------------------------------

        /*
         * Obtener la lista completa de la base de datos
         * IActionResult representa los resultados de un método
         * Se retorna un StatusCode para mensajes HTTPS
         */
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Contacto> lista = new List<Contacto>();

            try
            {

                /*
                 * La tabla Contactos contiene una clave foránea relacionada con
                 * la tabla tipo, por lo que dentro del modelo se genera este enlace 
                 * de manera directa
                 */
                lista = _dbcontext.Contactos.Include(x => x.ObjTipo).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = lista });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No se encontraron Datos"});

            }
        }


        [HttpGet]
        [Route("Detalle")]
        public IActionResult Detalle(int id)
        {

            Contacto ObjContacto = _dbcontext.Contactos.Find(id);

            if (ObjContacto == null)
            {
                return NotFound(new {mensaje = "No existe Datos" });
            }

            try
            {
                ObjContacto = _dbcontext.Contactos.Include(c => c.ObjTipo).Where(x=>x.IdContacto == id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = ObjContacto });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No se encontraron Datos" });

            }


        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Contacto objContacto)
        {

            try
            {
                _dbcontext.Contactos.Add(objContacto);               
                var result = _dbcontext.SaveChanges();
                if(result > 0)
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = "Datos Almacenados Correctamente" });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No es posible Almacenar Datos" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No es posible Almacenar Datos" });

            }

        }


        [HttpPut]
        [Route("Modificar")]
        public IActionResult Modificar([FromBody] Contacto objContacto )
        {
            Contacto _Contacto = _dbcontext.Contactos.Find(objContacto.IdContacto);

            if (_Contacto == null)
            {
                return NotFound(new { mensaje = "Contacto no encontrado" });
            }


            try
            {

                _Contacto.Nombre = string.IsNullOrEmpty(objContacto.Nombre)?  _Contacto.Nombre: objContacto.Nombre;
                _Contacto.Descripcion = objContacto.Descripcion is null ? _Contacto.Descripcion : objContacto.Descripcion;
                _Contacto.Telefono = objContacto.Telefono is null ? _Contacto.Telefono : objContacto.Telefono;
                _Contacto.IdTipo = objContacto.IdTipo is null ? _Contacto.IdTipo : objContacto.IdTipo;

                _dbcontext.Contactos.Update(_Contacto);
                var result = _dbcontext.SaveChanges();
            
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = "Datos Almacenados Correctamente" });
           }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No es posible Almacenar Datos" });

            }

        }



        [HttpDelete]
        [Route("Eliminar")]
        public IActionResult Eliminar (int id)
        {
            Contacto ObjContacto = _dbcontext.Contactos.Find(id);

            if (ObjContacto == null)
            {
                return NotFound(new { mensaje = "Contacto no encontrado" });
            }

            try
            {              
                _dbcontext.Contactos.Remove(ObjContacto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = "Contacto Eliminado Exitosamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No es posible Eliminar la información" });

            }
        }


    }
}
