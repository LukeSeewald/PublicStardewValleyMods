using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SObject = StardewValley.Object;

namespace WhatAreYouMissing
{
    public class AllCrops : Items, ISpecificItems
    {
        public AllCrops() : base()
        {
            AddItems();
        }

        protected override void AddItems()
        {
            AddAllCropsAndSaplings();
        }

        public Dictionary<int, SObject> GetItems()
        {
            return items;
        }
    }
}
