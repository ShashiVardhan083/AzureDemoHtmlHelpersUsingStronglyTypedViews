using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using HtmlHelpersUsingStronglyTypedViews.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HtmlHelpersUsingStronglyTypedViews.Models
{
    public class CreateViewModel
    {
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "Property Name is Required.")]
        [StringLength(40,MinimumLength = 3, ErrorMessage = "Property Name should be minimum 40 characters")]
        [Display(Name = "Property Name")]
        public string? PropertyName { get; set; }

        [Required(ErrorMessage = "Owner Name is Required.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Owner Name should be minimum 40 characters")]
        [Display(Name = "Owner Name")]
        public string? OwnerName { get; set; }

        public Guid OwnerId { get; set; }
        [Required(ErrorMessage = "PropertyType is Required.")]
        [Display(Name = " Property Type")]
        public PropertyTypeEnum? PropertyType { get; set; } //rent sale
        public BhkTypeEnum? BhkType {  get; set; }
        public IEnumerable<SelectListItem>? PropertyTypeList { get; set; }
        public IEnumerable<SelectListItem>? BhkTypeList { get; set; }

        [Display(Name = "No of Bedrooms")]
        public int BedroomCount { get; set; }
        [Display(Name = "No of Bathrooms")]
        public int BathroomCount { get; set; }

        [Required(ErrorMessage ="Price is mandatory")]
        [Range(100,9999999, ErrorMessage ="Price should be between 100 and 99,99,999")]
        public int Price { get; set; }

        [Display(Name = "Images")]
        public List<IFormFile>? ImageUrl { get; set; }
    }
}
