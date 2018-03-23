using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fox536.Machines.Config
{
	internal class ModConfig
	{
		public int UpdateTime { get; set; } = 10;

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

		public List<string> SapCollectorMachines { get; set; } = new List<string> { "Sap Collector" };

		public List<string> CollectorMachines { get; set; } = new List<string> { "Collector" };
		public List<string> CollectedMachines { get; set; } = new List<string>
		{
			"Growing Shrub",
			"Growing Rock",
			"Large Growing Tree",
			"Large Growing Stone",
			"Coal Extractor",
			"Copper Extractor",
			"Iron Extractor",
			"Gold Extractor",
			"Growing Quartz",
			"Growing Weeds",
			"Solar Panel",
			"Advanced Solar Panel",
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
