using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace YourProjectName
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
			TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;

		}

		private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
		{
			int size = 8;
			foreach (GameLocation location in Game1.locations)
			{
				foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects)
				{

					if (item.Value.name.Contains("arecrow"))
					{
						WaterScareCrowArea(location, item.Key);
					}
				}
			}
		}

		private void WaterScareCrowArea(GameLocation location, Vector2 centerPoint)
		{
			Vector2 currentPoint = new Vector2(centerPoint.X, centerPoint.Y);

			int width = 13;
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
		private static void print(string text)
		{
			if ((ModMonitor != null) && (modConfig.debug))
			{
				ModMonitor.Log(text);
			}
		}
	}
}