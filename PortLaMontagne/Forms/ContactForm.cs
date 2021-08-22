using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortLaMontagne.Forms
{
    [NotMapped]
    public class ContactForm
    {
        [Required] public string Username { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Subject { get; set; }
        [Required] public string Content { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
    }
}