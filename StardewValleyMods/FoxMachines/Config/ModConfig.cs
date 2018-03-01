using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fox536.Machines.Config
{
	internal class ModConfig
	{
		public bool ChangeSprinklers { get; set; } = false;
		public List<SprinklerArea> SprinklerAreaChanges { get; set; } = new List<SprinklerArea>() { new SprinklerArea() };

		public List<SprinklerScarecrow> SprinklerScarecrowConfig { get; set; } = new List<SprinklerScarecrow>()
		{
			new SprinklerScarecrow("Mystic Scarecrow", 8)
		};
		public List<SpeedIncreaser> SpeedIncreaserConfig { get; set; } = new List<SpeedIncreaser>
		{
			new SpeedIncreaser("Small Temporal Crystal", 50, 5),
			new SpeedIncreaser("Temporal Crystal", 100, 5),
			new SpeedIncreaser("Large Temporal Crystal", 200, 5)
		};
	}


	internal class SprinklerArea
	{
		public string Name		{ get; set; } = "Sprinkler";
		public string AreaType	{ get; set; } = "Basic";
		public int SquareWidth	{ get; set; } = 3;
		public int CustomNorth	{ get; set; } = 1;
		public int CustomWest	{ get; set; } = 1;
		public int CustomEast	{ get; set; } = 1;
		public int CustomSouth	{ get; set; } = 1;

		public SprinklerArea() { }
	}

	internal class SprinklerScarecrow
	{
		public string	Name  { get; set; } = "";
		public int		Width { get; set; } = 0;

		internal SprinklerScarecrow() { }
		internal SprinklerScarecrow(string name, int width)
		{
			Name = name;
			Width = width;
		}
	}
	internal class SpeedIncreaser
	{
		public string	Name		{ get; set; } = "";
		public int		SpeedBuff	{ get; set; } = 50;
		public int		Width		{ get; set; } = 5;

		internal SpeedIncreaser() { }
		internal SpeedIncreaser(string name, int speedBuff, int width)
		{
			Name = name;
			SpeedBuff = speedBuff;
			Width = width;
		}
	}
}
