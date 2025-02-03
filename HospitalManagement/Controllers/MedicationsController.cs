using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly HospitalManagementContext _context;

        public MedicationsController(HospitalManagementContext context)
        {
            _context = context;
        }

        // GET: api/Medications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
        {
            return Ok(await _context.Medications.ToListAsync());
        }

        // GET: api/Medications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Medication>> GetMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);

            if (medication == null)
            {
                return NotFound(new { Message = $"Medication with ID {id} not found." });
            }

            return Ok(medication);
        }

        // POST: api/Medications
        [HttpPost]
        public async Task<ActionResult<Medication>> CreateMedication([FromBody] Medication medication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedication), new { id = medication.MedicationId }, medication);
        }

        // PUT: api/Medications/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedication(int id, [FromBody] Medication medication)
        {
            if (id != medication.MedicationId)
            {
                return BadRequest(new { Message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMedication = await _context.Medications.FindAsync(id);
            if (existingMedication == null)
            {
                return NotFound(new { Message = $"Medication with ID {id} not found." });
            }

            existingMedication.MedicationName = medication.MedicationName;
            existingMedication.MedicationDescription = medication.MedicationDescription;
            existingMedication.Price = medication.Price;

            _context.Medications.Update(existingMedication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Medications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound(new { Message = $"Medication with ID {id} not found." });
            }

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
