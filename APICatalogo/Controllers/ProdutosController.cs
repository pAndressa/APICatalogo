using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]        
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreço().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        // [HttpGet("{id:int:min(1)}",Name ="ObterProduto")]
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            var produtosDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtosDTO;
        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId}, produtoDTO);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            //var produto = _uof.Produtos.Find(id); esse vai procurar sempre pela primary key da tabela e pelo que estiver carreagado

            if(produto == null)
            {
                return NotFound();
            }
            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }
    }
}
