
using EntityLayer.Entities;

namespace DataLayer.ViewModels
{
    public class DaynamicSiteViewModel
    {
        public List<Book> LastBook { get; set; }
        public List<News> LastNews { get; set; }
        public ApplicationUsers UserName { get; set; }

    }
}
