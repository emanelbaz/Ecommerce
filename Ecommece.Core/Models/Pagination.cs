using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class Pagination
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        public string Sort { get; set; }
    }
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }

        public PagedResult(IReadOnlyList<T> data, int count, int pageIndex, int pageSize)
        {
            Data = data;
            Count = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
