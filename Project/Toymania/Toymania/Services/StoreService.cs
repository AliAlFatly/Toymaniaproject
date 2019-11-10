using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toymania.Models;

namespace Toymania.Services
{
    public class StoreService
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<Toy> GetProducts(string category, string subCategory, int? minPrice, int? maxPrice, string search)
        {

            //this method is extremly slow
            //var MaxPrice = maxPrice ?? (from ToysTable in db.Toy select ToysTable.Price).Max();
            //var MinPrice = minPrice ?? 0;
            //var result = (from ToysTable in db.Toy
            //            where category != null ? ToysTable.Categories.CategoryName == category : true
            //                && subCategory != null ? ToysTable.SubCategories.SubCategoryName == subCategory : true
            //                && ToysTable.Price > MinPrice && ToysTable.Price < MaxPrice
            //            select ToysTable).ToList();

            var result = db.Toy.AsQueryable();
            if (!string.IsNullOrEmpty(category))
                result = result.Where(x => x.Categories.CategoryName == category);
            if (!string.IsNullOrEmpty(subCategory))
                result = result.Where(x => x.SubCategories.SubCategoryName == subCategory);
            if (minPrice.HasValue)
                result = result.Where(x => x.Price > minPrice);
            if (maxPrice.HasValue)
                result = result.Where(x => x.Price < maxPrice);
            if (!string.IsNullOrEmpty(search))
                result = result.Where(x => x.ToysName.Contains(search));


            return result.ToList();
        }

    }
}