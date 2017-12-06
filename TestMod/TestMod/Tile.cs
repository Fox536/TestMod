﻿namespace TestMod
{
	public enum TLayer { Back, Buildings, Paths, Front, AlwaysFront }

	public class Tile
	{
		public TLayer l;
		public string Layername;
		public int x;
		public int y;
		public int tileIndex;
		//public string layer;

		public Tile(TLayer l, int x, int y, int tileIndex)
		{
			this.l = l; this.x = x; this.y = y; this.tileIndex = tileIndex;
			Layername = l.ToString();
			/*
			switch (l)
			{
				case 0:
					this.layer = "Back";
					break;
				case 1:
					this.layer = "Buildings";
					break;
				case 2:
					this.layer = "Paths";
					break;
				case 3:
					this.layer = "Front";
					break;
				case 4:
					this.layer = "AlwaysFront";
					break;
			}
			*/
		}
	}
}