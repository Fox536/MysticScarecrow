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
			int timePassed = 0;
			if (e.NewInt - e.PriorInt == 100)
				timePassed = 60;
			else
				timePassed = e.NewInt - e.PriorInt;

			foreach (GameLocation location in Game1.locations)
			{
				itterateMachines(location, false, timePassed);
			}

			// Bee Hive Coding
			foreach (StardewValley.Buildings.Building bLocation in Game1.getFarm().buildings)
			{
				itterateMachines(bLocation.indoors, false, timePassed);
				continue;

				GameLocation location = bLocation.indoors;
				foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
				{
					StardewValley.Object machine = item.Value;
					if (machine.name == "Bee House")
					{
						// Allow beehive to function in Greenhouse
						if (bLocation.buildingType == "Shed")
							machine.minutesUntilReady = Math.Max(machine.minutesUntilReady - timePassed, 0);
					}
					else if (machine.name == "Temporal Device")
					{
						TemporalIncreaser(location, machine, timePassed);
					}
				}	
			}
		}
		private void itterateMachines(GameLocation location, bool isBuildableLocation, int timePassed)
		{
			if (location == null || location.objects == null)
				return;
			foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
			{
				StardewValley.Object machine = item.Value;
				if (item.Value == null)
					continue;
				if (item.Value.name == "Bee House")
				{
					// Allow beehive to function in Greenhouse
					if (location.Name == "Greenhouse" || location.Name.Contains("Shed"))
						item.Value.minutesUntilReady = Math.Max(machine.minutesUntilReady - timePassed, 0);
				}
				else if (item.Value.name == "Temporal Crystal")
				{
					TemporalIncreaser(location, machine, timePassed);
				}
				else if (item.Value.name == "Large Growing Tree")
				{
					if (item.Value.readyForHarvest)
						if (item.Value.heldObject != null)
							print("Machine holds: " + machine.heldObject.name);
				}
			}
		}

		private void TemporalIncreaser(GameLocation location, StardewValley.Object machine, int timePassed)
		{
			StardewValley.Object affectedMachine;

			Vector2 centerPoint = machine.TileLocation;
			int width = 5;
			int halfSize = width / 2;
			float modifier = 0.5f;

			Vector2 currentPoint = centerPoint;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < width; y++)
				{
					currentPoint.X = centerPoint.X - halfSize + x;
					currentPoint.Y = centerPoint.Y - halfSize + y;

					if (location.objects.ContainsKey(currentPoint))
					{
						affectedMachine = location.objects[currentPoint];
						affectedMachine.minutesUntilReady = Math.Max(affectedMachine.minutesUntilReady - (int)Math.Floor(timePassed * modifier), 0);
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
