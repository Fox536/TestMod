using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Layers;
using xTile.Tiles;

namespace TestMod.MapEditor
{
	public static class TreeArea
	{
		internal static ModEntry _this;

		/*	Tile(int layer, int x, int y, int tileIndex)
Layer
0: Back
1: Buildings
2: Paths
3: Front
4: AlwaysFront */

		public static void AddCenter(List<Tile> tileList, int startX, int startY)
		{
			// Back Grassy Edits
			tileList.Add(new Tile(TLayer.Back, startX, startY, 200));
			tileList.Add(new Tile(TLayer.Back, startX + 1, startY, 201));
			tileList.Add(new Tile(TLayer.Back, startX + 2, startY, 203));

			tileList.Add(new Tile(TLayer.Back, startX, startY + 1, 225));
			tileList.Add(new Tile(TLayer.Back, startX + 1, startY + 1, 206));
			tileList.Add(new Tile(TLayer.Back, startX + 2, startY + 1, 228));

			tileList.Add(new Tile(TLayer.Back, startX, startY + 2, 250));
			tileList.Add(new Tile(TLayer.Back, startX + 1, startY + 2, 251));
			tileList.Add(new Tile(TLayer.Back, startX + 2, startY + 2, 253));
		}

		// Sidding
		public static void AddLeftSide(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 228));
		}
		public static void AddRightSide(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 225));
		}
		public static void AddTopSide(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 251));
		}
		public static void AddBottomSide(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 201));
		}

		// Corners
		public static void AddCornerTopLeft(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 178));
		}
		public static void AddCornerTopRight(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 252));
		}
		public static void AddCornerBottomLeft(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 177));
		}
		public static void AddCornerBottomRight(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 202));
		}


		public static void AddTurnLeftBottom(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 253));
		}
		

		public static void AddTileSquare(List<Tile> tileList, int startX, int startY, int width, int height, int tileId, TLayer layer)
		{
			// Back Grassy Edits
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					tileList.Add(new Tile(layer, startX + x, startY+ y, tileId));
				}
			}
		}

		
		public static void AddGrass(List<Tile> tileList, int startX, int startY)
		{
			tileList.Add(new Tile(TLayer.Back, startX, startY, 175));
		}

		public static void AddNoSpawn(List<Tile> tileList, List<int[]> noSpawnArea, GameLocation gl, int startX, int startY)
		{
			//tileList.Add(new Tile(3, startX, startY, 232));
			noSpawnArea.Add(new int[] { startX, startY });
			//gl.setTileProperty(startX, startY, "Back", "NoSpawn", "All");
		}

		public static void FillSquareArea(int startX, int startY, int width, int height, bool left, bool right, bool top, bool bottom, bool topLeft, bool topRight, bool bottomLeft, bool bottomRight)
		{
			// Create Arrays
			List<Tile> tileEdits = new List<Tile>();

			List<int[]> noSpawnArea = new List<int[]>();


			//--------------------------------------------------------------------------

			//--------------------------------------------------------------------------
			// Main Area
			//--------------------------------------------------------------------------
			// Modding - Add TileData Changes
			for (int x = 0; x < width * 3; x++)
			{
				for (int y = 0; y < height * 3; y++)
				{
					AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], startX + x, startY + y);
				}
			}

			// Modding - Add TileId Changes
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					AddCenter(tileEdits, startX + x * 3, startY + y * 3);
				}
			}
			//--------------------------------------------------------------------------

			//--------------------------------------------------------------------------
			// Sidding
			//--------------------------------------------------------------------------
			// Modding - Add Left Sidding
			if (left)
			{
				for (int y = 0; y < height * 3; y++)
				{
					AddLeftSide(tileEdits, startX - 1, startY + y);
					AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], startX - 1, startY + y);
				}
			}
			// Modding - Add Right Sidding
			if (right)
			{
				int rightSide = startX + width * 3;
				for (int y = 0; y < height * 3; y++)
				{
					AddRightSide(tileEdits, rightSide, startY + y);
					AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], rightSide, startY + y);
				}
			}
			// Modding - Add Top Sidding
			if (top)
			{
				for (int x = 0; x < width * 3; x++)
				{
					AddTopSide(tileEdits, startX + x, startY - 1);
					AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], startX + x, startY - 1);
				}
			}
			// Modding - Add Bottom Sidding
			if (bottom)
			{
				int bottomSide = startY + height * 3;
				for (int x = 0; x < width * 3; x++)
				{
					AddBottomSide(tileEdits, startX + x, bottomSide);
					AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], startX + x, bottomSide);
				}
			}
			//--------------------------------------------------------------------------

			//--------------------------------------------------------------------------
			// Corners
			//--------------------------------------------------------------------------
			// Modding - Add TopLeft Sidding
			if (topLeft)
			{
				AddCornerTopLeft(tileEdits, startX - 1, startY - 1);
				AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], startX - 1, startY - 1);
			}
			// Modding - Add TopRight Sidding
			if (topRight)
			{
				int rightSide = startX + width * 3;
				AddCornerTopRight(tileEdits, rightSide, startY - 1);
				AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], rightSide, startY - 1);
			}
			// Modding - Add BottomLeft Sidding
			if (bottomLeft)
			{
				int bottomSide = startY + height * 3;
				AddCornerBottomLeft(tileEdits, startX - 1, bottomSide);
				AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], startX - 1, bottomSide);
			}
			// Modding - Add BottomRight Sidding
			if (bottomRight)
			{
				int rightSide = startX + width * 3;
				int bottomSide = startY + height * 3;
				AddCornerBottomRight(tileEdits, rightSide, bottomSide);
				AddNoSpawn(tileEdits, noSpawnArea, Game1.locations[1], rightSide, bottomSide);
			}
			//--------------------------------------------------------------------------
			
			// Finalize Edits
			PatchMap(Game1.locations[1], tileEdits);

			for (int i = 0; i < noSpawnArea.Count; i++)
			{
				int x = noSpawnArea[i][0];
				int y = noSpawnArea[i][1];
				Game1.locations[1].setTileProperty(x, y, "Back", "NoSpawn", "All");
			}
		}

		public static void PatchMap(GameLocation gl, List<Tile> tileArray)
		{
			foreach (Tile tile in tileArray)
			{
				Layer layer = gl.map.GetLayer(tile.Layername);

				if (tile.tileIndex < 0)
				{
					gl.removeTile(tile.x, tile.y, tile.Layername);
					continue;
				}

				if (layer.Tiles[tile.x, tile.y] == null)
				{
					layer.Tiles[tile.x, tile.y] = new StaticTile(layer, gl.map.TileSheets[gl.map.TileSheets.Count-1], xTile.Tiles.BlendMode.Alpha, tile.tileIndex);
				}
				else
				{
					gl.setMapTileIndex(tile.x, tile.y, tile.tileIndex, layer.Id);
				}
			}
		}

		
	}

}
