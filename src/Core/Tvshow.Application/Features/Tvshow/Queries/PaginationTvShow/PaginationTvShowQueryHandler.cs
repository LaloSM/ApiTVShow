using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Persistence;
using Tvshow.Application.Shared.Queries;
using Tvshow.Application.Specifications.TvShows;
using Tvshow.Domain;

namespace Tvshow.Application.Features.Tvshow.Queries.PaginationTvShow
{
    public class PaginationTvShowQueryHandler : IRequestHandler<PaginationTvShowQuery, PaginationVm<TvShowVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaginationTvShowQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<TvShowVm>> Handle(PaginationTvShowQuery request, CancellationToken cancellationToken)
        {
            var tvshowSpecificationParams = new TvShowSpecificationParams
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
                Name = request.Name,
                Favorite = request.Favorite
            };

            var spec = new TvShowSpecification(tvshowSpecificationParams);
            var shows = await _unitOfWork.Repository<TvShow>().GetAllWithSpec(spec);

            //calcula el total de records
            var specCount = new TvShowForCountingSpecification(tvshowSpecificationParams);
            var totalTvShows = await _unitOfWork.Repository<TvShow>().CountAsync(specCount);

            //arma el objeto paginacion
            var rounded = Math.Ceiling(Convert.ToDecimal(totalTvShows) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<TvShowVm>>(shows);
            var tvshowsByPage = shows.Count();

            //aqui regresa el objeto pagination
            var pagination = new PaginationVm<TvShowVm>
            {
                Count = totalTvShows,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ResultByPage = tvshowsByPage
            };

            return pagination;
        }
    }
}
