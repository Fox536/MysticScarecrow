using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fox536.Machines.Config
{
	internal class ModConfig
	{
		public List<SprinklerScarecrow> SprinklerScarecrowConfig { get; set; } = new List<SprinklerScarecrow>()
		{
			new SprinklerScarecrow("Mystic Scarecrow", 13)
		};
		public List<SpeedIncreaser> SpeedIncreaserConfig { get; set; } = new List<SpeedIncreaser>
		{
			new SpeedIncreaser("Temporal Crystal", 50, 5),
			new SpeedIncreaser("Large Temporal Crystal", 100, 5)
		};
	}

	internal class SprinklerScarecrow
	{
		public string	Name  { get; set; }
		public int		Width { get; set; }

		internal SprinklerScarecrow(string name, int width)
		{
			Name = name;
			Width = width;
		}
	}
	internal class SpeedIncreaser
	{
		public string	Name		{ get; set; }
		public int		SpeedBuff	{ get; set; }
		public int		Width		{ get; set; }

		internal SpeedIncreaser(string name, int speedBuff, int width)
		{
			Name = name;
			SpeedBuff = speedBuff;
			Width = width;
		}
	}
}
