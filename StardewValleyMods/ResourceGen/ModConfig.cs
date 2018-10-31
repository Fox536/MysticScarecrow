using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Fox536.ResourceGen
{		
	class ModConfig
	{
		// Mine Area
		public bool doSpawnOre { get; set; } = false;
		public bool OreUseMineLevel { get; set; } = true;
		public List<Vector2[]> MineLocations { get; set; } = new List<Vector2[]>() {
			new Vector2[] {new Vector2( 0, 0), new Vector2( 0, 0) },
		};

		public bool doSpawnOre2 { get; set; } = false;
		public List<MineArea> MineLocations2 { get; set; } = new List<MineArea> {
			new MineArea("Farm", new List<Rectangle> {
				new Rectangle(1, 2, 3, 4),
				new Rectangle(4, 1, 3, 1),
			}),
		};
		public bool UseMineOreLimit { get; set; } = false;
		public int MinOreLimit { get; set; } = 10;
		private List<Vector2> mineArea = null;
		public List<Vector2> GetMineArea()
		{
			if (mineArea == null)
			{
				mineArea = new List<Vector2>();
				foreach (Vector2[] pointGroup in MineLocations)
				{
					for (int x = (int)pointGroup[0].X; x <= (int)pointGroup[1].X; x++)
					{
						for (int y = (int)pointGroup[0].Y; y <= (int)pointGroup[1].Y; y++)
						{
							mineArea.Add(new Vector2(x, y));
						}
					}
				}
			}

			return mineArea;
		}
		public double oreChance { get; set; } = 0.1;
		public double gemChance { get; set; } = 0.05;
		
		// Tree Spawns
		public bool doSpawnTrees { get; set; } = false;
		public List<Vector2> TreeLocations { get; set; } = new List<Vector2>() {
			new Vector2( 1,   2),
			new Vector2( 3,   4),
		};

		// Boulder Spawns
		public bool doSpawnBoulders { get; set; } = false;
		public List<Vector2> BoulderLocations { get; set; } = new List<Vector2>() {
			new Vector2( 4,   4),
			new Vector2( 4,   6),
		};

		// Stump Spawns
		public bool doSpawnStumps { get; set; } = false;
		public List<Vector2> StumpLocations { get; set; } = new List<Vector2>() {
			new Vector2( 6,   4),
			new Vector2( 6,   6),
		};

		// Log Spawns
		public bool doSpawnLogs { get; set; } = false;
		public List<Vector2> LogLocations { get; set; } = new List<Vector2>() {
			new Vector2( 8,   4),
			new Vector2( 8,   6),
		};

		// Log Spawns
		public bool doSpawnGrass { get; set; } = false;
		public List<Vector2[]> GrassLocations { get; set; } = new List<Vector2[]>() {
			new Vector2[] { new Vector2(20, 20), new Vector2( 60, 80) },
			new Vector2[] { new Vector2(80, 20), new Vector2(140, 80) },
		};

		private List<Vector2> grassArea = null;
		public List<Vector2> GetGrassArea()
		{
			if (grassArea == null)
			{
				grassArea = new List<Vector2>();
				foreach (Vector2[] pointGroup in GrassLocations)
				{
					for (int x = (int)pointGroup[0].X; x <= (int)pointGroup[1].X; x++)
					{
						for (int y = (int)pointGroup[0].Y; y <= (int)pointGroup[1].Y; y++)
						{
							grassArea.Add(new Vector2(x, y));
						}
					}
				}
			}

			return grassArea;
		}
	}

	public class MineArea
	{
		public string		LocationName	{ get; set; }
		public List<Rectangle> Area { get; set; } = new List<Rectangle> {
			new Rectangle(1, 2, 4, 5)
		};

		public MineArea(string name, List<Rectangle> area)
		{
			LocationName = name;
			Area = area;
		}

		public static List<MineArea> CombineAllDuplicates(List<MineArea> list)
		{
			List<MineArea> newArea = new List<MineArea>();
			
			// Get All affected Maps
			List<string> locations = new List<string>();
			foreach (var item in list) {
				if (!locations.Contains(item.LocationName))
					locations.Add(item.LocationName);
			}
			foreach (var location in locations)
			{
				foreach (var item in list.FindAll(n => n.LocationName == location))
				{
					MineArea first = newArea.Find(n => n.LocationName == location);
					if (first != null)
					{
						first.Area.AddRange(item.Area);
						first.Area.Sort();
					}
					else
					{
						newArea.Add(item);
					}
				}
			}

			return newArea;
		}

		public static List<Vector2> GetArea(MineArea area)
		{
			List<Vector2> newArea = new List<Vector2>();

			foreach (var rect in area.Area)
			{
				for (int x = 0; x < rect.Width; x++)
				{
					for (int y = 0; y < rect.Height; y++)
					{
						newArea.Add(new Vector2(rect.X + x, rect.Y + y));
					}
				}
			}

			return newArea;
		}
	}
}
