using System;
using System.ComponentModel.DataAnnotations;
using Slugify;

namespace PortLaMontagne.Models
{
    public class MarketProduct
    {
        public int MarketProductId { get; set; }
        [Required, Display(Name = "Titre")] public string Title { get; set; }
        [Required, Display(Name = "Description")] public string Description { get; set; }
        [Required, Display(Name = "Prix")] public decimal Price { get; set; }
        public bool IsSold { get; set; }
        public bool IsSuspended { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        public DateTime? SoldAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required] public ApplicationUser Vendor { get; set; }
        public ApplicationUser Buyer { get; set; }
        [Required] public string Image1 { get; set; }
        public string Image2 { get; set; }
        private string _slug;
        
        public string Slug
        {
            get => _slug;
            private set
            {
                var helper = new SlugHelper();
                _slug = helper.GenerateSlug(value);
            }
        }

        public MarketProduct()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        
    }
}