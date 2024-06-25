using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Commands.CreateTvShow;
using Tvshow.Application.Features.Tvshow.Commands.UpdateTvShow;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Domain;

namespace Tvshow.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo de TvShow a TvShowVm
            CreateMap<TvShow, TvShowVm>()
                .ForMember(destino => destino.Favorite, opt => opt.MapFrom(origen => origen.Favorite == true ? 1 : 0));

            // Mapeo de TvShowVm a TvShow
            CreateMap<TvShowVm, TvShow>()
                .ForMember(destino => destino.Favorite, opt => opt.MapFrom(origen => origen.Favorite == 1 ? true : false));

            // Mapeo de UpdateTvShowCommand a TvShow
            CreateMap<UpdateTvShowCommand, TvShow>();

            // Mapeo de CreateTvShowCommand a TvShow
            CreateMap<CreateTvShowCommand, TvShow>();
        }
    }
}
