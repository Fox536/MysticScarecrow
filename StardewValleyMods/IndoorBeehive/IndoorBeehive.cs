using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace Fox536.IndoorBeehive
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
		}

		private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
		{
			int timePassed = e.NewInt - e.PriorInt;
			//print("timeOfDayChanged");
			foreach (StardewValley.Buildings.Building bLocation in Game1.getFarm().buildings)
			{
				GameLocation location = bLocation.indoors;
				if (bLocation.buildingType == "Shed")
				{
					//print("farm building: " + location.isFarmBuildingInterior());
					//print("IsOutdoors: " + location.IsOutdoors);
					//print("IsFarm: " + location.IsFarm);
					location.IsFarm = true;

					foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
					{
						StardewValley.Object machine = item.Value;
						print(machine.name);
						if (machine.name == "Bee House")
						{
							StardewValley.Object beehive = item.Value;
							beehive.minutesUntilReady = Math.Max(beehive.minutesUntilReady - timePassed, 0);
							//beehive.honeyType

						}
					}
				}
			}
		}
		/// <summary>
		/// Debug Print, requires modConfig.debug to equal true.
		/// </summary>
		/// <param name="text">Text to print.</param>
		private void print(string text)
		{
			this.Monitor.Log(text);
		}
	}
}
