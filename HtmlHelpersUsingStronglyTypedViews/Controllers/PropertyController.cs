using HtmlHelpersUsingStronglyTypedViews.Models;
using HtmlHelpersUsingStronglyTypedViews.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HtmlHelpersUsingStronglyTypedViews.Controllers
{
    public class PropertyController : Controller
    {
        public IActionResult CreateProperty()
        {
            var model = new CreateViewModel();

            model.PropertyTypeList = Enum.GetValues(typeof(PropertyTypeEnum))
                .Cast<PropertyTypeEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            model.BhkTypeList = Enum.GetValues(typeof(BhkTypeEnum))
                .Cast<BhkTypeEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProperty(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.PropertyTypeList = Enum.GetValues(typeof(PropertyTypeEnum))
                    .Cast<PropertyTypeEnum>()
                    .Select(e => new SelectListItem
                    {
                        Value = e.ToString(),
                        Text = e.ToString()
                    });

                model.BhkTypeList = Enum.GetValues(typeof(BhkTypeEnum))
                    .Cast<BhkTypeEnum>()
                    .Select(e => new SelectListItem
                    {
                        Value = e.ToString(),
                        Text = e.ToString()
                    });

                return View(model);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            // Ensure folder exists
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string? savedImagePath = null;

            // Save images
            if (model.ImageUrl != null && model.ImageUrl.Any())
            {
                foreach (var file in model.ImageUrl)
                {
                    if (file.Length > 0)
                    {
                        // Unique file name
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        var filePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Save relative path 
                        savedImagePath = "/images/" + fileName;

                        break; // take only first image
                    }
                }
            }

            // Store data in TempData
            TempData["PropertyName"] = model.PropertyName;
            TempData["OwnerName"] = model.OwnerName;
            TempData["Price"] = model.Price;
            TempData["BedroomCount"] = model.BedroomCount;
            TempData["BathroomCount"] = model.BathroomCount;
            TempData["PropertyType"] = model.PropertyType?.ToString();
            TempData["BhkType"] = model.BhkType?.ToString();
            TempData["ImagePath"] = savedImagePath;

            return RedirectToAction("ShowProperty");
        }
        public IActionResult ShowProperty()
        {
            var model = new CreateViewModel
            {
                PropertyName = TempData["PropertyName"] as string,
                OwnerName = TempData["OwnerName"] as string,
                Price = TempData["Price"] != null ? Convert.ToInt32(TempData["Price"]) : 0,
                BedroomCount = TempData["BedroomCount"] != null ? Convert.ToInt32(TempData["BedroomCount"]) : 0,
                BathroomCount = TempData["BathroomCount"] != null ? Convert.ToInt32(TempData["BathroomCount"]) : 0,
            };

            // Enums
            var propertyTypeValue = TempData["PropertyType"] as string;

            if (Enum.TryParse<PropertyTypeEnum>(propertyTypeValue, out var propertyType))
            {
                model.PropertyType = propertyType;
            }

            var bhkValue = TempData["BhkType"] as string;

            if (Enum.TryParse<BhkTypeEnum>(bhkValue, out var bhkType))
            {
                model.BhkType = bhkType;
            }
        

            ViewBag.ImagePath = TempData["ImagePath"] as string;

            return View(model);
        }
    }
}
