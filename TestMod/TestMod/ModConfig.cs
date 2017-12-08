using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TestMod
{
	class ModConfig
	{
		public bool debug			{ get; set; } = true;
		public bool AddTreeSector1	{ get; set; } = true;
		public bool AddTreeSector2	{ get; set; } = true;
		public bool AddTreeSector3	{ get; set; } = true;
		public bool AddTreeSector4	{ get; set; } = false;
		public bool AddTreeSector5	{ get; set; } = true;
		public bool AddTreeSector6	{ get; set; } = true;
		public bool AddTreeSector7	{ get; set; } = true;
		public bool AddTreeSector8	{ get; set; } = true;
		public bool AddTreeSector9	{ get; set; } = true;

		public bool AddMineArea		{ get; set; } = true;

		public int Mine_StartX	{ get; set; } = 6;
		public int Mine_StartY	{ get; set; } = 104;
		public int Mine_EndX	{ get; set; } = 24;
		public int Mine_EndY	{ get; set; } = 114;

		public double oreChance { get; set; } = 0.05;
		public double gemChance { get; set; } = 0.01;

		public bool UsingFarmExpansionPatch { get; set; } = true;
		public bool AddBothEntrances	{ get; set; } = false;

		public List<Vector2> BoulderArea { get; set; } = new List<Vector2>() {
			new Vector2( 13,  10),
			new Vector2(  6, 113),
			new Vector2(  9, 113),
			new Vector2( 18, 113),
			new Vector2( 21, 113),
			new Vector2( 23, 113),
			new Vector2( 86,   2),
			new Vector2( 50,  57),
			new Vector2( 60,  71),
			new Vector2(114,  45) };

	}
}
