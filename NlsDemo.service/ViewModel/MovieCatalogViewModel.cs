using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.service.ViewModel
{
    public class MovieCatalogViewModel
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string CatalogImage { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
