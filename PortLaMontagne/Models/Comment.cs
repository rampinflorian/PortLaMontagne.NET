using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortLaMontagne.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tu dois saisir un commentaire !")]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Article Article { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
    }
}