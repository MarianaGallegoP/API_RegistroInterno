using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_RegistroInterno.Context;

namespace API_RegistroInterno.Controllers
{
    // TO DO: editar respuesta de endpoint de acuerdo a lo indicado en los requerimientos 

    // TO DO: cambiar esta ruta por la que se indica en los requerimientos
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioClientesExternosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioClientesExternosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // TO DO: cambiar este nombre para que la ruta concuerde con la indicada en los requerimientos
        [Route("Lista")]
        public async Task<ActionResult> Get()
        {
            var listaUsuariosClientesExternos = await _context.tblUsuariosClientesExternos.ToListAsync();
            return Ok(listaUsuariosClientesExternos);
        }

    }
}
