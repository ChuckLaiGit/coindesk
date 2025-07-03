using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Utiity
{
    public static class Mapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source, Action<IMappingExpression<TSource, TTarget>>? expressionFunc = null)
        {
            MapperConfiguration config = new(cfg =>
            {
                IMappingExpression<TSource, TTarget>? map = cfg.CreateMap<TSource, TTarget>();
                expressionFunc?.Invoke(map);
            });
            IMapper mapper = config.CreateMapper();
            TTarget response = mapper.Map<TSource, TTarget>(source);
            return response;
        }
    }
}
