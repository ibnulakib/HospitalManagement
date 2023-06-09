﻿using HospitalManagement.Data;
using HospitalManagement.Helper;
using HospitalManagement.Models;
using HospitalManagement.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        HospitalManagementDbContext _context;
        UserManager<User> _userManager;
        RoleManager<UserRole> _roleManager;
        SignInManager<User> _signInManager;
        IWebHostEnvironment _webHostEnvironment;


        public DoctorController(
            HospitalManagementDbContext context,
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            SignInManager<User> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }




        public async Task<IActionResult> GetAllPatients(string search_key)
        {
            try
            {
                var allPatients = await _context.Users.Join(_context.UserRoles, outter => outter.Id, inner => inner.UserId, (outter, inner) => new { user = outter, role = inner }).
              Join(_context.Roles, outter => outter.role.RoleId, inner => inner.Id, (outter, inner) => new { user_role = outter, role = inner }).AsNoTracking().
              Where(a => a.user_role.user.IsActive == true && a.role.Name == "Patient").ToListAsync();


                var patient_list = new List<UserModel>();
                foreach (var item in allPatients)
                {
                    var patient = new UserModel();
                    var u = item.user_role.user;
                    patient.age = u.Age;
                    patient.city_name = u.city_name;
                    patient.country_name = u.country_name;
                    patient.country_phone_code = u.country_phone_code;
                    patient.country_short_name = u.country_short_name;
                    patient.email = u.Email;
                    patient.gender = u.Gender;
                    patient.created_date = u.CreatedDate;
                    patient.id = u.Id;
                    patient.isActive = u.IsActive;
                    patient.name = u.Name;
                    patient.phoneNumber = u.PhoneNumber;
                    patient.roles = new List<string> { "Patient" };
                    patient.state_name = u.state_name;
                    patient.username = u.UserName;

                    patient_list.Add(patient);
                }

                long patient_id;
                var parse_success = long.TryParse(search_key, out patient_id);
                if (parse_success == true)
                {
                    patient_list = patient_list.Where(a => a.id == patient_id ).ToList();
                }
                else
                {
                    var search_key_toUpper = search_key.ToUpper();
                    patient_list = patient_list.Where(a => a.name.ToUpper().IndexOf(search_key_toUpper) >= 0 || a.email.ToUpper().IndexOf(search_key_toUpper) >= 0).ToList();
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    patient_list
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }

        }




        public async Task<IActionResult> GetPatientDetails(long patient_id)
        {
            try
            {
                var patient = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == patient_id && a.IsActive == true);
                if(patient != null)
                {
                    var p = ModelBindingResolver.ResolveUser(patient);
                   

                    var u_roles = await _context.UserRoles.Join(_context.Roles, outter => outter.RoleId, inner => inner.Id, (outter, inner) => new 
                    { ur = outter, role = inner }).AsNoTracking().Where(a => a.ur.UserId == patient.Id).ToListAsync();
                    p.roles = new List<string>();
                    foreach(var role in u_roles)
                    {
                        p.roles.Add(role.role.Name);
                    }
                   
                    return new JsonResult(new
                    {
                        success = true,
                        error = false,
                        patient = p
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = false
                    });
                }
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }
        }






        public async Task<IActionResult> GetTodayPatients(long doctor_id)
        {
            try
            {
                var today = DateTime.Now;
                var doctor = await _context.Users.FirstOrDefaultAsync(a => a.Id == doctor_id);
                var patient_appointments = await _context.Users.Join(_context.DoctorAppointments, outter => outter.Id, inner => inner.PatientId, (outter, inner) =>
                 new { patient = outter, appointment = inner }).Where(a => a.appointment.DoctorId == doctor_id &&
                  a.appointment.AppointmentDate.Date == today.Date).ToListAsync();
                var patients = new List<UserModel>();

                foreach(var item in patient_appointments)
                {
                    var patient = new UserModel();
                    var ap = item.appointment;
                    var u = item.patient;
                    patient.age = u.Age;
                    patient.city_name = u.city_name;
                    patient.country_name = u.country_name;
                    patient.country_phone_code = u.country_phone_code;
                    patient.country_short_name = u.country_short_name;
                    patient.email = u.Email;
                    patient.gender = u.Gender;
                    patient.id = u.Id;
                    patient.isActive = u.IsActive;
                    patient.name = u.Name;
                    patient.phoneNumber = u.PhoneNumber;
                    patient.roles = new List<string> { "Patient" };
                    patient.state_name = u.state_name;
                    patient.username = u.UserName;
                    patient.created_date = u.CreatedDate;
                    patient.approved = u.Approved;
                    patient.biography = u.Biography;
                    patient.bloodGroup = u.BloodGroup;
                    patient.bmdc_certifcate = u.BMDC_certifcate;
                    patient.degree_title = u.DegreeTittle;
                    patient.doctor_title = u.DoctorTitle;
                    patient.experience = u.year_of_Experience;
                    patient.new_patient_visiting_price = u.NewPatientVisitingPrice;
                    patient.old_patient_visiting_price = u.OldPatientVisitingPrice;
                    patient.types_of = u.TypesOf;
                    patient.appointment = new DoctorAppointmentModel();
                    patient.appointment.appointment_date = ap.AppointmentDate;
                    patient.appointment.consulted = ap.Consulted;
                    patient.appointment.created_date = ap.CreatedDate;
                    patient.appointment.doctor_id = doctor_id;
                    patient.appointment.doctor_name = doctor.Name;
                    patient.appointment.id = ap.Id;
                    patient.appointment.serial_no = ap.SerialNo;
                    patient.appointment.visiting_price = ap.VisitingPrice;

                    patients.Add(patient);
                }



                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    patients
                });
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }
        }






        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                
                doctors = doctors.ToList();

                var doctor_list = new List<UserModel>();
                foreach(var item in doctors)
                {
                    doctor_list.Add(ModelBindingResolver.ResolveUser(item));
                }


                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    doctor_list
                });
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }
        }


       
    }
}
