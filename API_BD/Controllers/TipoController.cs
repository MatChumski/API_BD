using API_BD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_BD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoController : ControllerBase
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
        public TipoController(WEB_APIContext _context)
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
            List<Tipo> lista = new List<Tipo>();

            try
            {
                
                lista = _dbcontext.Tipos.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = lista });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No se encontraron Datos" });

            }
        }


        [HttpGet]
        [Route("Detalle")]
        public IActionResult Detalle(int id)
        {

            Tipo ObjTipo = _dbcontext.Tipos.Find(id);

            if (ObjTipo == null)
            {
                return NotFound(new { mensaje = "No existe Datos" });
            }

            try
            {
                ObjTipo = _dbcontext.Tipos.Where(x => x.IdTipo == id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = ObjTipo });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No se encontraron Datos" });

            }


        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Tipo objTipo)
        {
            try
            {
                _dbcontext.Tipos.Add(objTipo);
                var result = _dbcontext.SaveChanges();
                if (result > 0)
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
        public IActionResult Modificar([FromBody] Tipo objTipo)
        {
            Tipo _Tipo = _dbcontext.Tipos.Find(objTipo.IdTipo);

            if (_Tipo == null)
            {
                return NotFound(new { mensaje = "Tipo no encontrado" });
            }


            try
            {
                _Tipo.Descripcion = string.IsNullOrEmpty(objTipo.Descripcion) ? _Tipo.Descripcion : objTipo.Descripcion;
                
                _dbcontext.Tipos.Update(_Tipo);
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
        public IActionResult Eliminar(int id)
        {
            Tipo ObjTipo = _dbcontext.Tipos.Find(id);

            if (ObjTipo == null)
            {
                return NotFound(new { mensaje = "Tipo no encontrado" });
            }

            try
            {
                _dbcontext.Tipos.Remove(ObjTipo);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = "Tipo Eliminado Exitosamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No es posible Eliminar la información" });

            }
        }
    }
}
