namespace Tutorial11.Models

{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Timestamp] // ensures concurrency token [oai_citation:3‡learn.microsoft.com](https://learn.microsoft.com/en-us/ef/core/saving/concurrency#:~:text=%5BTimestamp%5D%20public%20byte%5B%5D%20Version%20,get%3B%20set%3B)
        public byte[] RowVersion { get; set; }

        public ICollection<PrescriptionMedication> PrescriptionMedications { get; set; }
    }
}