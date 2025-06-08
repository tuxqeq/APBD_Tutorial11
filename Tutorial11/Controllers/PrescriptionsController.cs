using Tutorial11.DTO;
using Tutorial11.Service;

namespace Tutorial5.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PrescriptionsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDetailsDto>> AddPrescription(PrescriptionRequestDto dto)
        {
            try
            {
                var result = await _dbService.AddPrescriptionAsync(dto);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{patientId}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int patientId)
        {
            var patient = await _dbService.GetPatientAsync(patientId);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }
    }
}