namespace Tutorial11.DTO

{
    using System;
    using System.Collections.Generic;

    public class PrescriptionRequestDto
    {
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int? PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int DoctorId { get; set; }
        public List<MedicationItemDto> Medications { get; set; }
    }
}