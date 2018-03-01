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
			config = helper.ReadConfig<Config.ModConfig>();

			TimeEvents.AfterDayStarted  += TimeEvents_AfterDayStarted;
			TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
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

			try
			{
				foreach (GameLocation location in Game1.locations)
				{
					ItterateMachines(location, false, timePassed);
				}

				foreach (StardewValley.Buildings.Building bLocation in Game1.getFarm().buildings)
				{
					ItterateMachines(bLocation.indoors, false, timePassed);
				}
			} catch (Exception ex)
			{
				this.Monitor.Log("Fox ERROR: " + ex.Message, LogLevel.Error);
				this.Monitor.Log("Fox ERROR: " + ex.Source, LogLevel.Error);
			}
		}
		private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
		{
			foreach (GameLocation location in Game1.locations)
			{
				foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
				{
					c.SprinklerScarecrow sprinkler = config.SprinklerScarecrowConfig.Find(x => x.Name == item.Value.name);
					if (sprinkler != null)
					{
						WaterScareCrowArea(location, item.Key, sprinkler);
					}

					if (config.ChangeSprinklers)
					{
						c.SprinklerArea sprinklerArea = config.SprinklerAreaChanges.Find(x => x.Name == item.Value.name);
						if (sprinklerArea != null)
						{
							WaterSprinklerArea(location, item.Key, sprinklerArea);
						}
					}
				}
			}
		}

		
		#endregion
		//-------------------------/

		//-------------------------/
		#region Private Methods
		//-------------------------/
		private void WaterScareCrowArea(GameLocation location, Vector2 centerPoint, c.SprinklerScarecrow sprinkler)
		{
			WaterAreaCircle(location, centerPoint, sprinkler.Width);
			return;

			Vector2 currentPoint = centerPoint;// new Vector2(centerPoint.X, centerPoint.Y);

			int width = sprinkler.Width;
			int halfWidth = ((width - 1) / 2);

			// Main Section
			WaterAreaSquare(location, centerPoint, 6);

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
		private void WaterSprinklerArea(GameLocation location, Vector2 centerPoint, c.SprinklerArea sprinklerArea)
		{
			if (sprinklerArea.AreaType.ToLower() == "basic")
			{
				List<Vector2> area = new List<Vector2>();
				area = Utility.getAdjacentTileLocations(centerPoint);
				WaterArea(location, area);
			}
			else if (sprinklerArea.AreaType.ToLower() == "custom")
			{
				List<Vector2> oldArea = new List<Vector2>();
				List<Vector2> area = new List<Vector2>();

				// West
				Vector2 currentPoint = new Vector2(centerPoint.X, centerPoint.Y);
				for (int x = 0; x < sprinklerArea.CustomEast; x++)
				{
					currentPoint = new Vector2();
					currentPoint.X = centerPoint.X - 1 - x;
					currentPoint.Y = centerPoint.Y;
					area.Add(currentPoint);
				}
				// East
				for (int x = 0; x < sprinklerArea.CustomEast; x++)
				{
					currentPoint = new Vector2();
					currentPoint.X = centerPoint.X + 1 + x;
					currentPoint.Y = centerPoint.Y;
					area.Add(currentPoint);
				}
				// North
				for (int y = 0; y < sprinklerArea.CustomNorth; y++)
				{
					currentPoint = new Vector2();
					currentPoint.X = centerPoint.X;
					currentPoint.Y = centerPoint.Y - 1 - y;
					area.Add(currentPoint);
				}
				// South
				for (int y = 0; y < sprinklerArea.CustomSouth; y++)
				{
					currentPoint = new Vector2();
					currentPoint.X = centerPoint.X;
					currentPoint.Y = centerPoint.Y + 1 + y;
					area.Add(currentPoint);
				}

				oldArea = Utility.getAdjacentTileLocations(centerPoint);
				WaterArea(location, oldArea, true);
				WaterArea(location, area);
			}
			else if (sprinklerArea.AreaType.ToLower() == "square")
			{
				WaterAreaSquare(location, centerPoint, sprinklerArea.SquareWidth);
			}
		}

		private void WaterAreaSquare(GameLocation location, Vector2 centerPoint, int width)
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
		private void WaterArea(GameLocation location, List<Vector2> area, bool dry = false)
		{
			int state = 1;
			if (dry) { state = 0; }
			foreach (var point in area)
			{
				if (location.terrainFeatures.ContainsKey(point))
				{
					if (location.terrainFeatures[point] is HoeDirt)
					{
						(location.terrainFeatures[point] as HoeDirt).state = state;
					}
				}
			}
		}
		private void WaterAreaCircle(GameLocation location, Vector2 centerPoint, int width)
		{
			int fullWidth = width * 2 + 1;
			int taper = 4;
			
			// Get the area for the Sprinkler
			List<Vector2> area = new List<Vector2>();
			Vector2 point = new Vector2();
			// Main Center Square
			for (int x = 0; x < fullWidth - taper; x++)
			{
				for (int y = 0; y < fullWidth - taper; y++)
				{
					point = new Vector2();
					point.X = centerPoint.X - width + x + taper / 2;
					point.Y = centerPoint.Y - width + y + taper / 2;
					area.Add(point);
				}
			}



			int moddedWidth = fullWidth - taper - 2;

			int offset = 0;
			// West Section
			for (int x = 1; x < taper / 2 + 1; x++)
			{
				for (int y = 0; y < moddedWidth; y++)
				{
					if (y > (offset - 1) && y < moddedWidth - offset)
					{
						point = new Vector2();
						point.X = centerPoint.X - (width - taper / 2) - x;
						point.Y = centerPoint.Y - (moddedWidth - 1) / 2 + y;
						area.Add(point);
					}
				}
				offset = 1;
			}

			offset = 0;
			// East Section
			for (int x = 1; x < taper / 2 + 1; x++)
			{
				for (int y = 0; y < moddedWidth; y++)
				{
					if (y > (offset - 1) && y < moddedWidth - offset)
					{
						point = new Vector2();
						point.X = centerPoint.X + (width - taper / 2) + x;
						point.Y = centerPoint.Y - (moddedWidth - 1) / 2 + y;
						area.Add(point);
					}
				}
				offset = 1;
			}



			offset = 0;
			// North Section
			for (int y = 1; y < taper / 2 + 1; y++)
			{
				for (int x = 0; x < moddedWidth; x++)
				{
				
					if (x > (offset - 1) && x < moddedWidth - offset)
					{
						point = new Vector2();
						point.X = centerPoint.X - (moddedWidth - 1) / 2 + x;
						point.Y = centerPoint.Y - (width - taper / 2) - y;
						area.Add(point);
					}
				}
				offset = 1;
			}

			offset = 0;
			// South Section
			for (int y = 1; y < taper / 2 + 1; y++)
			{
				for (int x = 0; x < moddedWidth; x++)
				{
					if (y > (offset - 1) && y < moddedWidth - offset)
					{
						point = new Vector2();
						point.X = centerPoint.X - (moddedWidth - 1) / 2 + x;
						point.Y = centerPoint.Y + (width - taper / 2) + y;
						area.Add(point);
					}
				}
				offset = 1;
			}


			// Finally Water the area obtained
			WaterArea(location, area);
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
				c.SpeedIncreaser speedIncreaser = config.SpeedIncreaserConfig.Find(x => x.Name == machine.name);
				if (speedIncreaser != null)
				{
					TemporalIncreaser(location, speedIncreaser, machine, timePassed);
					continue;
				}

				/*
				if (machine.name == "Large Growing Tree")
				{
					if (machine.readyForHarvest)
						if (machine.heldObject != null)
							print("Machine holds: " + machine.heldObject.name);
				}
				*/
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
