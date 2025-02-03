using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly HospitalManagementContext _context;

        public PrescriptionsController(HospitalManagementContext context)
        {
            _context = context;
        }

        // GET: api/Prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            var prescriptions = await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medication)
                .ToListAsync();

            return Ok(prescriptions);
        }

        // GET: api/Prescriptions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetPrescription(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medication)
                .FirstOrDefaultAsync(p => p.MedicationId == id);

            if (prescription == null)
            {
                return NotFound(new { Message = $"Prescription with ID {id} not found." });
            }

            return Ok(prescription);
        }

        // POST: api/Prescriptions
        [HttpPost]
        public async Task<ActionResult<Prescription>> CreatePrescription([FromBody] Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate Patient, Doctor, and Medication IDs
            var patient = await _context.Patients.FindAsync(prescription.PatientId);
            var doctor = await _context.Doctors.FindAsync(prescription.DoctorId);
            var medication = await _context.Medications.FindAsync(prescription.MedicationId);

            if (patient == null || doctor == null || medication == null)
            {
                return BadRequest(new { Message = "Invalid Patient, Doctor, or Medication ID." });
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.MedicationId }, prescription);
        }

        // PUT: api/Prescriptions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, [FromBody] Prescription prescription)
        {
            if (id != prescription.MedicationId)
            {
                return BadRequest(new { Message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPrescription = await _context.Prescriptions.FindAsync(id);
            if (existingPrescription == null)
            {
                return NotFound(new { Message = $"Prescription with ID {id} not found." });
            }

            // Update fields
            existingPrescription.PatientId = prescription.PatientId;
            existingPrescription.DoctorId = prescription.DoctorId;
            existingPrescription.MedicationId = prescription.MedicationId;
            existingPrescription.Dosage = prescription.Dosage;
            existingPrescription.Instructions = prescription.Instructions;

            _context.Prescriptions.Update(existingPrescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Prescriptions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound(new { Message = $"Prescription with ID {id} not found." });
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

