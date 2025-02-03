using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly HospitalManagementContext _context;

        public ReportsController(HospitalManagementContext context)
        {
            _context = context;
        }

        // GET: api/Reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            var reports = await _context.Reports
                .Include(r => r.Patient)
                .ToListAsync();

            return Ok(reports);
        }

        // GET: api/Reports/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(r => r.ReportId == id);

            if (report == null)
            {
                return NotFound(new { Message = $"Report with ID {id} not found." });
            }

            return Ok(report);
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult<Report>> CreateReport([FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate Patient ID
            var patient = await _context.Patients.FindAsync(report.PatientId);
            if (patient == null)
            {
                return BadRequest(new { Message = "Invalid Patient ID." });
            }

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, report);
        }

        // PUT: api/Reports/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] Report report)
        {
            if (id != report.ReportId)
            {
                return BadRequest(new { Message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingReport = await _context.Reports.FindAsync(id);
            if (existingReport == null)
            {
                return NotFound(new { Message = $"Report with ID {id} not found." });
            }

            // Update fields
            existingReport.PatientId = report.PatientId;
            existingReport.ReportType = report.ReportType;
            existingReport.ReportDetails = report.ReportDetails;

            _context.Reports.Update(existingReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Reports/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound(new { Message = $"Report with ID {id} not found." });
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
