using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectCRUD.Data;
using SubjectCRUD.Models;

namespace SubjectCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            var subjects = await _context.Subjects.ToListAsync();

            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null) return NotFound();

            return Ok(subject);
        }

        [HttpPost]
        public async Task<ActionResult<Subject>> CreateSubject(Subject subject)
        {
            var nameSubject = await _context.Subjects.CountAsync(i => i.NameSubject == subject.NameSubject);
            if (nameSubject > 0)
            {
                return BadRequest("La materia que intenta agregar ya existe.");
            }

            var totalSubjests = await _context.Subjects.CountAsync(i => i.TeacherId == subject.TeacherId);
            if (totalSubjests >= 2)
            {
                return BadRequest("El Profesor no puede tener más de 2 materias.");
            }

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubject), new { id = subject.SubjectId }, subject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, Subject updatedSubject)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null) return NotFound();

            try
            {
                subject.NameSubject = updatedSubject.NameSubject;
                subject.Credits = updatedSubject.Credits;
                subject.TeacherId = updatedSubject.TeacherId;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Subjects.Any(s => s.SubjectId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
