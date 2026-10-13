using Microsoft.AspNetCore.Mvc;
using Winiffer_P1_P4.Service;
using Winiffer_P1_P4.Models;

namespace Winiffer_P1_P4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController(AutorService autorService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SaveAsync(AutorRecord autor)
    {
        int id = await autorService.SaveAsync(autor);

        var nuevo = new AutorRecord(
            id,
            autor.Nombres,
            autor.Nacionalidad,
            autor.FechaNacimiento,
            autor.Sueldo
        );

        return Ok(nuevo);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var listado = await autorService.GetAll();

        return Ok(listado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var autor = await autorService.GetByIdAsync(id);

        if (autor == null)
            return NotFound();

        return Ok(autor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, AutorRecord autor)
    {
        int filas = await autorService.UpdateAsync(id, autor);

        if (filas == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        int filas = await autorService.DeleteAsync(id);

        if (filas == 0)
            return NotFound();

        return NoContent();
    }
}

