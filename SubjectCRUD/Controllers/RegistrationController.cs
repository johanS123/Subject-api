using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectCRUD.Data;
using SubjectCRUD.Models;

namespace SubjectCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations()
        {
            return await _context.Registrations
                    .Include(i => i.Student)
                    .ThenInclude(e => e.Registrations)
                    .Include(i => i.Subject)
                    .ThenInclude(m => m.Teacher)
                    .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(i => i.RegistrationId == id);

            if (registration == null) return NotFound();

            return Ok(registration);
        }

        [HttpPost]
        public async Task<ActionResult> RegistrationStudent([FromBody] Registration newRegistration)
        {
            // Verificar si el estudiante existe
            var student = await _context.Students.FindAsync(newRegistration.StudentId);
            if (student == null)
            {
                return NotFound("El estudiante no existe.");
            }

            // Verificar si la materia existe
            var subject = await _context.Subjects.Include(m => m.Teacher).FirstOrDefaultAsync(m => m.SubjectId == newRegistration.SubjectId);
            if (subject == null)
            {
                return NotFound("La materia no existe.");
            }

            // Verificar si el estudiante ya está inscrito en esta materia
            var registrationExisting = await _context.Registrations
                .AnyAsync(i => i.StudentId == newRegistration.StudentId && i.SubjectId == newRegistration.SubjectId);
            if (registrationExisting)
            {
                return BadRequest("El estudiante ya está inscrito en esta materia.");
            }

            // Verificar si el estudiante ya tiene 3 materias inscritas
            var totalRegistrations = await _context.Registrations.CountAsync(i => i.StudentId == newRegistration.StudentId);
            if (totalRegistrations >= 3)
            {
                return BadRequest("El estudiante no puede inscribirse en más de 3 materias.");
            }

            // Verificar si el estudiante tiene clases con el mismo profesor
            var hasClassWithSameTeacher = await _context.Registrations
                .Include(i => i.Subject)
                .AnyAsync(i => i.StudentId == newRegistration.StudentId && i.Subject.TeacherId == subject.SubjectId);
            if (hasClassWithSameTeacher)
            {
                return BadRequest("El estudiante no puede inscribirse en otra materia con el mismo profesor.");
            }

            // Registrar la inscripción
            _context.Registrations.Add(newRegistration);
            await _context.SaveChangesAsync();

            return Ok("Inscripción realizada con éxito.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound("La inscripción no existe.");
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            return Ok("Inscripción eliminada con éxito.");
        }
    }
}
