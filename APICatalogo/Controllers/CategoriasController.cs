using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes ="Bearer")]    
    //[Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        //private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            //_configuration = config;
            _mapper = mapper;
        }


        //[HttpGet("autor")]
        //public string GetAutor()
        //{
        //    var autor = _configuration["autor"];
        //    return $"Autor: {autor}";
        //}

        [HttpGet("produtos")]//é preciso tratar o startup no newtonsoftjson
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            var categoria = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDTO;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categoria = _uof.CategoriaRepository.Get().ToList();
            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDTO;
        }

        //[HttpGet("paginacao")]
        //public ActionResult<IEnumerable<CategoriaDTO>> GetPaginacao(int pag = 1, int reg = 5)
        //{
        //    //controle para caso exceda 1000 registros
        //    if (reg > 99)
        //        reg = 5;

        //    var categorias = _uof.CategoriaRepository.LocalizaPagina<Categoria>(pag, reg).ToList();

        //    var totalRegistros = _uof.CategoriaRepository.GeraTotalRegistros();
        //    var numeroPaginas = ((int)Math.Ceiling((double)totalRegistros / reg));

        //    Response.Headers["X-Total-Registros"] = totalRegistros.ToString();
        //    Response.Headers["X-Numero-Paginas"] = numeroPaginas.ToString();

        //    var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
        //    return categoriasDto;
        //}

        /// <summary>
        /// Obtém uma categoria pelo seu ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        [EnableCors("PermitirApiRequest")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDTO;
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <param name="categoriaDto"></param>
        /// <returns>O objeto categoria incluído</returns>
        /// <remarks>Retorna um objeto categoria incluído</remarks>
        [HttpPost]
        public ActionResult Post([FromBody]CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Put))]
        public ActionResult Put(int id, [FromBody]CategoriaDTO categoriaDto)
        {            
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            //var categoria = _uof.Produtos.Find(id); esse vai procurar sempre pela primary key da tabela e pelo que estiver carreagado

            if (categoria == null)
            {
                return NotFound();
            }
            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }
    }
}
