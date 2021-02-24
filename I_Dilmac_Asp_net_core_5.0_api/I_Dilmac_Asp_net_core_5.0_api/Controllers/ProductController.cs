using I_Dilmac_Asp_net_core_5._0_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace I_Dilmac_Asp_net_core_5._0_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Repos005Context _context;

        public ProductController(IConfiguration configuration, Repos005Context context)
        {
            _config = configuration;
            _context = context;
        }

        [HttpGet("get_product")]
        [Authorize]
        public async Task<ObjectResult> GetPProduct()
        {
            try
            {
                var product = await _context.Products.ToListAsync();
                return StatusCode(200, new { product });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("post_product")]
        [Authorize]
        public async Task<ObjectResult> PostProduct([FromBody] Product _product)
        {
            try
            {
                await _context.Products.AddAsync(_product);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Başarıyla oluşturuldu." });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPatch("patch_product")]
        [Authorize]
        public async Task<ObjectResult> PatchProduct([FromBody] Product _product)
        {
            try
            {
                var productDBExisting = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == _product.ProductId);
                if (productDBExisting == null)
                {
                    return StatusCode(400, new { message = "Güncellenecek kayıt bulanamadı" });
                }
                else
                {
                    _context.Entry(productDBExisting).State = EntityState.Detached;
                    _context.Products.Update(_product);
                    await _context.SaveChangesAsync();

                    return StatusCode(200, new { message = "Başarıyla güncellendi." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpDelete("delete_product/{id}")]
        [Authorize]
        public async Task<ObjectResult> DeleteProduct(int id)
        {
            try
            {
                var productDBExisting = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == id);
                if (productDBExisting != null)
                {
                    _context.Remove(productDBExisting);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, new { message = "Başarıyla silindi." });
                }
                else
                {
                    return StatusCode(400, new { message = "Silinecek kayıt bulunamadı" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
