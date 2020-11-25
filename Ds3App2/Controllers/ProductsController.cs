using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ds3App2.Models;
using Ds3App2.Services.ProductService;
using System.IO;
using Ds3App2.Extensions;

namespace Ds3App2.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProduct _product;
        public ProductsController(IProduct product)
        {
            _product = product;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Parts()
        {
            try
            {
                return View(await _product.GetProducts());
            }
            catch (Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            try
            {
                return View(await _product.GetProducts());
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> Create(Models.Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (!Helper.IsImage(ext))
                    {
                        ViewBag.Error = $"Invalid image format.";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/ProductImages"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        product.ProductImage = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Image upload was not successfully, please try again.";
                        return View(product);
                    }
                }
                try
                {
                    await _product.CreateProduct(product);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(product);
        }
        
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Product product = await _product.GetProductToEdit((Guid)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> Edit(Models.Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _product.EditProduct(product);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(product);
        }
        [AllowAnonymous]
        public async Task<JsonResult> Delete(Guid id)
        {
            await _product.DeleteProduct(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
