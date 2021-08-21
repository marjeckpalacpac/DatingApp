using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        //this properties will gonna return to client along with the list of users
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; } //total count of items

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items); // noteExperiment
        }

        //retun a new instance of a PagedList and need to pass its parameters
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //in the source is going to be a list of Users or whatever else it is that passed to this source parameter
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //when this method called it return new PagedList passed the info in the parameter
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }



        //note: to use this PagedList class ==> call the CreateAsync with parameters ==> process the info 
        //and return new instace of Pagedlist ==> inside the constructor of PagedList will assign the properties with its value from its parameter.
        //the properties can now be use by the client
    }
}