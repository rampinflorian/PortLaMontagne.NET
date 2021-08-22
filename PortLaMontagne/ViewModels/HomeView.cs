using System.Collections.Generic;

namespace PortLaMontagne.Models.Views
{
    public class HomeView
    {
        public List<Partner> Partner { get; set; }
        public List<Article> Articles { get; set; }
        public int CountArticles { get; set; }
    }
}