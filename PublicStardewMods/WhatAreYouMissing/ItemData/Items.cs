using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using SObject = StardewValley.Object;

namespace WhatAreYouMissing
{
    public abstract class Items
    {
        protected Dictionary<int, SObject> items;
        protected ModConfig Config;
        public enum FarmTypes
        {
            Normal = 0,
            River = 1,
            Forest = 2,
            Hilltop = 3,
            Wilderness = 4
        };

        abstract protected void AddItems();

        public Items()
        {
            Config = ModEntry.modConfig;
            items = new Dictionary<int, SObject>();
            AddItems();
        }

        protected void AddFish(int parentSheetIndex)
        {
            Dictionary<int, string> data = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
            switch (data[parentSheetIndex].Split('/')[7])
            {
                case "sunny":
                    if (!Game1.isRaining || Config.ShowAllFishFromCurrentSeason)
                    {
                        AddOneCommonObject(parentSheetIndex);
                    }
                    break;
                case "rainy":
                    if (Game1.isRaining || Config.ShowAllFishFromCurrentSeason)
                    {
                        AddOneCommonObject(parentSheetIndex);
                    }
                    break;
                case "both":
                    AddOneCommonObject(parentSheetIndex);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Adds a crop if there is sufficient time to grow it before the 
        /// season ends.
        /// </summary>
        /// <param name="key"></param>
        protected void ManuallyAddCrop(int key)
        {
            CropConversion cropConverter = new CropConversion();
            if (Utilities.IsThereEnoughTimeToGrowSeeds(cropConverter.CropToSeedIndex(key)))
            {
                AddOneCommonObject(key);
            }
        }

        protected void AddOneCommonObject(int parentSheetIndex)
        {
            AddCommonObject(parentSheetIndex, 1);
        }

        protected void AddCommonObject(int parentSheetIndex, int stackSize)
        {
            if (!items.ContainsKey(parentSheetIndex))
            {
                items.Add(parentSheetIndex, new SObject(parentSheetIndex, stackSize));
            }
        }

        private enum SeasonIndex
        {
            Spring = 4,
            Summer = 5,
            Fall = 6,
            Winter = 7
        };

        private int SeasonNameToIndex(string season)
        {
            switch (season)
            {
                case "spring":
                    return (int)SeasonIndex.Spring;
                case "summer":
                    return (int)SeasonIndex.Summer;
                case "fall":
                    return (int)SeasonIndex.Fall;
                case "winter":
                    return (int)SeasonIndex.Winter;
                default:
                    return -1;
            }
        }

        protected void AddNormalSeasonalFish(string season)
        {
            Dictionary<string, string> LocationData = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
            int seasonIndex = SeasonNameToIndex(season);

            foreach (KeyValuePair<string, string> data in LocationData)
            {
                if (!Utilities.IsTempOrFishingGameOrBackwoodsLocation(data.Key))
                {
                    string[] seasonalFish = data.Value.Split('/')[seasonIndex].Split(' ');
                    for (int i = 0; i < seasonalFish.Length; ++i)
                    {
                        if (i % 2 == 0)
                        {
                            //Its a parent sheet index
                            int parentSheetIndex = int.Parse(seasonalFish[i]);

                            //I want to add them manually, -1 means no fish at this location
                            if (IsNormalFish(parentSheetIndex) && NotInAllSeasons(parentSheetIndex))
                            {
                                AddFish(parentSheetIndex);
                            }
                        }
                    }
                }
            }
        }

        protected void AddAllFish()
        {
            Dictionary<string, string> LocationData = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");

            foreach (KeyValuePair<string, string> data in LocationData)
            {
                for (int season = (int)SeasonIndex.Spring; !Utilities.IsTempOrFishingGameOrBackwoodsLocation(data.Key) && season < (int)SeasonIndex.Winter + 1; ++season)
                {
                    string[] seasonalFish = data.Value.Split('/')[season].Split(' ');
                    for (int i = 0; i < seasonalFish.Length; ++i)
                    {
                        if (i % 2 == 0)
                        {
                            //Its a parent sheet index
                            int parentSheetIndex = int.Parse(seasonalFish[i]);

                            //I want to add them manually, -1 means no fish at this location
                            if (IsAFish(parentSheetIndex))
                            {
                                AddOneCommonObject(parentSheetIndex);
                            }
                        }
                    }
                }
            }
        }

        private bool IsNormalFish(int parentSheetIndex)
        {
            bool isAFish = IsAFish(parentSheetIndex);
            Constants constants = new Constants();
            return !constants.LEGENDARY_FISH.Contains(parentSheetIndex) && isAFish;
        }

        private bool IsAFish(int parentSheetIndex)
        {
            //Sometimes a mod can put the info into location data but not edit fish data
            //or object info so the mod it is meant to support doesn't exist
            //on this machine. Just double check
            if (parentSheetIndex == -1 || !Game1.objectInformation.ContainsKey(parentSheetIndex))
            {
                return false;
            }

            int category = -1;
            bool isAFish = true;

            string[] typeAndCategory = Game1.objectInformation[parentSheetIndex].Split('/')[3].Split(' ');
            if (typeAndCategory.Length > 1)
            {
                category = int.Parse(typeAndCategory[1]);
            }
            else
            {
                //Things like Algae don't have the category -4 (fish category)
                //they only have the word Fish
                //i.e Fish vs Fish -4
                isAFish = false;
            }

            return isAFish && category != SObject.junkCategory;
        }

        private bool NotInAllSeasons(int parentSheetIndex)
        {
            List<FishInfo> fishInfo = new FishDisplayInfo(parentSheetIndex).GetFishInfoList();

            foreach(FishInfo info in fishInfo)
            {
                if(info.GetSeasons().Count != 4)
                {
                    return true; 
                }
            }

            return false;
        }

        protected void AddCrops(string season)
        {
            Dictionary<int, string> cropData = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
            Constants constants = new Constants();
            foreach(KeyValuePair<int, string> data in cropData)
            {
                string[] crop = data.Value.Split('/');
                string[] seasons = crop[1].Split(' ');
                //Don't add it if its common to all seasons
                if(seasons.Length != 4)
                {
                    if (seasons.Contains(season) && Game1.currentSeason == season && !constants.SPECIAL_SEEDS.Contains(data.Key))
                    {
                        if (Utilities.IsThereEnoughTimeToGrowSeeds(data.Key))
                        {
                            AddOneCommonObject(int.Parse(crop[3]));
                        }
                    }
                    else if (seasons.Contains(season) && Game1.currentSeason != season && !constants.SPECIAL_SEEDS.Contains(data.Key))
                    {
                        //AddCrop checks to see if you can grow it
                        //this adds out of season items to their
                        //season items which is used by the 
                        //travelling merchant check
                        AddOneCommonObject(int.Parse(crop[3]));
                    }
                }
            }
        }

        protected void AddFruitTrees(string season)
        {
            Dictionary<int, string> fruitTreesData = Game1.content.Load<Dictionary<int, string>>("Data\\fruitTrees");
            foreach (KeyValuePair<int, string> data in fruitTreesData)
            {
                string[] fruitTree = data.Value.Split('/');
                if (fruitTree[1] == season)
                {
                    AddOneCommonObject(int.Parse(fruitTree[2]));
                }
            }
        }
    }
}
