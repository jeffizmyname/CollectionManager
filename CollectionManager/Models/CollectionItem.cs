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
        public DateTime SellDate { get; set; }
        public int Rating { get; set; }
        public bool Sold { get; set; }
        public bool ToSale { get; set; }
        public string ImageName { get; set; }

        public CustomVariable customVariable { get; set; }


    }
}
