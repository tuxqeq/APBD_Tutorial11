namespace Tutorial11.DTO

{
    using System.Collections.Generic;

    public class PatientDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PrescriptionDetailsDto> Prescriptions { get; set; }
    }
}