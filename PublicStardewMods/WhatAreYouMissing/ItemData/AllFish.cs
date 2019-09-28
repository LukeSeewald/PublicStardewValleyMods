using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SObject = StardewValley.Object;

namespace WhatAreYouMissing
{
    public class AllFish : Items, ISpecificItems
    {
        public AllFish() : base() { }

        protected override void AddItems()
        {
            AddAllFish();

            AddOneCommonObject(Constants.CRIMSONFISH);
            AddOneCommonObject(Constants.ANGLER);
            AddOneCommonObject(Constants.GLACIERFISH);
            AddOneCommonObject(Constants.MUTANT_CARP);

            AddOneCommonObject(Constants.MIDNIGHT_SQUID);
            AddOneCommonObject(Constants.SPOOK_FISH);
            AddOneCommonObject(Constants.BLOBFISH);

            AddOneCommonObject(Constants.STONEFISH);
            AddOneCommonObject(Constants.ICE_PIP);
            AddOneCommonObject(Constants.LAVA_EEL);
        }

        public Dictionary<int, SObject> GetItems()
        {
            return items;
        }
    }
}
