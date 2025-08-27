using ForParts.DTO.Product;
using ForParts.DTOs.Product;
using ForParts.Exceptions.Product;
using ForParts.IService.Product;
using ForParts.IServices.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForParts.Controllers.Product
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        //ALTA
        [HttpPost("crearProducto")]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto dto)
        {
            //Antes de realizar la operacion, el sistema ya verifica la existencia de todos los datos
            try
            {
                var create = await _productService.CreateProductAsync(dto);
                if (!create)
                    return BadRequest(new { status = 400, message = "Error al crear el producto." });
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { satus = 400, message = e.Message });
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }

        //ELIMINAR
        [HttpDelete("eliminarProducto")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string codeProduct)
        {
            //Antes de realizar la operacion, el sistema ya verifica la existencia de todos los datos
            try
            {
                var x = await _productService.DeleteProductAsync(codeProduct);
                if (!x) return BadRequest(new { status = 400, message = "Error al eliminar el producto." });

                //Si la operacion se realizo correctamente, devolvemos un mensaje de exito
                return Ok("Producto eliminado correctamente.");
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }

        //ACTUALIZAR PRODUCTO
        [HttpPut("actualizarDescripcionProducto")]
        [Authorize]
        public async Task<IActionResult> UpdateDescriptionProduct(UpdateDescriptionProductDto dto)
        {
            //Antes de realizar la operacion, el sistema ya verifica la existencia de todos los datos
            try
            {
                var update = await _productService.UpdateDescriptionProductAsync(dto);
                if (!update)
                    return BadRequest(new { status = 400, message = "Error al actualizar el producto." });
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { satus = 400, message = e.Message });
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }


        //ACTUALIZAR PRODUCTO
        [HttpPut("actualizarImagenProducto")]
        [Authorize]
        public async Task<IActionResult> UpdateImageProduct([FromForm] UpdateImageProductDto dto)
        {
            //Antes de realizar la operacion, el sistema ya verifica la existencia de todos los datos
            try
            {
                var update = await _productService.UpdateImageProductAsync(dto);
                if (!update)
                    return BadRequest(new { status = 400, message = "Error al actualizar el producto." });
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { satus = 400, message = e.Message });
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }


        //OBTENER TODOS LOS PRODUCTOS
        [HttpGet("obtenerProductos")]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                if (products == null || !products.Any())
                    return NotFound(new { status = 404, message = "No hay productos registrados." });
                return Ok(products);
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }

        //OBTENER UN PRODUCTO
        [HttpGet("obtenerProducto")]
        [Authorize]
        public async Task<IActionResult> GetProductByCode( string codeProduct)
        {
            try
            {
                var products = await _productService.GetProductByCode(codeProduct);
                if (products == null)
                    return NotFound(new { status = 404, message = "No hay productos registrados." });
                return Ok(products);
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }

        //OBTENER INSUMOS QUE FORMAN AL PRODUCTO
        [HttpPost("obtenerInsumosDelProducto")]
        [Authorize]
        public async Task<IActionResult> GetSuppliesForProduct(string codeProduct)
        {
            try
            {
                var supplies = await _productService.GetSuppliesForProducts(codeProduct);
                if (supplies == null || !supplies.Any())
                    return NotFound(new { status = 404, message = "No hay productos registrados." });
                return Ok(supplies);
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }

        }

        //OBTENER MOVIMIENTOS DE PRODUCTOS
        [HttpGet("obtenerMovimientos")]
        [Authorize]
        public async Task<IActionResult> GetProductMovement()
        {
            try
            {
                var pm = await _productService.GetAllProductMovements();
                if (pm == null || !pm.Any())
                    return NotFound(new { status = 404, message = "No hay productos registrados." });
                return Ok(pm);
            }
            catch (ProductException e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error inesperado: " + e.Message });
            }
        }
    }
}
