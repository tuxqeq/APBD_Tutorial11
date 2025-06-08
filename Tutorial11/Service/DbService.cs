using Tutorial11.Context;
using Tutorial11.DTO;
using Tutorial11.Models;

namespace Tutorial11.Service

{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class DbService : IDbService
    {
        private readonly PrescriptionDbContext _context;

        public DbService(PrescriptionDbContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionDetailsDto> AddPrescriptionAsync(PrescriptionRequestDto dto)
        {
            if (dto.Medications == null || dto.Medications.Count == 0 || dto.Medications.Count > 10)
                throw new Exception("Prescription must have 1 to 10 medications.");

            if (dto.DueDate < dto.Date)
                throw new Exception("DueDate must be greater than or equal to Date.");

            Patient patient = null;
            if (dto.PatientId.HasValue)
            {
                patient = await _context.Patients.FindAsync(dto.PatientId.Value);
            }
            if (patient == null)
            {
                patient = await _context.Patients.SingleOrDefaultAsync(p =>
                    p.FirstName == dto.PatientFirstName && p.LastName == dto.PatientLastName);
            }
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = dto.PatientFirstName,
                    LastName = dto.PatientLastName
                };
                _context.Patients.Add(patient);
            }

            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            if (doctor == null)
                throw new Exception("Doctor not found.");

            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                Patient = patient,
                Doctor = doctor,
                PrescriptionMedications = new List<PrescriptionMedication>()
            };

            foreach (var item in dto.Medications)
            {
                var medication = await _context.Medications.FindAsync(item.MedicationId);
                if (medication == null)
                    throw new Exception($"Medication with ID {item.MedicationId} not found.");

                prescription.PrescriptionMedications.Add(new PrescriptionMedication
                {
                    Medication = medication,
                    Dose = item.Dose
                });
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return new PrescriptionDetailsDto
            {
                PrescriptionId = prescription.PrescriptionId,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                Doctor = new DoctorDto
                {
                    DoctorId = doctor.DoctorId,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName
                },
                Medications = prescription.PrescriptionMedications
                    .Select(pm => new MedicationItemDto
                    {
                        MedicationId = pm.MedicationId,
                        Name = pm.Medication.Name,
                        Dose = pm.Dose
                    })
                    .ToList()
            };
        }

        public async Task<PatientDto> GetPatientAsync(int patientId)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedications)
                        .ThenInclude(pm => pm.Medication)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .SingleOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null) return null;

            var patientDto = new PatientDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Prescriptions = patient.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDetailsDto
                    {
                        PrescriptionId = pr.PrescriptionId,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Doctor = new DoctorDto
                        {
                            DoctorId = pr.Doctor.DoctorId,
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName
                        },
                        Medications = pr.PrescriptionMedications
                            .Select(pm => new MedicationItemDto
                            {
                                MedicationId = pm.MedicationId,
                                Name = pm.Medication.Name,
                                Dose = pm.Dose
                            })
                            .ToList()
                    })
                    .ToList()
            };

            return patientDto;
        }
    }
}