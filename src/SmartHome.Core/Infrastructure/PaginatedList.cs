//using Matty.Framework.Enums;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SmartHome.Core.Infrastructure
//{
//    public class PaginatedList<T> : List<T>
//    {
//        public int PageIndex { get; private set; }
//        public int TotalPages { get; private set; }
//        public int TotalCount { get; private set; }

//        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
//        {
//            TotalCount = count;
//            PageIndex = pageIndex;
//            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

//            this.AddRange(items);
//        }

//        public bool HasPreviousPage => (PageIndex > 1);
//        public bool HasNextPage => (PageIndex < TotalPages);

//        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source,
//                                                               Func<IQueryable<T>, DataOrder, IQueryable<T>> filterDelegate,
//                                                               int pageIndex, int pageSize, DataOrder order)
//        {
//            var count = await source.CountAsync();
//            var items = filterDelegate(source, DataOrder.Desc)
//                .Skip((pageIndex - 1) * pageSize)
//                .Take(pageSize);

//            items = filterDelegate(items, order);
//            var list = await items.ToListAsync();

//            return new PaginatedList<T>(list, count, pageIndex, pageSize);
//        }
//    }
//}
