using HW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HW.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Display()
        {
            masterEntities2 ent = new masterEntities2();
            //viewData = 
            return View(ent.Display());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterModel registerModel)
        {
            masterEntities2 ent = new masterEntities2();
            string message;

            //Validation
            DateTime temp1,temp2;
            if (DateTime.TryParse(registerModel.Information.StartDeliveryDate.ToString(),out temp1) && 
                DateTime.TryParse(registerModel.Information.EndDeliveryDate.ToString(),out temp2) &&
                    registerModel.Information.Product!=null &&
                    registerModel.Seller.SellerName!=null &&
                    registerModel.Seller.Country!=null )
            {
                //duplicate check with sp
                Int32? ID = (Int32?)ent.ValidateInformation(registerModel.Seller.Product,
                                                        registerModel.Information.StartDeliveryDate,
                                                        registerModel.Information.EndDeliveryDate);
                Int32? productNo = (Int32?)ent.ValidateSeller(registerModel.Seller.SellerName, 
                                                            registerModel.Seller.Country,
                                                            registerModel.Seller.Product);


                if (ID == 0 && productNo == 0)
                {
                    message = "Duplicate data, this data exists";
                    ViewBag.message = message;
                    return View();
                }
                else
                {
                    //insert data with sp
                    ent.InsertInformation(registerModel.Seller.Product, registerModel.Information.StartDeliveryDate, registerModel.Information.EndDeliveryDate);
                    ent.InsertSeller(registerModel.Seller.SellerName, registerModel.Seller.Country, registerModel.Seller.Product);
                    var dt = ent.Display().FirstOrDefault();
                    IEnumerable<Display_Result> dr = (IEnumerable<Display_Result>)ent.Display();
                    //Console.WriteLine(dr.ToString());
                    return View("Display",dr);
                }

            }
            else
            {
                message = "Invalid Data";
                ViewBag.message = message;
                return View();
            }

            

           
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
    }
}