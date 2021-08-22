using System;

namespace PortLaMontagne.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Article Article { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}