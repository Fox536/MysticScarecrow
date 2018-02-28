using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fox536.MysticScarecrow.Config
{
	internal class ModConfig
	{
		public List<SprinklerScarecrow> SprinklerScarecrowConfig	{ get; set; } = new List<SprinklerScarecrow>()
		{
			new SprinklerScarecrow("Mystic Scarecrow", 13)
		};
		public List<SpeadIncreaser>		SpeadIncreaserConfig		{ get; set; } = new List<SpeadIncreaser>
		{
			new SpeadIncreaser("Temporal Crystal", 50),
			new SpeadIncreaser("Large Temporal Crystal", 100)
		};
	}
	

	internal class SprinklerScarecrow
	{
		public string Name { get; set; }
		public int Width { get; set; }

		internal SprinklerScarecrow(string name, int width)
		{
			Name = name;
			Width = width;
		}
	}
	internal class SpeadIncreaser
	{
		public string	Name		{ get; set; }
		public int		SpeedBuff	{ get; set; }

		internal SpeadIncreaser(string name, int speedBuff)
		{
			Name = name;
			SpeedBuff = speedBuff;
		}
	}
}
