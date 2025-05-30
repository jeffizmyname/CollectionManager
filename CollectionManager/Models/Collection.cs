﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<CollectionItem> Items { get; set; }

        public Collection() { }

        public Collection(string name, string description)
        {
            Id = 0;
            Name = name;
            Description = description;
            Items = new List<CollectionItem>();
        }


        public string ToDisplayString()
        {
            return
            $@"Id: {Id}
            Name: {Name}
            Description: {Description}
            ";
        }
    }
}
