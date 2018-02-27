using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;

namespace Fox536.ResourceGen
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
			AddObjects();
		}

		#region Daily Spawns
		/// <summary>
		/// Adds Spawnable Objects
		/// </summary>
		private void AddObjects()
		{
			Farm farm = (Farm)Game1.locations[1];
			if (modConfig.doSpawnOre)
				AddMineObjs(farm);

			if (modConfig.doSpawnBoulders)
				SpawnBoulders(farm);

			if (modConfig.doSpawnTrees)
				SpawnTrees(farm);

			if (modConfig.doSpawnStumps)
				SpawnStumps(farm);

			if (modConfig.doSpawnLogs)
				SpawnLogs(farm);

			if (modConfig.doSpawnGrass)
				SpawnGrassPoints(farm);
		}

		private void SpawnBoulders(Farm farm)
		{
			foreach (Vector2 point in modConfig.BoulderLocations)
			{
				SpawnBoulder(farm, point);
			}
		}
		private void SpawnBoulder(Farm farm, Vector2 point)
		{
			ClearResourceClump(ref farm.resourceClumps, point);
			farm.addResourceClumpAndRemoveUnderlyingTerrain(ResourceClump.boulderIndex, 2, 2, point);
		}

		private void SpawnTrees(Farm farm)
		{
			// Add Config Option Here
			foreach (Vector2 point in modConfig.TreeLocations)
			{
				SpawnTree(farm, point);
			}
		}
		private void SpawnTree(Farm farm, Vector2 point)
		{
			StardewValley.TerrainFeatures.Tree t = new Tree(1, 5);
			t.seasonUpdate(true);
			ClearResourceClump(ref farm.resourceClumps, point);
			TerrainFeature feature = null;
			if (farm.terrainFeatures.TryGetValue(point, out feature))
			{
				if (feature.GetType() != t.GetType())
				{
					farm.terrainFeatures.Clear();
					farm.terrainFeatures.Add(point, t);
				}
			}
			else
				farm.terrainFeatures.Add(point, t);
		}

		private void SpawnLogs(Farm farm)
		{
			foreach (Vector2 point in modConfig.LogLocations)
			{
				SpawnLog(farm, point);
			}
		}
		private void SpawnLog(Farm farm, Vector2 point)
		{
			ClearResourceClump(ref farm.resourceClumps, point);
			farm.addResourceClumpAndRemoveUnderlyingTerrain(ResourceClump.hollowLogIndex, 2, 2, point);
		}

		private void SpawnStumps(Farm farm)
		{
			foreach (Vector2 point in modConfig.StumpLocations)
			{
				SpawnStump(farm, point);
			}
		}
		private void SpawnStump(Farm farm, Vector2 point)
		{
			ClearResourceClump(ref farm.resourceClumps, point);
			farm.addResourceClumpAndRemoveUnderlyingTerrain(ResourceClump.stumpIndex, 2, 2, point);
		}

		private void SpawnGrassPoints(Farm farm)
		{
			foreach (Vector2 point in modConfig.GetGrassArea())
			{
				SpawnGrass(farm, point);
			}
		}
		private void SpawnGrass(Farm farm, Vector2 point)
		{
			TerrainFeature check;
			if (farm.terrainFeatures.TryGetValue(point, out check))
			{
				if (check is Grass)
				{
					((Grass)check).numberOfWeeds = 4;
				}
			}
			else
			{
				farm.terrainFeatures.Add(point, new Grass(Grass.springGrass, 4));
			}

		}
		#endregion

		#region Mine Methods
		private void AddMineObjs(Farm farm)
		{
			print("adding mine objs");
			// Create Mine Area if needed
			// Mine Area
			//if (modConfig.AddMineArea)
			//{
			Random randomGen = new Random();
			foreach (Vector2 tile in modConfig.GetMineArea())
			{
				if (!modConfig.OreUseMineLevel)
				{
					if (randomGen.NextDouble() < modConfig.oreChance)
					{
						addRandomOre(ref farm, ref randomGen, 4, tile);
						continue;
					}
				}
				//calculate ore spawn
				else if (Game1.player.hasSkullKey)
				{
					if (randomGen.NextDouble() < modConfig.oreChance)
					{
						addRandomOre(ref farm, ref randomGen, 4, tile);
						continue;
					}
				}
				else
				{
					//check mine level
					if (Game1.player.deepestMineLevel > 80) //gold level
					{
						if (randomGen.NextDouble() < modConfig.oreChance)
						{
							addRandomOre(ref farm, ref randomGen, 3, tile);
							continue;
						}
					}
					else if (Game1.player.deepestMineLevel > 40) //iron level
					{
						if (randomGen.NextDouble() < modConfig.oreChance)
						{
							addRandomOre(ref farm, ref randomGen, 2, tile);
							continue;
						}
					}
					else
					{
						if (randomGen.NextDouble() < modConfig.oreChance)
						{
							addRandomOre(ref farm, ref randomGen, 1, tile);
							continue;
						}
					}
				}

				//if ore doesnt spawn then calculate gem spawn
				//1% to spawn gem
				if (randomGen.NextDouble() < modConfig.gemChance)
				{
					if (!modConfig.OreUseMineLevel)
					{
						if (randomGen.Next(0, 100) < 10)
						{
							farm.setObject(tile, createOre("mysticStone", tile));
							continue;
						}
					}
					else if (Game1.player.hasSkullKey)
						if (randomGen.Next(0, 100) < 10)
						{
							farm.setObject(tile, createOre("mysticStone", tile));
							continue;
						}
						else if (randomGen.Next(0, 500) < 1)
						{
							farm.setObject(tile, createOre("mysticStone", tile));
							continue;
						}

					switch (randomGen.Next(0, 100) % 8)
					{
						case 0: farm.setObject(tile, createOre("gemStone", tile)); break;
						case 1: farm.setObject(tile, createOre("diamond", tile)); break;
						case 2: farm.setObject(tile, createOre("ruby", tile)); break;
						case 3: farm.setObject(tile, createOre("jade", tile)); break;
						case 4: farm.setObject(tile, createOre("amethyst", tile)); break;
						case 5: farm.setObject(tile, createOre("topaz", tile)); break;
						case 6: farm.setObject(tile, createOre("emerald", tile)); break;
						case 7: farm.setObject(tile, createOre("aquamarine", tile)); break;
						default: break;
					}
					continue;
				}
			}
			//}
		}
		static void ClearResourceClump(ref List<ResourceClump> input, Vector2 RCLocation)
		{
			for (int i = 0; i < input.Count; i++)
			{
				ResourceClump RC = input[i];
				if (RC.tile == RCLocation)
				{
					input.RemoveAt(i);
					i--;
				}
			}
		}
		static void addRandomOre(ref Farm input, ref Random randomGen, int highestOreLevel, Vector2 tileLocation)
		{
			switch (randomGen.Next(0, 100) % highestOreLevel)
			{
				case 0: input.setObject(tileLocation, createOre("copperStone", tileLocation)); break;
				case 1: input.setObject(tileLocation, createOre("ironStone", tileLocation)); break;
				case 2: input.setObject(tileLocation, createOre("goldStone", tileLocation)); break;
				case 3: input.setObject(tileLocation, createOre("iridiumStone", tileLocation)); break;
				default: break;
			}
		}
		static StardewValley.Object createOre(string oreName, Vector2 tileLocation)
		{
			switch (oreName)
			{
				case "mysticStone":
					return new StardewValley.Object(tileLocation, 46, "Stone", true, false, false, false);
				case "gemStone":
					return new StardewValley.Object(tileLocation, (Game1.random.Next(7) + 1) * 2, "Stone", true, false, false, false);
				case "diamond":
					return new StardewValley.Object(tileLocation, 2, "Stone", true, false, false, false);
				case "ruby":
					return new StardewValley.Object(tileLocation, 4, "Stone", true, false, false, false);
				case "jade":
					return new StardewValley.Object(tileLocation, 6, "Stone", true, false, false, false);
				case "amethyst":
					return new StardewValley.Object(tileLocation, 8, "Stone", true, false, false, false);
				case "topaz":
					return new StardewValley.Object(tileLocation, 10, "Stone", true, false, false, false);
				case "emerald":
					return new StardewValley.Object(tileLocation, 12, "Stone", true, false, false, false);
				case "aquamarine":
					return new StardewValley.Object(tileLocation, 14, "Stone", true, false, false, false);
				case "iridiumStone":
					return new StardewValley.Object(tileLocation, 765, 1);
				case "goldStone":
					return new StardewValley.Object(tileLocation, 764, 1);
				case "ironStone":
					return new StardewValley.Object(tileLocation, 290, 1);
				case "copperStone":
					return new StardewValley.Object(tileLocation, 751, 1);
				default:
					return null;
			}
		}
		#endregion


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
