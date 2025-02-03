using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly HospitalManagementContext _context;

        public BillingController(HospitalManagementContext context)
        {
            _context = context;
        }

        // GET: api/Billing
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Billing>>> GetBillings()
        {
            return Ok(await _context.Billings
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .ToListAsync());
        }

        // GET: api/Billing/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetBillingById(int id)
        {
            var billing = await _context.Billings
                .Include(b => b.Patient)
                .Include(b => b.Appointment)
                .FirstOrDefaultAsync(b => b.BillingId == id);

            if (billing == null)
            {
                return NotFound(new { Message = $"Billing record with ID {id} not found." });
            }

            return Ok(billing);
        }

        // POST: api/Billing
        [HttpPost]
        public async Task<ActionResult<Billing>> CreateBilling([FromBody] Billing billing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate Patient and Appointment IDs
            var patient = await _context.Patients.FindAsync(billing.PatientId);
            var appointment = await _context.Appointments.FindAsync(billing.AppointmentId);

            if (patient == null || appointment == null)
            {
                return BadRequest(new { Message = "Invalid Patient or Appointment ID." });
            }

            _context.Billings.Add(billing);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBillingById), new { id = billing.BillingId }, billing);
        }

        // PUT: api/Billing/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBilling(int id, [FromBody] Billing billing)
        {
            if (id != billing.BillingId)
            {
                return BadRequest(new { Message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBilling = await _context.Billings.FindAsync(id);
            if (existingBilling == null)
            {
                return NotFound(new { Message = $"Billing record with ID {id} not found." });
            }

            existingBilling.TotalAmount = billing.TotalAmount;
            existingBilling.Insurance = billing.Insurance;
            existingBilling.PaidAmount = billing.PaidAmount;
            existingBilling.PatientId = billing.PatientId;
            existingBilling.AppointmentId = billing.AppointmentId;

            _context.Billings.Update(existingBilling);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Billing/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBilling(int id)
        {
            var billing = await _context.Billings.FindAsync(id);
            if (billing == null)
            {
                return NotFound(new { Message = $"Billing record with ID {id} not found." });
            }

            _context.Billings.Remove(billing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
