using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectCRUD.Data;
using SubjectCRUD.Models;

namespace SubjectCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
            //.Include(e => e.Registrations)
            //.ThenInclude(i => i.Subject)

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(e => e.Registrations)
                .ThenInclude(i => i.Subject)
                .FirstOrDefaultAsync(e => e.StudentId == id);

            if (student == null)
            {
                return NotFound("Estudiante no encontrado.");
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromBody] Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, [FromBody] Student updateStudent)
        {
            if (id != updateStudent.StudentId)
            {
                return BadRequest("El ID del estudiante no coincide.");
            }

            _context.Entry(updateStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.StudentId == id))
                {
                    return NotFound("Estudiante no encontrado.");
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Estudiante no encontrado.");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Estudiante eliminado con éxito.");
        }
    }
}
