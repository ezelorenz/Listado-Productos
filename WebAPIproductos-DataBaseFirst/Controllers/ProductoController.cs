using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIproductos_DataBaseFirst.Entidades;
using System;
using Microsoft.AspNetCore.Cors;

namespace WebAPIproductos_DataBaseFirst.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DBPROYECTOContext dbcontext;
        public ProductoController(DBPROYECTOContext context)
        {
            this.dbcontext = context;
        }


        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<List<Producto>>> Get()
        {
            List<Producto> producto = new();
            try
            {
                producto = await dbcontext.Productos
                    .Include(c => c.Categoria)
                    .ToListAsync();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", Response = producto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = producto });
            }

        }

        [HttpGet]
        [Route("{idProducto:int}")]
        public async Task<ActionResult<Producto>> Obtener(int idProducto)
        {


            var producto = await dbcontext.Productos
                .Include(c => c.Categoria).Where(x => x.IdProducto == idProducto)
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound("No se encontro el producto solicitado");
            }

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", Response = producto });

        }

        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult> Post([FromBody] Producto producto)
        {

            try
            {
                dbcontext.Productos.Add(producto);
                await dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar/{idProducto:int}")]
        public async Task<ActionResult> Put(int idProducto, Producto producto)
        {
            var nuevo = await dbcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == idProducto);

            if (nuevo == null)
                return NotFound("El producto no existe");

            try
            {
                
                nuevo.CodigoBarra = producto.CodigoBarra is null ? nuevo.CodigoBarra : producto.CodigoBarra;
                nuevo.Descripcion = producto.Descripcion is null ? nuevo.Descripcion : producto.Descripcion;
                nuevo.Marca = producto.Marca is null ? nuevo.Marca : producto.Marca;
                //nuevo.IdCategoria = producto.IdCategoria is null ? nuevo.IdCategoria : producto.IdCategoria;
                nuevo.Precio = producto.Precio is null ? nuevo.Precio : producto.Precio;

                dbcontext.Productos.Update(nuevo);
                await dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public async Task<ActionResult> Delete(int idProducto)
        {
            var existe = await dbcontext.Productos.AnyAsync(x =>x.IdProducto == idProducto);

            if (!existe)
                return NotFound("El producto que desea eliminar no existe");
            try
            {
                dbcontext.Productos.Remove(new Producto { IdProducto = idProducto});
                await dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok" });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
