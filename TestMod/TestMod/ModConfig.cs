using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TestMod
{
	class ModConfig
	{
		public bool debug			{ get; set; } = true;

		// Tree Plots
		public bool AddTreePlot1				{ get; set; } = true;
		public bool AddTreePlot2				{ get; set; } = true;
		public bool AddTreePlot3				{ get; set; } = true;
		public bool AddTreePlot4				{ get; set; } = true;
		public bool AddTreePlot5				{ get; set; } = true;
		public bool AddTreePlot6				{ get; set; } = true;
		public bool AddTreePlot7				{ get; set; } = true;
		public bool AddTreePlot8				{ get; set; } = true;
		public bool AddTreePlot9				{ get; set; } = true;
		
		// Farm Expansion Patch
		public bool UsingFarmExpansionPatch		{ get; set; } = true;
		public bool AddBothEntrances			{ get; set; } = true;
		
		// Mine Area
		public bool AddMineArea					{ get; set; } = true;
		public bool doSpawnOre					{ get; set; } = true;
		public bool OreUseMineLevel				{ get; set; } = true;
		public List<Vector2[]> MineLocations	{ get; set; } = new List<Vector2[]>() {
			new Vector2[] {new Vector2( 6, 101), new Vector2( 24, 112) },
			new Vector2[] {new Vector2(89,   3), new Vector2( 96,   7) },
			new Vector2[] {new Vector2(97,   4), new Vector2(115,  10) },
			new Vector2[] {new Vector2(91,   8), new Vector2( 96,   8) },
			new Vector2[] {new Vector2(92,   9), new Vector2( 96,   9) },
			new Vector2[] {new Vector2(93,  10), new Vector2( 96,  10) },
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
		public double oreChance					{ get; set; } = 0.1;
		public double gemChance					{ get; set; } = 0.05;

		// Tree Spawns
		public bool doSpawnTrees				{ get; set; } = true;
		public List<Vector2> TreeLocations		{ get; set; } = new List<Vector2>() {
			new Vector2(  6,   8),
			new Vector2( 10,   8),
			new Vector2( 35,  59),
			new Vector2( 60,  71),
			new Vector2( 73,  32),
			new Vector2( 29,  63),
			new Vector2( 28,  65),
			new Vector2(  5,  74),
			new Vector2(108,  23),
			new Vector2( 96,   1),
		};

		// Boulder Spawns
		public bool doSpawnBoulders				{ get; set; } = true;
		public List<Vector2> BoulderLocations	{ get; set; } = new List<Vector2>() {
			new Vector2(  3,   9),
			new Vector2(  3,  11),
			new Vector2(  3,  13),
			new Vector2(  3,  15),
			new Vector2(  6,  16),
			new Vector2( 23,   8),
			new Vector2( 25,   8),
			new Vector2( 27,   8),
			new Vector2( 29,   8),
			new Vector2(  6, 113),
			new Vector2(  8, 113),
			new Vector2( 10, 113),
			new Vector2( 12, 113),
			new Vector2( 17, 113),
			new Vector2( 19, 113),
			new Vector2( 21, 113),
			new Vector2( 23, 113),
			new Vector2( 86,   2),
			new Vector2( 50,  57),
			new Vector2( 62,  72),
			new Vector2(114,  45),
			new Vector2(111,  12)
		};
		
		// Stump Spawns
		public bool doSpawnStumps				{ get; set; } = true;
		public List<Vector2> StumpLocations		{ get; set; } = new List<Vector2>() {
			new Vector2( 7,  24),
			new Vector2( 9,  26),
			new Vector2(13,  27),
		};

		// Log Spawns
		public bool doSpawnLogs					{ get; set; } = true;
		public List<Vector2> LogLocations		{ get; set; } = new List<Vector2>() {
			new Vector2(  3,  23),
			new Vector2(  4,  26),
			new Vector2( 18,  28),
			new Vector2(107,  12),
		};

		// Log Spawns
		public bool doSpawnGrass				{ get; set; } = true;
		public List<Vector2[]> GrassLocations	{ get; set; } = new List<Vector2[]>() {
			new Vector2[] { new Vector2(99, 73), new Vector2(115, 84) },
			new Vector2[] { new Vector2(99, 96), new Vector2(115, 108) },
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

		public Vector2 Warps_Forest				{ get; set; } = new Vector2(32, 117);
		public Vector2 Warps_Backwoods			{ get; set; } = new Vector2(-1, -1);
		public Vector2 Warps_Busstop			{ get; set; } = new Vector2(-1, -1);

		public int FarmExpansionWidth			{ get; set; } = 160;
		public int FarmExpansionHeight			{ get; set; } = 80;
	}

}
