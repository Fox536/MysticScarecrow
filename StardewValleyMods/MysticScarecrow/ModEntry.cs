using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace Fox536.MysticScarecrow
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		
		internal Config.ModConfig config;
		
		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;

			config = helper.ReadConfig<Config.ModConfig>();
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