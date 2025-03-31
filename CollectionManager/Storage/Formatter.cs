using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Models;

namespace CollectionManager.Storage
{
    public static class Formatter
    {
        public static string SerializeCollection(Collection collection)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Collection]");
            sb.AppendLine($"Id={collection.Id}");
            sb.AppendLine($"Name={collection.Name}");
            sb.AppendLine($"Description={collection.Description}");
            sb.AppendLine("[CollectionEnd]");

        }

        public static string SerializeCollectionItems(Collection collection)
        {
            collection.Items.ForEach(item => {

            });
        }

        public static string SerializeCollectionItem(CollectionItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Item]");
            sb.AppendLine($"Id={item.Id}");
            sb.AppendLine($"Name={item.Name}");
            sb.AppendLine($"Description={item.Description}");
            sb.AppendLine($"Price={item.Price}");
            sb.AppendLine($"CollectionDate={item.CollectionDate}");
            sb.AppendLine($"SellDate={item.SellDate}");
            sb.AppendLine($"Rating={item.Rating}");
            sb.AppendLine($"Sold={item.Sold}");
            sb.AppendLine($"ToSale={item.ToSale}");
            sb.AppendLine($"ImageName={item.ImageName}");
            sb.AppendLine($"CustomVariable={item.customVariable}");
            sb.AppendLine("[ItemEnd]");
            return sb.ToString();
        }
    }
}
