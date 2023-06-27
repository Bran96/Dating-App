using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DatingAppApi.Helpers
{
    // We want our PagedList to work with any type of object or class and we do this by giving this class a generic type, this is a generic type: "<T>"
    // And then we gonna tell it what "T" will be replaced with and in this case it will be "MemberDto" model for an example, but we just gonna stick to "T"
    // and then we gonna derive from a list also from "T"
    // Inside our PagedList class we are going to create a static method so that we can call our PagedList
    public class PagedList<T>: List<T>
    {
        // It populates the arguments in the constructr with our properties
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            // So when we create a new instance of this PagedList, we can expect to receive all these different properties back from it and we will be able to get access to the items of the pagedList from the list itself
            CurrentPage = pageNumber;
            // The TotalPages will depend on the count and the pageSize in the constructor arguments
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            PageSize = pageSize;
            TotalCount = count;
            // When we return this PagedList, we gonna return it with a list of our items
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // We will be using this method inside our repositories
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); // This is gonna give us the total count of the items from our query, which is basically all the items in the list or in the db table
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); // This is where the pagination takes place. This is where we gonna get our items based on the skip and take methods
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
