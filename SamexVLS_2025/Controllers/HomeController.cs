using iText.Commons.Utils;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamexVLS_2025.Data;
using SamexVLS_2025.Models;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
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
        private static Cell createCell(string content, int colspan, int rowspan, Style style,int fontsize,TextAlignment align,iText.Kernel.Colors.Color color, iText.Kernel.Colors.Color textcolor,bool bold)
        {
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            if (bold)
            {
                font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            }
            Paragraph paragraph = new Paragraph(content)
                    .SetFontSize(fontsize)
                    .SetTextAlignment(align).SetFontColor(textcolor).SetFont(font);

            Cell cell = new Cell(rowspan, colspan).Add(paragraph).SetBackgroundColor(color);
            cell.AddStyle(style);

            return cell;
        }
        public ActionResult Pdf(int id)
        {
            iText.Kernel.Colors.Color verdeSamex = new DeviceRgb(0, 133, 64);
            iText.Kernel.Colors.Color grisSamex = new DeviceRgb(221, 221, 221);
            iText.Kernel.Colors.Color textoBlanco = ColorConstants.WHITE;
            iText.Kernel.Colors.Color textoNegro = ColorConstants.BLACK;
            var model1 = DBcontext.Quotes.Where(s => s.IsDeleted == false && s.Id == id).ToList();
            var model2 = DBcontext.QuotesDetails.Where(s => s.IsDeleted == false && s.ParentId == id).ToList() ?? new List<MR23_cotizacion_detalles>(); ;
            Style style = new Style().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            MemoryStream memoryStream = new MemoryStream();

            PdfWriter pw= new PdfWriter(memoryStream);
            PdfDocument pdfdocument = new PdfDocument(pw);
            Document doc = new Document(pdfdocument, PageSize.LETTER, false);





            ImageData data = ImageDataFactory.Create("wwwroot/AdminLTE/img/logo.png");

            Image image = new Image(data);
            image.SetWidth(235); // Set the width of the image
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
             
            
            table.AddCell(new Cell(2,1).Add(image).SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            if(model1.FirstOrDefault().IsPrequote)
            {
                table.AddCell(createCell("Precotización", 1, 1, style, 20, TextAlignment.CENTER, verdeSamex,textoBlanco  ,true));
            }
            else if(model1.FirstOrDefault().IsEstimate)
            {
                table.AddCell(createCell("Estimado", 1, 1, style, 20, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            }
            else
            {
                table.AddCell(createCell("Cotización", 1, 1, style, 20, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            }
            table.AddCell(createCell(model1.FirstOrDefault().Id.ToString(), 1, 1, style, 20, TextAlignment.CENTER, verdeSamex,textoBlanco,true));
            table.AddCell(createCell("\n",2,2, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            doc.Add(table);
            table=new Table(UnitValue.CreatePercentArray(7)).UseAllAvailableWidth();
            table.AddCell(createCell("INFORMACIÓN DEL SOLICITANTE", 3, 1, style, 10, TextAlignment.CENTER,grisSamex,textoNegro,true));
            table.AddCell(createCell(" ", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE,  textoNegro,false));
            table.AddCell(createCell("DATOS DE CONTACTO SAMEX", 3, 1, style, 10, TextAlignment.CENTER, grisSamex, textoNegro, true));

            table.AddCell(createCell("CLIENTE:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell(model1.FirstOrDefault().Customer, 2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));
            table.AddCell(createCell(" ", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, true));
            table.AddCell(createCell("FECHA:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell(model1.FirstOrDefault().CreationDate.ToString("dd/MM/yyyy"), 2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));
            
            table.AddCell(createCell("CONTACTO:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell(model1.FirstOrDefault().ContactName, 2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));
            table.AddCell(createCell(" ", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, true));
            table.AddCell(createCell("VENDEDOR:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell(model1.FirstOrDefault().SalesRep, 2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));

            table.AddCell(createCell("DIRECCIÓN:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell(model1.FirstOrDefault().Address, 2, 1, style, 10, TextAlignment.LEFT, grisSamex ,textoNegro,false));
            table.AddCell(createCell(" ", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, true));
            table.AddCell(createCell("DIRECCIÓN:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell("Andador Vecinal 3902 \r\nValle Redondo 22335 \r\nTijuana, BC Mexico:", 2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));

            table.AddCell(createCell("TELÉFONO:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell("(664) 634-0000",2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));
            table.AddCell(createCell(" ", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("TELÉFONO:", 1, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, true));
            table.AddCell(createCell("(664) 624-44550", 2, 1, style, 10, TextAlignment.LEFT, grisSamex, textoNegro, false));

            table.AddCell(createCell("\n", 7, 3, style, 10, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("DETALLES", 7, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));

            table.AddCell(createCell("ITEM", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("DESCRIPCIÓN", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("OBSERVACIONES", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("DISPOCISIÓN", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("UNIDAD", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("PRECIO", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("MONEDA", 1, 1, style, 10, TextAlignment.CENTER, verdeSamex, textoBlanco, true));
            foreach(var item in model2)
            {
                table.AddCell(createCell(item.Profile ?? string.Empty, 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
                table.AddCell(createCell(item.Description ?? string.Empty, 1, 1, style, 10, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
                table.AddCell(createCell(item.Notes ?? string.Empty, 1, 1, style, 10, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
                table.AddCell(createCell(item.Treatment ?? string.Empty, 1, 1, style, 10, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
                table.AddCell(createCell(item.ContainerDescription ?? string.Empty + " " + item.Profile ?? string.Empty , 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro,false));
                table.AddCell(createCell(item.Price.ToString("C"), 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE,textoNegro,false));
                table.AddCell(createCell(item.Currency ?? string.Empty , 1, 1, style, 10, TextAlignment.CENTER ,ColorConstants.WHITE,textoNegro,false));
            }
            table.AddCell(createCell("\n", 7, 3, style, 10, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("OBSERVACIONES", 7, 1, style, 10, TextAlignment.LEFT, verdeSamex, textoBlanco, true));
            table.AddCell(createCell("* Transportación de México hasta el sitio de disposición final (México-EUA).", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoBlanco, false));
            table.AddCell(createCell("* Se entregará documentación que incluye certificado de disposición.", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("* Perfiles, trámites aduanales, documentos de recolección, etc.", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("* Los precios no incluyen IVA.", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("* Terminos de pago: 15 días de crédito", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("* Tiempo de vigencia de la cotización sin firmar será 30 días", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("* Nota: Los costos establecidos en este formato tendrán duración de un año, y estos pueden variar en caso de que se presente una situación especial o cambios en la cuestión legal. En cuyo caso se elaborará una cotización con las modificaciones correspondientes", 7, 1, style, 8, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("\n", 7, 3, style, 10, TextAlignment.LEFT, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell(model1.FirstOrDefault().SalesRep, 2, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell(model1.FirstOrDefault().ContactName, 2, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 2, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            /*
            table.AddCell(createCell("________________", 2, 2, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("________________", 1, 2, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("________________", 2, 2, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));

            table.AddCell(createCell("Representante de ventas", 2, 2, style, 8, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("Cliente", 1, 2, style, 8, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("", 1, 1, style, 10, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            table.AddCell(createCell("Gerencia General", 2, 2, style, 8, TextAlignment.CENTER, ColorConstants.WHITE, textoNegro, false));
            */
            doc.Add(table);
           // pdfdocument.GetNumberOfPages();


            doc.Close();
            pdfdocument.Close();

            //add footer with page numbers  

            



            byte[] pdfBytes = memoryStream.ToArray();
            memoryStream = new MemoryStream();
            memoryStream.Write(pdfBytes, 0, pdfBytes.Length);
            memoryStream.Position = 0; // Reset the stream position to the beginning

            /*
            PdfReader pdfReader = new PdfReader(memoryStream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader, new PdfWriter(memoryStream));
            int totalages = pdfDoc.GetNumberOfPages();
            // Add a footer to each page
            for (int i = 1; i <= totalages; i++)
            {
                PdfPage page = pdfDoc.GetPage(i);
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 8)
                    .SetTextMatrix(30, 20) // Position the text at the bottom left corner
                    .ShowText($"Página {i} de {totalages}").EndText();
            }
            pdfDoc.Close(); // Close the PdfDocument to finalize changes
                
                 
            */
            return new FileStreamResult(memoryStream, "application/pdf");
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
