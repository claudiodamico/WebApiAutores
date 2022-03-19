using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        public ComentariosController(ApplicationDbContext context, 
            IMapper mapper,
            UserManager<IdentityUser> _userManager)
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = _userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync(); 
            
            return _mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpGet("{id:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetPorId(int id)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(comentarioDB => comentarioDB.Id == id);

            if (comentario == null)
            {
                return NotFound();
            }

            return _mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await _userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = _mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            _context.Add(comentario);
            await _context.SaveChangesAsync();

            var comentarioDTO = _mapper.Map<ComentarioDTO>(comentario);

            return CreatedAtRoute("obtenerComentario", new { id = comentario.Id, libroId = libroId }, comentarioDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await _context.Comentarios.AnyAsync(comentarioDB => comentarioDB.Id == id);

            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = _mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;   
            _context.Update(comentario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
