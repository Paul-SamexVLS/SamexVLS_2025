using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamexVLS_2025.Data;
using SamexVLS_2025.Models;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SamexVLS_2025.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private appDBcontext DBcontext;

        public HomeController(ILogger<HomeController> logger, appDBcontext dBcontext)
        {
            _logger = logger;
            DBcontext = dBcontext;
        }
        public async Task<IActionResult> DeleteQuote(int id, IFormCollection form)
        {
            try
            {
                if (id == 0)
                {
                    id = int.Parse(form["_id"].ToString());
                }
                var objClass = await DBcontext.Quotes.Where(s => s.Id == id).DefaultIfEmpty().FirstOrDefaultAsync();
                objClass.IsDeleted = true;
                DBcontext.Update(objClass);
                await DBcontext.SaveChangesAsync();

                return RedirectToAction(nameof(Quotations));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> DeleteQuoteDetails(int id)
        {
            try
            {
                var objClass = await DBcontext.QuotesDetails.Where(s => s.Id == id).DefaultIfEmpty().FirstOrDefaultAsync();
                objClass.IsDeleted = true;
                DBcontext.Update(objClass);
                await DBcontext.SaveChangesAsync();

                return RedirectToAction(nameof(Quotations));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
      

        public async Task<IActionResult> EditQuote(int id, IFormCollection form)
        {
            if (id == 0)
            {
                id = int.Parse(form["_id"].ToString());
            }

           
            var objClass = await DBcontext.Quotes.Where(s => s.Id == id).DefaultIfEmpty().FirstOrDefaultAsync();


            bool _estimate = false;
            bool _prequote = false;
            try
            {
                if (form["_isestimate"].ToString() == "on" || form["_isestimate"].ToString() == "true")
                {
                    _estimate = true;
                }
                if (form["_isprequote"].ToString() == "on" || form["_isprequote"].ToString() == "true")
                {
                    _prequote = true;
                }



                objClass.Customer = form["_customer"].ToString();
                objClass.SalesRep = form["_salesrep"].ToString();
                objClass.Address = form["_address"].ToString();
                objClass.ContactName = form["_contactname"].ToString();
                objClass.IsEstimate = _estimate;
                objClass.IsPrequote = _prequote;
                objClass.IsDeleted = false;
                objClass.LastUpdateDate = DateTime.Now;




                DBcontext.Update(objClass);
                await DBcontext.SaveChangesAsync();

                return RedirectToAction(nameof(Quotations));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> AddQuote(int id, IFormCollection form)
        {
            bool _estimate = false;
            bool _prequote = false;
            try
            {
                if (form["_isestimate"].ToString() == "on" || form["_isestimate"].ToString() == "true")
                {
                    _estimate = true;
                }
                if (form["_isprequote"].ToString() == "on" || form["_isprequote"].ToString() == "true")
                {
                    _prequote = true;
                }
                var objClass = new MR23_cotizacion
                {

                    //txtName = Request.Form["recipient-name"].ToString(),
                    Customer = form["customer-name"].ToString(),
                    SalesRep = form["salesrep-name"].ToString(),
                    Address = form["address-text"].ToString(),
                    ContactName = form["contact-name"].ToString(),
                    IsEstimate = _estimate,
                    IsDeleted = false,
                    IsPrequote = _prequote,
                    CreationDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now

                };


                await DBcontext.AddAsync(objClass);

                await DBcontext.SaveChangesAsync();

                return RedirectToAction(nameof(Quotations));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> AddQuoteDetails(int id, IFormCollection form)
        {
            bool _charge = false;
            bool _showinquote = false;
            try
            {
                if (form["charge-name"].ToString() == "on" || form["charge-name"].ToString() == "true")
                {
                    _charge = true;
                }
                if (form["showinquote-name"].ToString() == "on" || form["showinquote-name"].ToString() == "true")
                {
                    _showinquote = true;
                }
                var objClass2 = new MR23_cotizacion_detalles();
                objClass2.Class = form["class-name"].ToString() ?? string.Empty;
                objClass2.Profile = form["profile-name"].ToString() ?? string.Empty;
                objClass2.Description = form["description-name"].ToString() ?? string.Empty;    
                objClass2.BillingContainer = form["billingcontainer-name"].ToString() ?? string.Empty;
                objClass2.CollectionContainer = form["collectioncontainer-name"].ToString() ?? string.Empty;
                objClass2.ContainerDescription = form["containerdescription-name"].ToString() ?? string.Empty;
                objClass2.Treatment = form["treatment-name"].ToString() ?? string.Empty;
                objClass2.Destination = form["destination-name"].ToString() ?? string.Empty;
                if(form["quantity-name"].Count()>0)
                { 
                    if (int.TryParse(form["quantity-name"].ToString(), out int quantity))
                    {
                        objClass2.Quantity = quantity;
                    }
                    else
                    {
                        objClass2.Quantity = 0; // Default value if parsing fails
                    }
                }
                else
                {
                    objClass2.Quantity = 0; // Default value if the field is null
                }
               
                if (form["price-name"].Count() > 0)
                {
                    if (float.TryParse(form["price-name"].ToString(), out float price))
                    {
                        objClass2.Price = price;
                    }
                    else
                    {
                        objClass2.Price = 0.0f; // Default value if parsing fails
                    }
                }
                else
                {
                    objClass2.Price = 0.0f; // Default value if the field is null
                }
                
                objClass2.Currency = form["currency-name"].ToString() ?? string.Empty;
                if (form["transportationcost-name"].Count() > 0)
                {
                    if (float.TryParse(form["transportationcost-name"].ToString(), out float transportationCost))
                    {
                        objClass2.TransportationCost = transportationCost;
                    }
                    else
                    {
                        objClass2.TransportationCost = 0.0f; // Default value if parsing fails
                    }
                }
                else
                {
                    objClass2.TransportationCost = 0.0f; // Default value if the field is null
                }
               
                if (form["overcharges-name"].Count() > 0)
                {
                    if (float.TryParse(form["overcharges-name"].ToString(), out float overcharges))
                    {
                        objClass2.Overcharges = overcharges;
                    }
                    else
                    {
                        objClass2.Overcharges = 0.0f; // Default value if parsing fails
                    }
                }
                else
                {
                    objClass2.Overcharges = 0.0f; // Default value if the field is null
                }
               
                if (form["minimum-name"].Count() > 0)
                {
                    if (float.TryParse(form["minimum-name"].ToString(), out float minimum))
                    {
                        objClass2.Minimum = minimum;
                    }
                    else
                    {
                        objClass2.Minimum = 0.0f; // Default value if parsing fails
                    }
                }
                else
                {
                    objClass2.Minimum = 0.0f; // Default value if the field is null
                }
              
                objClass2.Charge = _charge;
                if (form["maxweight-name"].Count() > 0)
                {
                    if (float.TryParse(form["maxweight-name"].ToString(), out float maxWeight))
                    {
                        objClass2.Maxweight = maxWeight;
                    }
                    else
                    {
                        objClass2.Maxweight = 0.0f; // Default value if parsing fails
                    }
                }
                else
                {
                    objClass2.Maxweight = 0.0f; // Default value if the field is null
                }
              
                objClass2.Notes = form["notes-name"].ToString() ?? string.Empty;
                objClass2.ShowInQuote = _showinquote;
                objClass2.ParentId = id;
                objClass2.IsDeleted = false;
                objClass2.CreationDate = DateTime.Now;
                objClass2.LastUpdateDate = DateTime.Now;


                

               
                await DBcontext.AddAsync(objClass2);
                await DBcontext.SaveChangesAsync();

                return RedirectToAction("Quote", new { id = id });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Quotations()
        {
            try
            {
                var model = DBcontext.Quotes.Where(s => s.IsDeleted == false).ToList() ?? new List<MR23_cotizacion>();
                return View(model);
            }
            catch (Exception ex)
            {
                // Loguea el error
                _logger.LogError(ex, "Error at loading quotations");
                return View("Error");
            }
        }

       
       

        public IActionResult Quote(int id)
        {
            try
            {

                ViewBag.Id = id;


            var model1 = DBcontext.Quotes.Where(s => s.IsDeleted == false && s.Id == id).ToList();
            var model2 = DBcontext.QuotesDetails.Where(s => s.IsDeleted == false && s.ParentId == id).ToList() ?? new List<MR23_cotizacion_detalles>(); ;
            var CombinedViewModel = new ViewModel
                {
                    Quote1 = model1,
                    QuoteDetails1 = model2
                };
            
            return View(CombinedViewModel);
            }
            catch (Exception ex)
            {
                // Loguea el error
                _logger.LogError(ex, "Error at loading a quote id:"+id.ToString());
                return View("Error");
            }
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
