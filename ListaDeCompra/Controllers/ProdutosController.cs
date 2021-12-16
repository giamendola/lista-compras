using AgendaContatos.DbRepository;
using ListaDeCompra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ListaDeCompra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutosDTO>>> GetProdutos()
        {           
            return await _context.Produtos.OrderByDescending(t => t.CreateDate)
                .Select(x => ProdutosDTO(x))
                .ToListAsync();
        }

        // GET: api/produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutosDTO>> GetProduto(long id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return ProdutosDTO(produto);
        }
        // PUT: api/produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProdutos(long id, ProdutosDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
            {
                return BadRequest();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            produto.Name = produtoDTO.Name;
            produto.IsComplete = produtoDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ProdutoExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        // POST: api/produtos
        [HttpPost]
        public async Task<ActionResult<ProdutosDTO>> CriarProduto(ProdutosDTO produtosDTO)
        {
            bool isCreateDateNull = true;

            if (!String.IsNullOrEmpty(produtosDTO.CreateDate.ToString()))
            {
                isCreateDateNull = false;                
            }

            var produto = new Produtos()
            {
                IsComplete = produtosDTO.IsComplete,
                Name = produtosDTO.Name,
                CreateDate = isCreateDateNull? DateTime.Now : produtosDTO.CreateDate
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProduto),
                new { id = produto.Id },
                ProdutosDTO(produto));
        }

        // DELETE: api/produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(long id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(long id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

        private static ProdutosDTO ProdutosDTO(Produtos produto) =>
            new ProdutosDTO
            {
                Id = produto.Id,
                Name = produto.Name,
                IsComplete = produto.IsComplete,
                CreateDate = produto.CreateDate
            };
    }
}