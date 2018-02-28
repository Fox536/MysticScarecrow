using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TerrainFeatures;
using c = Fox536.Machines.Config;

namespace Fox536.Machines
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		internal c.ModConfig config;

		const string BeeHiveName = "Bee House";

		//-------------------------
		// Public Methods
		//-------------------------
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			TimeEvents.AfterDayStarted  += TimeEvents_AfterDayStarted;
			TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;

			config = helper.ReadConfig<Config.ModConfig>();
		}

		//-------------------------/
		#region - Events
		//-------------------------/
		private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
		{
			int timePassed = 0;
			if (e.NewInt - e.PriorInt == 100)
				timePassed = 60;
			else
				timePassed = e.NewInt - e.PriorInt;

			foreach (GameLocation location in Game1.locations)
			{
				ItterateMachines(location, false, timePassed);
			}

			// Bee Hive Coding
			foreach (StardewValley.Buildings.Building bLocation in Game1.getFarm().buildings)
			{
				ItterateMachines(bLocation.indoors, false, timePassed);
			}
		}
		private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
		{
			foreach (GameLocation location in Game1.locations)
			{
				foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
				{
					Config.SprinklerScarecrow sprinkler = config.SprinklerScarecrowConfig.Find(x => x.Name == item.Value.name);
					if (sprinkler != null)
					{
						WaterScareCrowArea(location, item.Key, sprinkler);
					}
				}
			}
		}
		#endregion
		//-------------------------/

		//-------------------------/
		#region Private Methods
		//-------------------------/
		private void WaterScareCrowArea(GameLocation location, Vector2 centerPoint, Config.SprinklerScarecrow sprinkler)
		{
			Vector2 currentPoint = new Vector2(centerPoint.X, centerPoint.Y);

			int width = sprinkler.Width;
			int halfWidth = ((width - 1) / 2);

			// Main Section
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < width; y++)
				{
					currentPoint.X = centerPoint.X - halfWidth + x;
					currentPoint.Y = centerPoint.Y - halfWidth + y;
					if (location.terrainFeatures.ContainsKey(currentPoint))
					{
						if (location.terrainFeatures[currentPoint] is HoeDirt)
						{
							(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
						}
					}
				}
			}

			// West Section 1
			for (int y = 0; y < width - 2; y++)
			{
				currentPoint.X = centerPoint.X - halfWidth - 1;
				currentPoint.Y = centerPoint.Y - halfWidth + 1 + y;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}
			for (int y = 0; y < width - 4; y++)
			{
				currentPoint.X = centerPoint.X - halfWidth - 2;
				currentPoint.Y = centerPoint.Y - halfWidth + 2 + y;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}
			// East Section 1
			for (int y = 0; y < width - 2; y++)
			{
				currentPoint.X = centerPoint.X + halfWidth + 1;
				currentPoint.Y = centerPoint.Y - halfWidth + 1 + y;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}
			for (int y = 0; y < width - 4; y++)
			{
				currentPoint.X = centerPoint.X + halfWidth + 2;
				currentPoint.Y = centerPoint.Y - halfWidth + 2 + y;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}

			// North Section 1
			for (int x = 0; x < width - 2; x++)
			{
				currentPoint.X = centerPoint.X - halfWidth + 1 + x;
				currentPoint.Y = centerPoint.Y - halfWidth - 1;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}
			for (int x = 0; x < width - 4; x++)
			{
				currentPoint.X = centerPoint.X - halfWidth + 2 + x;
				currentPoint.Y = centerPoint.Y - halfWidth - 2;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}

			// Sout Section 1
			for (int x = 0; x < width - 2; x++)
			{
				currentPoint.X = centerPoint.X - halfWidth + 1 + x;
				currentPoint.Y = centerPoint.Y + halfWidth + 1;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}
			for (int x = 0; x < width - 4; x++)
			{
				currentPoint.X = centerPoint.X - halfWidth + 2 + x;
				currentPoint.Y = centerPoint.Y + halfWidth + 2;
				if (location.terrainFeatures.ContainsKey(currentPoint))
				{
					if (location.terrainFeatures[currentPoint] is HoeDirt)
					{
						(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
					}
				}
			}
		}
		private void SquareSprinkler(Vector2 centerPoint, GameLocation location, int width)
		{
			Vector2 currentPoint = new Vector2(centerPoint.X, centerPoint.Y);
			int halfWidth = ((width - 1) / 2);

			// Main Section
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < width; y++)
				{
					currentPoint.X = centerPoint.X - halfWidth + x;
					currentPoint.Y = centerPoint.Y - halfWidth + y;
					if (location.terrainFeatures.ContainsKey(currentPoint))
					{
						if (location.terrainFeatures[currentPoint] is HoeDirt)
						{
							(location.terrainFeatures[currentPoint] as HoeDirt).state = 1;
						}
					}
				}
			}
		}

		private void ItterateMachines(GameLocation location, bool isBuildableLocation, int timePassed)
		{
			if (location == null || location.objects == null)
				return;
			foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
			{
				StardewValley.Object machine = item.Value;
				if (item.Value == null)
					continue;


				// Make Beehive work indoors
				if (item.Value.name == BeeHiveName)
				{
					UpdateBeehive(location, machine, timePassed);
					continue;
				}
				
				// Machine Speed Boost
				c.SpeedIncreaser speedIncreaser = config.SpeedIncreaserConfig.Find(x => x.Name == machine.Name);
				if (speedIncreaser != null)
				{
					TemporalIncreaser(location, speedIncreaser, machine, timePassed);
					continue;
				}

				if (machine.name == "Large Growing Tree")
				{
					if (machine.readyForHarvest)
						if (machine.heldObject != null)
							print("Machine holds: " + machine.heldObject.name);
				}
			}
		}

		private void UpdateBeehive(GameLocation location, StardewValley.Object machine, int timePassed)
		{
			// Allow beehive to function in Greenhouse
			if (location.Name == "Greenhouse" || location.Name.Contains("Shed"))
				machine.minutesUntilReady = Math.Max(machine.minutesUntilReady - timePassed, 0);
		}
		private void TemporalIncreaser(GameLocation location, c.SpeedIncreaser speedIncreaser, StardewValley.Object machine, int timePassed)
		{
			StardewValley.Object affectedMachine;

			Vector2 centerPoint = machine.TileLocation;
			int width = speedIncreaser.Width;
			int halfSize = width / 2;
			float modifier = (speedIncreaser.SpeedBuff / 100);

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
		#endregion
		//-------------------------/

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
