using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIproductos_DataBaseFirst.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace WebAPIproductos_DataBaseFirst.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        public readonly DBPROYECTOContext dbcontext;
        public CategoriaController(DBPROYECTOContext context)
        {
            this.dbcontext = context;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            List<Categoria> categoria = new();
            try
            {
                categoria = await dbcontext.Categoria.ToListAsync();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", Response = categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = categoria });
            }
        }

        [HttpGet]
        [Route("{idCategoria:int}")]
        public async Task<ActionResult<Producto>> Obtener(int idCategoria)
        {


            var categoria = await dbcontext.Categoria
                .Include(c => c.Productos).Where(x => x.IdCategoria == idCategoria)
                .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound("No se encontro la categoria solicitada");
            }

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", Response = categoria });

        }

        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult> Post([FromBody] Categoria categoria)
        {

            try
            {
                dbcontext.Categoria.Add(categoria);
                await dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar/{idCategoria:int}")]
        public async Task<ActionResult> Put(int idCategoria, Categoria categoria)
        {
            var nuevo = await dbcontext.Categoria.FirstOrDefaultAsync(x => x.IdCategoria == idCategoria);

            if (nuevo == null)
                return NotFound("La categoria no existe");

            try
            {
                nuevo.Descripcion = categoria.Descripcion is null ? nuevo.Descripcion : categoria.Descripcion;

                dbcontext.Categoria.Update(nuevo);
                await dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{idCategoria:int}")]
        public async Task<ActionResult> Delete(int idCategoria)
        {
            var existe = await dbcontext.Categoria.AnyAsync(x => x.IdCategoria == idCategoria);

            if (!existe)
                return NotFound("La categoria que desea eliminar no existe");
            try
            {
                dbcontext.Categoria.Remove(new Categoria { IdCategoria = idCategoria });
                await dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
