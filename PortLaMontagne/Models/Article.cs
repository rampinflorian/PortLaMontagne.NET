using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using PortLaMontagne.Enums;
using Slugify;

namespace PortLaMontagne.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        private string _title;

        [Required(ErrorMessage = "Le titre est obligatoire")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Slug = value;
            }
        }

        [Required(ErrorMessage = "Le contenu est obligatoire")]
        [Display(Name = "Contenu de l'article")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public ApplicationUser Editor { get; set; }
        
        public string Image { set;get; }
        [NotMapped]
        public IFormFile FormFile { set; get; }
        public bool IsPublished { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public CategoryEnum Category { get; set; }

        private string _slug;


        public string Slug
        {
            get => _slug;
            set
            {
                var helper = new SlugHelper();
                _slug = helper.GenerateSlug(value);
            }
        }

        public Article()
        {
            CreatedAt = DateTime.Now;
        }
    }
}