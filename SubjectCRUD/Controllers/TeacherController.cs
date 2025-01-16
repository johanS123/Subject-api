using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectCRUD.Data;
using SubjectCRUD.Models;

namespace SubjectCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers
               .Include(p => p.Subjects)
               .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
            .Include(p => p.Subjects)
            .FirstOrDefaultAsync(p => p.TeacherId == id);

            if (teacher == null)
            {
                return NotFound("Profesor no encontrado.");
            }

            return teacher;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeacher([FromBody] Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.TeacherId }, teacher);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeacher(int id, [FromBody] Teacher updateTeacher)
        {
            if (id != updateTeacher.TeacherId)
            {
                return BadRequest("El ID del profesor no coincide.");
            }

            _context.Entry(updateTeacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Teachers.Any(p => p.TeacherId == id))
                {
                    return NotFound("Profesor no encontrado.");
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound("Profesor no encontrado.");
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return Ok("Profesor eliminado con éxito.");
        }

    }
}
