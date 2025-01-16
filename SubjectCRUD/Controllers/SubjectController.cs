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
        public async Task<ActionResult<Subject>> CreateProduct(Subject subject)
        {
            if (!Enum.IsDefined(typeof(Modality), subject.Modality))
            {
                return BadRequest("Invalid modality. Allowed values are 'Virtual' or 'Presencial'.");
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
                subject.Modality = updatedSubject.Modality;
                subject.Credits = updatedSubject.Credits;

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
