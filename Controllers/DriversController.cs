using CarProject2.Models;
using CarProject2.REPOSITORIES;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CarProject2.Controllers
{
    public class DriversController : Controller
    {
        //private readonly string _repo;
        //public DriversController(DriversController driversController)
        //{
        //    _repo = driversController;//>> //ITS incorrect
        //    // chon nemitoneh convert koneh
        //     //>> its correct >>_repo = driversController.ToString(); 
        //}

        // be in marhale migan >>Dependency Injection (DI) 
        // agar ctor declare nakonam aps.net nemifahmeh ke _repo chieh ?? va null mifresteh  
        // pas bayad ctor bad az repository ijad beshe 
        private readonly IDriverRepository _repo;
        public DriversController(IDriverRepository idriverRepository)
        {
            _repo = idriverRepository;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAlllDrivers();
            return View(list);
        }
        public IActionResult Detail(int id)
        {
            var detail = _repo.GetDriverById(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }
      
        [HttpGet]
        public JsonResult GetDriverById(int id)// data ro bedeh be ajax ke namayesh bedeh after choos sujest 
        {
            var getbyid = _repo.GetDriverById(id);
            if (getbyid == null)
            {
                return Json(new List<Driver>());
            }
            return Json(getbyid);
        }

        [HttpGet]
        ///JsonResult BARAYE HAMOON SUJEST BE JSON PAFGE HASTESH
        public IActionResult SearchDrivers(string KeyWord)//data ro bedeh be ajax
        {
            var Result = _repo.GetSearchDrivers(KeyWord);

            if (string.IsNullOrEmpty(KeyWord))// agar input null bood ya empty bia 
            {//>> dar json  list ro bargardoon 
                return Json(new List<Driver>());// chon man mikham listi az driver haroo return komneh 
                //   return Json(KeyWord); in faghat hamoon key ro 
            }

            else if (Result == null || !Result.Any())//>> Result.Any() >> MIGEH AGE HADESGHALESH BARABAR YE CHIZI BOOD
            {
                return Json(new List<Driver>()); /// agar result null bood dobareh list khali bedeh 
            }
            //else
            //{
            //    var Result = _repo.GetSearchDrivers(KeyWord);

            //}
            return Json(Result);

        }
        [HttpGet]// read data ??
        // hamoon page hastesh
        // page joda baraye create 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // sent data ??
                   // hamoon page create hastesh ke vaghti karbar post(submit) ro mizanehh check mikoneh ke fill shodeh ya na  
        [ValidateAntiForgeryToken]
        // ye tocken amniati baraye intropt shodan haker hastesh   
        public IActionResult Create(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return View(driver);
            }
            var newid = _repo.AddDriver(driver);
            ViewBag.SuccessMessage = "the add Driver Was Successfully"; // after submit show this message
            ModelState.Clear(); // pak shodan form 
            return View();
            /*return View(new Driver());*/ // baraye in ke page refresh besheh va khali besheh ino (new Driver()) gozashtam
            //return RedirectToAction(nameof(Detail), new {id=newid});
            //return RedirectToAction("Detail");

        }
        [HttpGet] // namayesh form 
        public IActionResult Edit()
        {
            //var DriverUpdate = _repo.GetDriverById(Id);

            return View();

            //if (DriverUpdate == null)
            //{
            //    return NotFound();
            //}
            //else {
            //    return View(DriverUpdate);
            //}
            //return View(); >> >>page empty fore search 
            //var getidforedit = _repo.GetDriverById(id);
            //if (getidforedit != null)
            //{
            //    return View(getidforedit);
            //}
            //else
            //    return NotFound();
        }
        [HttpPost] // send form (zakhireh taghirat ya hamon save changes)
        public IActionResult Edit(string driver)
        {

            var Result = _repo.GetSearchDrivers(driver);
            //       var checkId = _repo.GetDriverById(id);

            //if (checkId == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return View(checkId);
            //}

            return View(Result);


            //if (!ModelState.IsValid)
            //{
            //    return View(driver);
            //}
            //  _repo.UpdateDriver(driver);
            //return RedirectToAction(nameof(Edit), new { id = driver.id });// in miad pass mideh be detail with id 
            //>>avazesh kardam >> mideh be Edit 
        }
        [HttpPost]
        public IActionResult EditFinallDriver(Driver driver)
        {
            if (driver == null)
            {

                // return Content("driver is null");//FAGHAT PAGE BLACK BA DATA EEE KE DARE MIZAREH     movaghati hastesh
                ViewBag.Error("there is no driver to udate");
                return View("Edit");
            }

            else
            {
                // return Content($"id:{driver.id} - NAME:{driver.FullName}");//FAGHAT PAGE BLACK BA DATA EEE KE DARE MIZAREH MESL id:0 - NAME:testHHH
                _repo.UpdateDriver(driver);
                TempData["Success"] = ("UPDATE is successfuly ");
                return RedirectToAction("Edit");
            }

            // return RedirectToAction("Edit"); //REDIRECT MISHEH BE METHOD GET Edit na post
        }
        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    var getdeleteid = _repo.GetDriverById(id);
        //    if (getdeleteid != null)
        //    {
        //        return View(getdeleteid);
        //    }
        //    else
        //        return NotFound();
        //}
        [HttpPost]
        public IActionResult DeleteFromClient(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = " Id is Invalid... ";
            }

            var Resault = _repo.GetDriverById(id);

            if (Resault == null)
            {
                TempData["Error"] = " The Driver is not Found... ";
            }
            //else >>else khali khatarnakeee va faghat khat avval ro run mikoneh va baghieh set mishan 
            //behtarahe bedoon {} nabasheh 
            
                _repo.DeleteDriver(id);
                TempData["Success"] = " Delete is successfuly... ";
                return RedirectToAction(nameof(Edit));
            

            //if (!ModelState.IsValid) check mikoneh ke khali nabashe mogheh fill the form 
            //{
            //    return View(id);
            //}
            //_repo.DeleteDriver(id);
        }
    }
}
