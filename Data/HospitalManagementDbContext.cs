﻿using HospitalManagement.Models;
using HospitalManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Data
{
  

    public class HospitalManagementDbContext :IdentityDbContext<User, UserRole, long>
    {
        public HospitalManagementDbContext(DbContextOptions<HospitalManagementDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<SelectedLanguage> SelectedLanguages { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<SelectedSpecialityTag> SelectedSpecialityTags { get; set; }
        public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<InvestigationDoc> InvestigationDocs { get; set; }
        public DbSet<InvestigationTag> InvestigationTags { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
        public DbSet<PrescriptionNote> PrescriptionNotes { get; set; }
        public DbSet<PrescriptionPatientComplain> PrescriptionPatientComplains { get; set; }
        public DbSet<PrescriptionPatientExamination> PrescriptionPatientExaminations { get; set; }
        public DbSet<PatientDocument> PatientDocuments { get; set; }
       

    }
}
