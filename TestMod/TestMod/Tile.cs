namespace TestMod
{
	public enum TLayer { Back, Buildings, Paths, Front, AlwaysFront }

	public class Tile
	{
		public TLayer l;
		public string Layername;
		public int x;
		public int y;
		public int tileIndex;

		public Tile(TLayer l, int x, int y, int tileIndex)
		{
			this.l = l; this.x = x; this.y = y; this.tileIndex = tileIndex;
			Layername = l.ToString();
		}
	}
}
