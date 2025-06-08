using Tutorial11.DTO;
namespace Tutorial11.Service
{
    using System.Threading.Tasks;

    public interface IDbService
    {
        Task<PrescriptionDetailsDto> AddPrescriptionAsync(PrescriptionRequestDto prescriptionDto);
        Task<PatientDto> GetPatientAsync(int patientId);
    }
}