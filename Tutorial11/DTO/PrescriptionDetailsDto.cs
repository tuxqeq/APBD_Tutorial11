namespace Tutorial11.DTO
{
    using System;
    using System.Collections.Generic;

    public class PrescriptionDetailsDto
    {
        public int PrescriptionId { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicationItemDto> Medications { get; set; }
    }
}