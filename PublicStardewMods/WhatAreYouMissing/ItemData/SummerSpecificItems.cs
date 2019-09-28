using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using SObject = StardewValley.Object;

namespace WhatAreYouMissing
{
    public class SummerSpecificItems : Items, ISpecificItems
    {
        public SummerSpecificItems() : base() { }

        public Dictionary<int, SObject> GetItems()
        {
            return items;
        }

        protected override void AddItems()
        {
            AddCrops();
            AddForagables();
            //AddFruitTrees();
            AddFruitTrees("summer");
            AddFish();
        }

        private void AddCrops()
        {
            //AddCrop(Constants.MELON);
            //AddCrop(Constants.POPPY);
            //AddCrop(Constants.RADISH);
            //AddCrop(Constants.SUMMER_SPANGLE);
            //AddCrop(Constants.SUNFLOWER);
            //AddCrop(Constants.WHEAT);
            //AddCrop(Constants.BLUEBERRY);
            //AddCrop(Constants.CORN);
            //AddCrop(Constants.HOPS);
            //AddCrop(Constants.HOT_PEPPER);
            //AddCrop(Constants.TOMATO);
            ////coffee beans can only come from travelling cart or mines
            //AddCrop(Constants.COFFEE_BEAN);

            AddCrops("summer");

            if (Utilities.IsMerchantAvailiableAndHasItem(Constants.RED_CABBAGE))
            {
                AddOneCommonObject(Constants.RED_CABBAGE);
            }
            else if (Game1.Date.Year > 1 || Utilities.IsMerchantAvailiableAndHasItem(Constants.RED_CABBAGE_SEEDS))
            {
                ManuallyAddCrop(Constants.RED_CABBAGE);
            }

            if (Utilities.IsMerchantAvailiableAndHasItem(Constants.STARFRUIT))
            {
                AddOneCommonObject(Constants.STARFRUIT);
            }
            else if (Utilities.IsDesertUnlocked() || Config.ShowItemsFromLockedPlaces || Utilities.IsMerchantAvailiableAndHasItem(Constants.STARFRUIT_SEEDS))
            {
                ManuallyAddCrop(Constants.STARFRUIT);
            }
        }

        private void AddForagables()
        {
            AddOneCommonObject(Constants.RAINBOW_SHELL);
            AddOneCommonObject(Constants.SPICE_BERRY);
            AddOneCommonObject(Constants.GRAPE);
            AddOneCommonObject(Constants.SWEET_PEA);
            AddOneCommonObject(Constants.TRUFFLE);
            //farm 4 is wilderness, 3 is hilltop, 2 is forest, 1 is river, 0 is normal
            if (Game1.whichFarm == (int)FarmTypes.Forest)
            {
                AddOneCommonObject(Constants.COMMON_MUSHROOM);
            }

            if(Utilities.IsSecretWoodsUnlocked() || Config.ShowItemsFromLockedPlaces)
            {
                AddOneCommonObject(Constants.FIDDLEHEAD_FERN);
            }
        }

        private void AddFruitTrees()
        {
            //AddOneCommonObject(Constants.APPLE);
            //AddOneCommonObject(Constants.POMEGRANATE);
        }

        private void AddFish()
        {
            AddNormalSeasonalFish("summer");

            if (Config.ShowAllFishFromCurrentSeason || (Game1.player.getEffectiveSkillLevel(1) > 4 && !Game1.player.fishCaught.ContainsKey(Constants.CRIMSONFISH)))
            {
                AddFish(Constants.CRIMSONFISH);
            }
        }
    }
}
