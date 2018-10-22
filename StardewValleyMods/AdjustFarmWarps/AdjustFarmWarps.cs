using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;

namespace Fox536.AdjustFarmWarps
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		private static ModConfig modConfig;

		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			// Add Events
			SaveEvents.AfterLoad += SaveEvents_AfterLoad;
			SaveEvents.AfterSave += SaveEvents_AfterSave;

			TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;

			ChangeWarpPoints();
		}

		/// <summary>
		/// Handles the Config Creation/Loading
		/// </summary>
		private void CreateConfig()
		{
			// Create/Read Config
			modConfig = this.Helper.ReadJsonFile<ModConfig>($"data/{Constants.SaveFolderName}.json") ?? new ModConfig();
			if (modConfig != null)
				// Write Config in case it's Needed
				this.Helper.WriteJsonFile($"data/{Constants.SaveFolderName}.json", modConfig);
		}

		/// <summary>
		/// Fix any Warp Points as needed
		/// </summary>
		static void ChangeWarpPoints()
		{
			foreach (GameLocation location in Game1.locations)
			{
				if (modConfig.Warps_Forest.X != -1)
				{
					if (location is Forest)
					{
						foreach (Warp w in location.warps)
						{
							if (w.TargetName.ToLower().Contains("farm"))
							{
								w.TargetX = (int)modConfig.Warps_Forest.X;
								w.TargetY = (int)modConfig.Warps_Forest.Y;
							}
						}
					}
				}

				if (modConfig.Warps_Backwoods.X != -1)
				{
					if (location.name.Value.ToLower().Contains("backwood"))
					{
						foreach (Warp w in location.warps)
						{
							if (w.TargetName.ToLower().Contains("farm"))
							{
								w.TargetX = (int)modConfig.Warps_Backwoods.X;
								w.TargetY = (int)modConfig.Warps_Backwoods.Y;
							}
						}
					}
				}

				if (modConfig.Warps_Busstop.X != -1)
				{
					if (location.name.Value.ToLower().Contains("busstop"))
					{
						foreach (Warp w in location.warps)
						{
							if (w.TargetName.ToLower().Contains("farm"))
							{
								w.TargetX = (int)modConfig.Warps_Busstop.X;
								w.TargetY = (int)modConfig.Warps_Busstop.Y;
							}
						}
					}
				}
			}
		}

		// Events
		private void SaveEvents_AfterLoad(object sender, EventArgs e)
		{
			// Create/Read Config
			CreateConfig();
		}
		private void SaveEvents_AfterSave(object sender, EventArgs e)
		{
			if (modConfig == null)
			{
				// Create/Read Config
				CreateConfig();
			}
			else
			{
				this.Helper.WriteJsonFile($"data/{Constants.SaveFolderName}.json", modConfig);
			}
		}
		private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
		{
			ChangeWarpPoints();
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
