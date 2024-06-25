using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Domain.Common;

namespace Tvshow.Domain
{
    public class TvShow : BaseDomainModel {
        public string Name { get; set; }
        public bool Favorite { get; set; }
    }
}
