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

}
