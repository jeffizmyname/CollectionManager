using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Helpers;

namespace CollectionManager.Models
{
    public class CollectionItem
    {
        public int Id { get; set; }
        public string CollectionName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime? SellDate { get; set; }
        public int Rating { get; set; }
        public bool Sold { get; set; }
        public bool ToSale { get; set; }
        public string ImageName { get; set; }

        public CustomVariable customVariable { get; set; }

        public string ImagePath => System.IO.Path.Combine(FileSystem.AppDataDirectory, "data", "Images" ,ImageName);
        public string RatingString => $"{Rating}/10";
        public string CollectionDateString => $"{CollectionDate.Day}/{CollectionDate.Month}/{CollectionDate.Year}";
        public string SellDateString => SellDate.HasValue
           ? $"{SellDate.Value.Day}/{SellDate.Value.Month}/{SellDate.Value.Year}"
           : "Not Sold";

        public string SoldString => Sold ? "Yes" : "No";
        public string ToSaleString => ToSale ? "Yes" : "No";


        public string ToDisplayString()
        {
            return
            $@"Id: {Id}
    Collection: {CollectionName}
    Name: {Name}
    Description: {Description}
    Price: {Price} PLN
    Collected On: {CollectionDate:G}
    Sold On: {(SellDate.HasValue ? SellDate.Value.ToString("G") : "Not Sold")}
    Rating: {Rating}/10
    Sold: {(Sold ? "Yes" : "No")}
    To Sale: {(ToSale ? "Yes" : "No")}
    Image: {ImageName}
    Custom Variable: {customVariable.ToString()}";
        }

        private static bool IsDuplicate(CollectionItem a, CollectionItem b)
        {
            return a.CollectionName == b.CollectionName &&
                   a.Name == b.Name &&
                   a.Description == b.Description &&
                   a.Price == b.Price &&
                   a.customVariable.Name == b.customVariable.Name &&
                   a.customVariable.GetValue() == b.customVariable.GetValue();
        }

        public static bool HasDuplicate(List<CollectionItem> list, CollectionItem item)
        {
            return list.Any(existing => IsDuplicate(existing, item) && !existing.Sold);
        }
    }
}
