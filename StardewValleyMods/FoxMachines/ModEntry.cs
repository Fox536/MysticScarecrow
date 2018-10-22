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

		// Used for caching the Machine Grouping so it doesn't have to re find all the machines every game hour.
		Dictionary<GameLocation, Dictionary<StardewValley.Object, List<StardewValley.Object>>> MachineGroups = new Dictionary<GameLocation, Dictionary<StardewValley.Object, List<StardewValley.Object>>>();
		
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
			LocationEvents.ObjectsChanged += LocationEvents_ObjectsChanged;

			GameEvents.OneSecondTick += GameEvents_OneSecondTick;
			
		}


		List<GameLocation> updateNeeded = new List<GameLocation>();

		int updateTick = 0;
		private void GameEvents_OneSecondTick(object sender, EventArgs e)
		{
			if (updateTick > config.UpdateTime)
			{
				ItterateMachines(Game1.currentLocation, 0);
				updateTick = 0;
			}
			else
			{
				updateTick++;
			}
		}

		private void LocationEvents_ObjectsChanged(object sender, EventArgsLocationObjectsChanged e)
		{
			if (MachineGroups.ContainsKey(Game1.currentLocation))
			{
				if (!updateNeeded.Contains(Game1.currentLocation))
					updateNeeded.Add(Game1.currentLocation);
			}
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

			// Check for Overnight Changes
			if (timePassed < 0) { timePassed *= -1; }

			try
			{
				foreach (GameLocation location in Game1.locations)
				{
					ItterateMachines(location, timePassed);
				}

				foreach (StardewValley.Buildings.Building bLocation in Game1.getFarm().buildings)
				{
					ItterateMachines(bLocation.indoors, timePassed);
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
				foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects.Pairs)
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
							(location.terrainFeatures[currentPoint] as HoeDirt).state.Value = 1;
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
						(location.terrainFeatures[point] as HoeDirt).state.Value = state;
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

		private void ItterateMachines(GameLocation location, int timePassed)
		{
			if (location == null || location.objects == null)
				return;

			foreach (KeyValuePair<Vector2, StardewValley.Object> item in location.objects.Pairs)
			{
				StardewValley.Object machine = item.Value;
				if (item.Value == null)
					continue;


				//-------------------------------------
				// Bee Hive
				//  - Make it work indoors
				//-------------------------------------
				/*
				if (item.Value.name == BeeHiveName)
				{
					UpdateBeehive(location, machine, timePassed);
					continue;
				}
				*/
				//-------------------------------------
				// Bee Hive
				//-------------------------------------

				//-------------------------------------
				// Machine Speed Boost
				//-------------------------------------
				c.SpeedIncreaser speedIncreaser = config.SpeedIncreaserConfig.Find(x => x.Name == machine.name);
				if (speedIncreaser != null)
				{
					TemporalIncreaser(location, speedIncreaser, machine, timePassed);
					continue;
				}
				/*
				else if (machine is StardewValley.Objects.Cask cask)
				{
					StardewValley.Object temporalMachine = TemporalIncreaserInRange(location, cask);
					if (temporalMachine != null)
					{
						speedIncreaser = config.SpeedIncreaserConfig.Find(obj => obj.Name == temporalMachine.name);
						if (speedIncreaser != null)
						{
							cask.agingRate = 1 + (speedIncreaser.SpeedBuff / 100);
						}
					}
					else
					{
						cask.agingRate = 1;
					}
				}
				*/
				//-------------------------------------
				// Machine Speed Boost
				//-------------------------------------


				//-------------------------------------
				// Mystic Collector
				//-------------------------------------
				if (config.CollectorMachines.Contains(machine.Name))
				{
					MysticCollectorUpdate(location, machine);
				}
				//-------------------------------------
				// Mystic Collector
				//-------------------------------------

				//-------------------------------------
				// Sap Collector
				//-------------------------------------
				if (config.SapCollectorMachines.Contains(machine.Name))
				{
					SapCollectorUpdate(location, machine);
				}
				//-------------------------------------
				// Sap Collector
				//-------------------------------------

			}
		}
		

		// For Collectors
		private StardewValley.Objects.Chest GetConnectedChest(GameLocation location, StardewValley.Object machine)
		{
			List<Vector2> surroundingPoints = GetSurroundingTiles(machine.tileLocation);
			foreach (var point in surroundingPoints)
			{
				if (location.Objects.ContainsKey(point))
				{
					if (location.Objects[point] is StardewValley.Objects.Chest chest)
						return chest;
				}
			}
			return null;
		}
		
		// For Mystic Collector
		private void MysticCollectorUpdate(GameLocation location, StardewValley.Object machine)
		{
			print("found collector in: " + location.name);

			List<StardewValley.Object> connectedMachines = null;

			// Get Connected Chest
			StardewValley.Objects.Chest chest = GetConnectedChest(location, machine);
			if (chest != null)
			{
				//print("found chest");

				
				Dictionary<StardewValley.Object, List<StardewValley.Object>> groups;


				// Get Machine Group
				if (updateNeeded.Contains(location)) // if update is needed
				{
					// do a new search
					connectedMachines = GetConnectedMachinesGroup(location, machine);
					print("New Search found " + connectedMachines.Count + " machines connected");
				}
				else if (MachineGroups.ContainsKey(location)) // otherwise
				{
					// use existing group, if possible
					groups = MachineGroups[location];
					if (groups.ContainsKey(machine))
					{
						connectedMachines = groups[machine];
						print("Already found " + connectedMachines.Count + " machines connected");
					}
				}

				// if no group exists create one
				if (connectedMachines == null)
				{
					connectedMachines = GetConnectedMachinesGroup(location, machine);
					print("New Search found " + connectedMachines.Count + " machines connected");
				}

				// Add location and group to the cache
				if (MachineGroups.ContainsKey(location))
				{
					if (MachineGroups[location].ContainsKey(machine))
					{
						MachineGroups[location][machine] = connectedMachines;
					}
					else
					{
						MachineGroups[location].Add(machine, connectedMachines);
					}
				}
				else
				{
					Dictionary<StardewValley.Object, List<StardewValley.Object>> machineGroup = new Dictionary<StardewValley.Object, List<StardewValley.Object>>();
					machineGroup.Add(machine, connectedMachines);
					MachineGroups.Add(location, machineGroup);
				}
				


				// loop through all connected machines
				foreach (var currentMachine in connectedMachines)
				{
					//print("found machine: " + currentMachine.name);
					// Check if any of the connected machines are ready to be collected
					if (currentMachine.readyForHarvest && currentMachine.heldObject != null)
					{
						//print("machine ready with: " + currentMachine.heldObject.name);

						// check if chest holds the item already
						if (chest.items.Contains(currentMachine.heldObject))
						{
							StardewValley.Item itemObj = null;
							for (int i = 0; i < chest.items.Count; i++)
							{
								if (currentMachine.heldObject.Value == chest.items[i])
								{
									itemObj = currentMachine.heldObject.Value;
								}
							}
							if ((itemObj.maximumStackSize() - itemObj.Stack > currentMachine.heldObject.Value.stack) && (itemObj != null))
							{
								//print("adding to chest stack");
								chest.addToStack(currentMachine.heldObject.Value.stack);
								currentMachine.readyForHarvest.Value = false;
								currentMachine.minutesUntilReady.Value = -1;
							}
							else
							{
								if (chest.items.Count < 35)
								{
									//print("adding to chest");
									chest.addItem(currentMachine.heldObject);
									currentMachine.readyForHarvest.Value = false;
									currentMachine.minutesUntilReady.Value = -1;
								}
							}
						}
						else
						{
							if (chest.items.Count < 35)
							{
								//print("adding to chest");
								chest.addItem(currentMachine.heldObject);
								currentMachine.readyForHarvest.Value = false;
								currentMachine.minutesUntilReady.Value = -1;
							}
						}

					}
				}
			}
		}
		private List<StardewValley.Object> GetConnectedMachinesGroup(GameLocation location, StardewValley.Object machine)
		{
			List<StardewValley.Object> connectedMachines = new List<StardewValley.Object>();
			List<StardewValley.Object> machinesToCheck = new List<StardewValley.Object>() { machine };
			
			// loop while machines.count > 0
			while (machinesToCheck.Count > 0)
			{
				// Get Current Machine
				StardewValley.Object currentMachine = machinesToCheck[0];
				
				// Get Adjacent Tiles
				List<Vector2> adjacentTiles = GetSurroundingTiles(currentMachine.tileLocation);
				
				// Check Adjacent Tiles for Machines
				foreach (var point in adjacentTiles)
				{
					if (location.objects.ContainsKey(point) && !connectedMachines.Contains(location.objects[point]))
					{
						if (config.CollectedMachines.Contains(location.objects[point].name))
						{
							// Add Machine Found to Return List
							connectedMachines.Add(location.objects[point]);
							
							// Add Machine Found, to Check on Later Cycle
							machinesToCheck.Add(location.objects[point]);
						}
					}
				}

				// Remove Current Machine
				machinesToCheck.Remove(currentMachine);
			}
			
			return connectedMachines;
		}
		private List<Vector2> GetSurroundingTiles(Vector2 point)
		{
			List<Vector2> surroundingPoints = new List<Vector2>();

			Vector2 currentPoint = point;

			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					currentPoint.X = point.X - 1 + x;
					currentPoint.Y = point.Y - 1 + y;
					if (currentPoint == point)
						continue;
					surroundingPoints.Add(currentPoint);
				}
			}

			return surroundingPoints;
		}

		// For Sap Collector
		private void SapCollectorUpdate(GameLocation location, StardewValley.Object machine)
		{
			List<StardewValley.Object> connectedMachines = GetAllMachinesInLocationNamed(location, "Tapper"); //GetConnectedMachines(location, machine);
			//print("Found " + connectedMachines.Count + " Tappers.");
			
			// Get Connected Chest
			StardewValley.Objects.Chest chest = GetConnectedChest(location, machine);
			if (chest != null)
			{
				// loop through all connected machines
				foreach (var currentMachine in connectedMachines)
				{
					// Check if any of the connected machines are ready to be collected
					if (currentMachine.readyForHarvest && currentMachine.heldObject != null)
					{
						// check if chest holds the item already
						if (chest.items.Contains(currentMachine.heldObject))
						{
							StardewValley.Item itemObj = null;
							for (int i = 0; i < chest.items.Count; i++)
							{
								if (currentMachine.heldObject.Value == chest.items[i])
								{
									itemObj = currentMachine.heldObject.Value;
								}
							}
							if ((itemObj.maximumStackSize() - itemObj.Stack > currentMachine.heldObject.Value.stack) && (itemObj != null))
							{
								chest.addToStack(currentMachine.heldObject.Value.stack);
								currentMachine.readyForHarvest.Value = false;
								currentMachine.heldObject.Value = null;
							}
							else
							{
								if (chest.items.Count < 35)
								{
									chest.addItem(currentMachine.heldObject);
									currentMachine.readyForHarvest.Value = false;
									currentMachine.heldObject.Value = null;
								}
							}
						}
						else
						{
							if (chest.items.Count < 35)
							{
								chest.addItem(currentMachine.heldObject);
								currentMachine.readyForHarvest.Value = false;
								currentMachine.heldObject.Value = null;
							}
						}

					}
				}
			}
		}
		private List<StardewValley.Object> GetAllMachinesInLocationNamed(GameLocation location, string machineName)
		{
			List<StardewValley.Object> machines = new List<StardewValley.Object>();
			foreach (var item in location.objects.Pairs)
			{
				if (item.Value.name == machineName)
				{
					machines.Add(item.Value);
				}
			}

			return machines;
		}

		// Not used anymore
		private List<StardewValley.Object> GetAllMysticMachinesInLocation(GameLocation location)
		{
			List<StardewValley.Object> machines = new List<StardewValley.Object>();
			foreach (var item in location.objects.Pairs)
			{
				if (config.CollectedMachines.Contains(item.Value.name))
				{
					machines.Add(item.Value);
				}
			}

			return machines;
		}
		
		// Beehive
		private void UpdateBeehive(GameLocation location, StardewValley.Object machine, int timePassed)
		{
			/*
			machine.setIndoors.Value = true;
			machine.setOutdoors.Value = true;
			// Allow beehive to function in Greenhouse
			if (location.Name == "Greenhouse" || location.Name.Contains("Shed"))
				machine.minutesUntilReady.Value = Math.Max(machine.minutesUntilReady - timePassed, 0);

				if (machine.minutesUntilReady <= 0)
				{
					Crop c = Utility.findCloseFlower(machine.tileLocation);
					machine.honeyType = (StardewValley.Object.HoneyType)c.indexOfHarvest;
					machine.readyForHarvest = true;
				}
			}
			*/
		}

		// Temporal Increaser
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
						if (affectedMachine is StardewValley.Objects.Cask cask)
						{
							cask.agingRate.Value = 1 + modifier;
						}
						else
						{ 
							if (!affectedMachine.readyForHarvest)
							{
								affectedMachine.minutesUntilReady.Value = Math.Max(affectedMachine.minutesUntilReady - (int)Math.Floor(timePassed * modifier), 0);

							}
						}
					}
				}
			}
		}


		private StardewValley.Object TemporalIncreaserInRange(GameLocation location, StardewValley.Object machine)
		{
			int width = 11;
			int halfSize = width / 2;
			//float modifier = (speedIncreaser.SpeedBuff / 100);

			Vector2 centerPoint = machine.tileLocation;
			Vector2 currentPoint = machine.tileLocation;

			int currentWidth = 0;

			for (int x = 0; x < width; x++)
			{
				currentWidth = x;
				for (int y = 0; y < width; y++)
				{
					currentWidth = x + y;

					currentPoint.X = centerPoint.X - halfSize + x;
					currentPoint.Y = centerPoint.Y - halfSize + y;

					if (location.objects.ContainsKey(currentPoint))
					{
						StardewValley.Object newMachine = location.objects[currentPoint];
						c.SpeedIncreaser speedIncreaser = config.SpeedIncreaserConfig.Find(obj => obj.Name == newMachine.name);
						if (currentWidth <= speedIncreaser.Width)
						{
							return newMachine;
						}
					}
				}
			}


			return null;
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
