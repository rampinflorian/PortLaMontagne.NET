using System.Collections.Generic;

namespace PortLaMontagne.Models.Views
{
    public class ArticleDetailsView
    {
        public Article Article { get; set; }
        public List<Article> RecentArticles { get; set; }
    }
}