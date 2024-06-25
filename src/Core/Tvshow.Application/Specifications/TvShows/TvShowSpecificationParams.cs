using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvshow.Application.Specifications.TvShows
{
    public class TvShowSpecificationParams : SpecificationParams
    {
        public int? ShowId { get; set; }
        public string Name { get; set; }
        public int Favorite { get; set; }
    }
}
