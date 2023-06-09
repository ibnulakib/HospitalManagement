﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagement.Data;
using HospitalManagement.Helper;
using HospitalManagement.Models;
using HospitalManagement.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        HospitalManagementDbContext _context;
        UserManager<User> _userManager;
        RoleManager<UserRole> _roleManager;
        SignInManager<User> _signInManager;
        IWebHostEnvironment _webHostEnvironment;


        public AppointmentController(
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




        public async Task<IActionResult> GeAvailableAppointmentDates(long doctor_id, long user_id)
        {
            try
            {
                var doctor = await _context.Users.Include(a => a.Schedules).AsNoTracking().FirstOrDefaultAsync(a => a.Id == doctor_id && a.Approved == true && a.IsActive == true);
               
                if (doctor != null)
                {
                    var today = DateTime.Now; //today is Sunday
                    var doctor_appointments = await _context.DoctorAppointments.AsNoTracking().Where(a => a.DoctorId == doctor_id && a.AppointmentDate >= today).ToListAsync();
                    var doctor_schedules = doctor.Schedules;

                    var availableDates = new List<DateTime>();
                    var unavailableDates = new List<DateTime>();
                    for (var i = 0; i < 30; i++)
                    {
                        var specific_date = today.AddDays(i);
                        //var isInScheduleList = false;

                        var schedule_day_item = doctor_schedules.FirstOrDefault(a => a.DayName == specific_date.DayOfWeek);
                        if(schedule_day_item != null)
                        {
                            var total_appointments = doctor_appointments.Where(a => a.AppointmentDate.Date == specific_date.Date).ToList();
                            var duration = schedule_day_item.EndTime - schedule_day_item.StartTime;
                            var max_appointments = Math.Floor(duration.TotalMinutes / Helper.MiscellaneousInfo.ConsultDoctorTimeDurationInMins);
                            if (total_appointments.Count < max_appointments)
                            {
                                availableDates.Add(specific_date.Date);
                            }
                            else
                            {
                                unavailableDates.Add(specific_date.Date);
                            }
                        }
                        else
                        {
                            unavailableDates.Add(specific_date.Date);
                        }
                    }

                    var schedules = new List<ScheduleModel>();
                    foreach(var item in doctor_schedules)
                    {
                        var schedule = new ScheduleModel
                        {
                            day_name = item.DayName,
                            end_time = item.EndTime,
                            id = item.Id,
                            start_time = item.StartTime
                        };
                        schedules.Add(schedule);
                    }

                    
                    var future_appointments = await _context.DoctorAppointments.AsNoTracking().Where(a => a.DoctorId == doctor_id && a.PatientId == user_id && 
                    a.AppointmentDate.Date >= today.Date).ToListAsync();
                    
                    var canCreateNewAppointment = true;
                    var appointments = new List<DoctorAppointmentModel>();
                    
                    if (future_appointments.Count > 0)
                    {
                        canCreateNewAppointment = false;
                        foreach(var item in future_appointments)
                        {
                           
                            var appointment = ModelBindingResolver.ResovleAppointment(item, ModelBindingResolver.ResolveUser(doctor));
                            appointments.Add(appointment);
                        }
                    }

                   

                    return new JsonResult(new
                    {
                        success = true,
                        error = false,
                        available_dates = availableDates,
                        unavailable_dates = unavailableDates,
                        schedules,
                        can_create_new_appointment = canCreateNewAppointment,
                        appointments
                    });

                }
                else
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = true,
                        error_msg = "Doctor not found"
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






        public async Task<IActionResult> GetAllDoctorList()
        {
            try
            {
                var doctors_list = await _context.Users.Include(a => a.Specialities).Join(_context.UserRoles, outter => outter.Id, inner => inner.UserId, 
                    (outter, inner) => new { doctors = outter, roles = inner }).Join(_context.Roles, outter => outter.roles.RoleId, inner => inner.Id, (outter, inner) => new
                    { doct_role = outter, role_collection = inner}).Where(a => a.role_collection.Name == "Doctor" && a.doct_role.doctors.Approved == true && 
                    a.doct_role.doctors.IsActive == true).ToListAsync();
                
                var listOfDoctors = new List<UserModel>();

                foreach (var item in doctors_list)
                {
                    var doctor = new UserModel();
                    doctor.age = item.doct_role.doctors.Age;
                    doctor.approved = item.doct_role.doctors.Approved;
                    doctor.bloodGroup = item.doct_role.doctors.BloodGroup;
                    doctor.bmdc_certifcate = item.doct_role.doctors.BMDC_certifcate;
                    doctor.city_name = item.doct_role.doctors.city_name;
                    doctor.country_name = item.doct_role.doctors.country_name;
                    doctor.country_phone_code = item.doct_role.doctors.country_phone_code;
                    doctor.country_short_name = item.doct_role.doctors.country_short_name;
                    doctor.email = item.doct_role.doctors.Email;
                    doctor.gender = item.doct_role.doctors.Gender;
                    doctor.isActive = item.doct_role.doctors.IsActive;
                    doctor.id = item.doct_role.doctors.Id;
                    doctor.name = item.doct_role.doctors.Name;
                    doctor.phoneNumber = item.doct_role.doctors.PhoneNumber;
                    doctor.roles = new List<string> { "Doctor" };
                    doctor.created_date = item.doct_role.doctors.CreatedDate;
                    doctor.state_name = item.doct_role.doctors.state_name;
                    doctor.username = item.doct_role.doctors.UserName;
                    doctor.biography = item.doct_role.doctors.Biography;
                    doctor.degree_title = item.doct_role.doctors.DegreeTittle;
                    doctor.doctor_title = item.doct_role.doctors.DoctorTitle;
                    doctor.experience = item.doct_role.doctors.year_of_Experience;
                    doctor.new_patient_visiting_price = item.doct_role.doctors.NewPatientVisitingPrice;
                    doctor.old_patient_visiting_price = item.doct_role.doctors.OldPatientVisitingPrice;
                    doctor.types_of = item.doct_role.doctors.TypesOf;
                    doctor.specialities = new List<SpecialityTagModel>();

                    foreach(var splt_item in item.doct_role.doctors.Specialities)
                    {
                        var speciality = new SpecialityTagModel
                        {
                            id = splt_item.SpecialityTagId,
                            specialityName = splt_item.SpecialityName
                        };
                        doctor.specialities.Add(speciality);
                    }

                    listOfDoctors.Add(doctor);
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    doctor_list = listOfDoctors
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





        [HttpPost]
        public async Task<IActionResult> GetAppointmentSerialNo(DoctorAppointmentModel appointmentModel)
        {
          
            try
            {
               
                var doctor = await _context.Users.FirstOrDefaultAsync(a => a.Id == appointmentModel.doctor_id);
              
                var appointments = await _context.DoctorAppointments.AsNoTracking().Where(a => a.DoctorId == appointmentModel.doctor_id && 
                a.AppointmentDate.Date == appointmentModel.appointment_date.Date).ToListAsync();
                int newSerialNo = 1;
                if (appointments.Count > 0)
                {
                    int last_serialNo = (int)appointments.Max(a => a.SerialNo);
                    newSerialNo = last_serialNo + 1;
                }

                //visiting price calculation
                var threeMonthsAgo = appointmentModel.appointment_date.AddDays(-90);
                var last_appointments_count = await _context.DoctorAppointments.Where(a => a.AppointmentDate.Date >= threeMonthsAgo.Date &&
                a.DoctorId == appointmentModel.doctor_id && a.PatientId == appointmentModel.patient_id).CountAsync();

                var visiting_price = doctor.NewPatientVisitingPrice;
                if(last_appointments_count > 0)
                {
                    visiting_price = doctor.OldPatientVisitingPrice;
                }


                //patient appointments on that day
                var patient_appointments = await _context.DoctorAppointments.Join(_context.Users.Include(a => a.Schedules), outter => outter.DoctorId, inner => inner.Id, (outter, inner) => new 
                { appointment = outter, doctor = inner}).Join(_context.Users, outter => outter.appointment.PatientId, inner => inner.Id, (outter, inner) => new 
                {app_doc = outter, patient = inner }).AsNoTracking().Where(a => 
                a.app_doc.appointment.PatientId == appointmentModel.patient_id && a.app_doc.appointment.AppointmentDate.Date == appointmentModel.appointment_date.Date).ToListAsync();




                var pa = new List<DoctorAppointmentModel>();
                foreach(var item in patient_appointments)
                {
                    var appointment = ModelBindingResolver.ResovleAppointment(item.app_doc.appointment, ModelBindingResolver.ResolveUser(item.app_doc.doctor));
                    
                    pa.Add(appointment);
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    serial_no = newSerialNo,
                    visiting_price,
                    appointments = pa
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






        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(DoctorAppointmentModel doctorAppointment)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                  
                    var today = DateTime.Now;
                    var doctor_appointment = new DoctorAppointment();
                    doctor_appointment.AppointmentDate = DateTime.Parse(doctorAppointment.appointment_date_str);
                    doctor_appointment.CreatedDate = today;
                    doctor_appointment.DoctorId = doctorAppointment.doctor_id;
                    doctor_appointment.PatientId = doctorAppointment.patient_id;
                    doctor_appointment.PatientName = doctorAppointment.patient_name;
                    doctorAppointment.visiting_price = doctorAppointment.visiting_price;

                    var allAppointments = await _context.DoctorAppointments.AsNoTracking().Where(a => a.DoctorId == doctorAppointment.doctor_id &&
                    a.AppointmentDate.Date == doctor_appointment.AppointmentDate.Date).ToListAsync();

                    if (allAppointments.Count > 0)
                    {
                        doctor_appointment.SerialNo = (allAppointments.Max(a => a.SerialNo) + 1);
                    }
                    else
                    {
                        doctor_appointment.SerialNo = 1;
                    }

                    //visiting price calculation
                    //var threeMonthsAgo = doctor_appointment.AppointmentDate.AddDays(-90);
                    //var last_appointments_count = await _context.DoctorAppointments.Where(a => a.AppointmentDate.Date >= threeMonthsAgo.Date &&
                    //a.DoctorId == doctorAppointment.doctor_id && a.PatientId == doctorAppointment.patient_id).CountAsync();

                    //var visiting_price = doctor.NewPatientVisitingPrice;
                    //if (last_appointments_count > 0)
                    //{
                    //    visiting_price = doctor.OldPatientVisitingPrice;
                    //}

                    doctor_appointment.VisitingPrice = doctorAppointment.visiting_price;

                    _context.DoctorAppointments.Add(doctor_appointment);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new JsonResult(new
                    {
                        success = true,
                        error = false
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new JsonResult(new
                    {
                        success = false,
                        error = true,
                        error_msg = ex.Message
                    });
                }

            }

        }






        public async Task<IActionResult> GetAppointmentsByPatient(long patient_id)
        {
            try
            {
                var today = DateTime.Now;
               
                var doctor_appointments = await _context.DoctorAppointments.Join(_context.Users.Include(a => a.Schedules), outter => outter.DoctorId, inner => inner.Id,
                    (outter, inner) => new { ap = outter, doc = inner }).AsNoTracking().
                    Where(a => a.ap.PatientId == patient_id).ToListAsync();

                


                var appointmentList = new List<DoctorAppointmentModel>();
                foreach(var item in doctor_appointments)
                {
                    if(item.ap.AppointmentDate.Date < today.Date)
                    {
                        if(item.ap.Consulted == false)
                        {
                            var ap = await _context.DoctorAppointments.FirstOrDefaultAsync(a => a.Id == item.ap.Id);
                            _context.DoctorAppointments.Remove(ap);
                            await _context.SaveChangesAsync();
                            continue;
                        }
                    }

                    var appointment = new DoctorAppointmentModel();
                    appointment.appointment_date_str = item.ap.AppointmentDate.ToString();
                    appointment.appointment_date = item.ap.AppointmentDate.Date;
                    appointment.created_date = item.ap.CreatedDate;
                    appointment.doctor_id = item.ap.DoctorId;
                    appointment.doctor_name = item.doc.Name;
                    appointment.end_time = item.doc.Schedules.FirstOrDefault(a => a.DayName == item.ap.AppointmentDate.DayOfWeek).EndTime;
                    appointment.id = item.ap.Id;
                    appointment.patient_id = item.ap.PatientId;
                    appointment.patient_name = item.ap.PatientName;
                    appointment.serial_no = item.ap.SerialNo;
                    appointment.start_time = item.doc.Schedules.FirstOrDefault(a => a.DayName == item.ap.AppointmentDate.DayOfWeek).StartTime;
                    appointment.consulted = item.ap.Consulted;
                    appointment.visiting_price = item.ap.VisitingPrice;

                    appointmentList.Add(appointment);
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    appointments = appointmentList
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





        [HttpPost]
        public async Task<IActionResult> CancelAllAppointmentsByPatient(UserModel userModel)
        {
            try
            {
              
                var today = DateTime.Now;
                var appointments = await _context.DoctorAppointments.Where(a => a.PatientId == userModel.id && a.Consulted == false).ToListAsync();
                if(appointments.Count > 0)
                {
                    _context.DoctorAppointments.RemoveRange(appointments);
                    await _context.SaveChangesAsync();
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false
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









        [HttpPost]
        public async Task<IActionResult> CancelAppointment(DoctorAppointmentModel doctorAppointment)
        {
            try
            {
                
                var appointment = await _context.DoctorAppointments.FirstOrDefaultAsync(a => a.Id == doctorAppointment.id);
                
                if (appointment != null)
                {
                    _context.DoctorAppointments.Remove(appointment);
                    await _context.SaveChangesAsync();
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false
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













        public async Task<IActionResult> GetPatientAllAppointmentList(long patient_id, long doctor_id)
        {
            try
            {
                var today = DateTime.Now;
                var da_list = await _context.DoctorAppointments.Join(_context.Users.Include(a => a.Schedules),
                    outter => outter.DoctorId, inner => inner.Id, (outter, inner) => new { da = outter, doc = inner }).
                   Where(a => a.da.Consulted == true && a.da.PatientId == patient_id && a.da.AppointmentDate.Date < today.Date).ToListAsync();

                var appointments = new List<DoctorAppointmentModel>();
                foreach (var item in da_list)
                {
                    var da = item.da;
                    var doctor_appointment = new DoctorAppointmentModel();
                    doctor_appointment.appointment_date_str = da.AppointmentDate.ToShortDateString();
                    doctor_appointment.appointment_date = da.AppointmentDate.Date;
                    doctor_appointment.consulted = da.Consulted;
                    doctor_appointment.created_date = da.CreatedDate;
                    doctor_appointment.doctor_id = da.DoctorId;
                    doctor_appointment.doctor_name = item.doc.Name;
                    doctor_appointment.end_time = item.doc.Schedules.FirstOrDefault(a => a.DayName == da.AppointmentDate.DayOfWeek).EndTime;
                    doctor_appointment.id = da.Id;
                    doctor_appointment.patient_id = patient_id;
                    doctor_appointment.patient_name = da.PatientName;
                    doctor_appointment.serial_no = da.SerialNo;
                    doctor_appointment.start_time = item.doc.Schedules.FirstOrDefault(a => a.DayName == da.AppointmentDate.DayOfWeek).StartTime;
                    doctor_appointment.visiting_price = da.VisitingPrice;

                    appointments.Add(doctor_appointment);

                }

                var ua = await _context.DoctorAppointments.Join(_context.Users.Include(a => a.Schedules),
                   outter => outter.DoctorId, inner => inner.Id, (outter, inner) => new { da = outter, doc = inner }).
                  FirstOrDefaultAsync(a => a.da.PatientId == patient_id && a.da.AppointmentDate.Date >= today.Date && a.doc.Id == doctor_id);

                DoctorAppointmentModel upcomingAppointment = null;
                if(ua != null)
                {
                    upcomingAppointment = new DoctorAppointmentModel();
                    upcomingAppointment.appointment_date = ua.da.AppointmentDate;
                    upcomingAppointment.appointment_date_str = ua.da.AppointmentDate.ToShortDateString();
                    upcomingAppointment.consulted = ua.da.Consulted;
                    upcomingAppointment.created_date = ua.da.CreatedDate;
                    upcomingAppointment.doctor_id = ua.da.DoctorId;
                    upcomingAppointment.doctor_name = ua.doc.Name;
                    upcomingAppointment.end_time = ua.doc.Schedules.FirstOrDefault(a => a.DayName == ua.da.AppointmentDate.DayOfWeek).EndTime;
                    upcomingAppointment.id = ua.da.Id;
                    upcomingAppointment.patient_id = ua.da.PatientId;
                    upcomingAppointment.patient_name = ua.da.PatientName;
                    upcomingAppointment.serial_no = ua.da.SerialNo;
                    upcomingAppointment.start_time = ua.doc.Schedules.FirstOrDefault(a => a.DayName == ua.da.AppointmentDate.DayOfWeek).StartTime;
                    upcomingAppointment.visiting_price = ua.da.VisitingPrice;
                }
              
                

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    appointments,
                    upcoming_appointment = upcomingAppointment
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






        public async Task<IActionResult> GetTodayAppointmentsByDoctor(long doctor_id)
        {
            try
            {
                var today = DateTime.Now;
                var db_app_patients = await _context.DoctorAppointments.Join(_context.Users, o => o.PatientId, i => i.Id, (o, i) => new
                {
                    appointment = o,
                    patient = i
                }).AsNoTracking().Where(a => a.appointment.AppointmentDate.Date == today.Date & a.appointment.DoctorId == doctor_id).ToListAsync();

                var db_doctor = await _context.Users.Include(a => a.Schedules).AsNoTracking().FirstOrDefaultAsync(a => a.Id == doctor_id);

                var appointment_list = new List<DoctorAppointmentModel>();

                foreach(var app_item in db_app_patients)
                {
                    var appointment = ModelBindingResolver.ResovleAppointment(app_item.appointment, ModelBindingResolver.ResolveUser(db_doctor));

                    if(appointment.visiting_price == db_doctor.OldPatientVisitingPrice)
                    {
                        appointment.is_new_patient = false;
                    }
                    else
                    {
                        appointment.is_new_patient = true;
                    }

                    appointment_list.Add(appointment);
                    
                }

              
                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    appointment_list
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







        public async Task<IActionResult> GetUpcomingAppointmentsByPatient(long patient_id)
        {
            try
            {
                var today = DateTime.Now;
                var db_apps = await _context.DoctorAppointments.Join(_context.Users.Include(a => a.Schedules), o => o.DoctorId, i => i.Id, (o, i) => new
                {
                    app = o,
                    doc = i
                }).AsNoTracking().Where(a => a.app.PatientId == patient_id && a.app.AppointmentDate.Date >= today.Date).ToListAsync();

                var appointment_list = new List<DoctorAppointmentModel>();
                foreach(var item in db_apps)
                {
                    var appointment = ModelBindingResolver.ResovleAppointment(item.app, ModelBindingResolver.ResolveUser(item.doc));
                    appointment_list.Add(appointment);
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    appointment_list
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
