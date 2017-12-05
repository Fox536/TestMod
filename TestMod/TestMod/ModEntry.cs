using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using xTile.Tiles;

namespace TestMod
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		private static List<Tile> FarmEdit1 = new List<Tile>()
		{

		};

		//Meticulously planned tile edits for Farm.xnb
		/*Create array of custom tile objects with layer, x & y location, and new texture information
            (-1 as a texture means set the tile to null) */
		private static List<Tile> FarmEdits = new List<Tile>()
		{
			new Tile(0, 0, 38, 175), new Tile(0, 1, 38, 175), new Tile(0, 0, 43, 537),
			new Tile(0, 1, 43, 537), new Tile(0, 2, 43, 586), new Tile(0, 0, 44, 566),
			new Tile(0, 1, 44, 537), new Tile(0, 2, 44, 618), new Tile(0, 0, 45, 587),
			new Tile(0, 1, 45, 473), new Tile(0, 0, 46, 587), new Tile(0, 1, 46, 587),
			new Tile(0, 0, 48, 175), new Tile(0, 1, 48, 175),

			new Tile(1, 0, 39, 175), new Tile(1, 1, 39, 175), new Tile(1, 2, 39, 444),
			new Tile(1, 0, 40, 446), new Tile(1, 1, 40, 468), new Tile(1, 2, 40, 469),
			new Tile(1, 0, 41, 492), new Tile(1, 1, 41, 493), new Tile(1, 2, 41, 494),
			new Tile(1, 0, 42, 517), new Tile(1, 1, 42, 518), new Tile(1, 2, 42, 519),
			new Tile(1, 0, 43, 542), new Tile(1, 1, 43, 543), new Tile(1, 2, 43, 544),
			new Tile(1, 0, 44, -1), new Tile(1, 1, 44, -1), new Tile(1, 2, 44, -1),
			new Tile(1, 0, 45, -1), new Tile(1, 1, 45, -1), new Tile(1, 2, 45, -1),
			new Tile(1, 0, 46, -1), new Tile(1, 1, 46, -1), new Tile(1, 2, 46, -1),
			new Tile(1, 0, 47, 175), new Tile(1, 1, 47, 175), new Tile(1, 0, 48, -1),

			new Tile(3, 0, 36, -1), new Tile(3, 1, 36, -1), new Tile(3, 0, 37, -1),
			new Tile(3, 1, 37, -1), new Tile(3, 0, 38, -1), new Tile(3, 1, 38, -1),
			new Tile(3, 0, 39, -1), new Tile(3, 1, 39, -1), new Tile(3, 0, 40, -1),
			new Tile(3, 0, 41, -1), new Tile(3, 0, 46, 414), new Tile(3, 1, 46, 413),
			new Tile(3, 2, 46, 438), new Tile(3, 0, 47, 175), new Tile(3, 1, 47, 175),
			new Tile(3, 2, 47, 394)
		};


		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{


			//or if you've created your own GameLocation object when you loaded your mod, just send that instead of referencing the Game1.locations array
			/*
            //These are random made up examples
            Game1.locations[1].warps.Add(new Warp(64, 19, "Farm", 62, 23, false));
            Game1.locations[1].setTileProperty(63, 13, "Buildings", "Action", "Warp 64 18 Farm");
            */

			SaveEvents.AfterLoad += SaveEvents_AfterLoad;

			//ControlEvents.KeyPressed += this.ControlEvents_KeyPress;
		}



		/*********
        ** Private methods
        *********/
		/// <summary>The method invoked when the player presses a keyboard button.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
		{
			if (Context.IsWorldReady) // save is loaded
			{
				this.Monitor.Log($"{Game1.player.name} pressed {e.KeyPressed}.");
			}
		}

		private void SaveEvents_AfterLoad(object sender, EventArgs e)
		{
			this.Monitor.Log($"Tilesheet Count: {Game1.locations[1].map.TileSheets.Count}");


			//this.Monitor.Log($"{FarmEdits.Count}");
			this.Monitor.Log($"Adding Map Edits");

			this.Monitor.Log($"Adding Sector 1");
			AddSector1();
			this.Monitor.Log($"Adding Sector 2");
			AddSector2();
			this.Monitor.Log($"Adding Sector 3");
			AddSector3();
			this.Monitor.Log($"Adding Sector 4");
			AddSector4();


			this.Monitor.Log($"Finished Adding Sectors");
			//MapEditor.TreeArea.PatchMap(Game1.locations[1], tileEdits);
		}

		private static void AddSector1()
		{
			int startX = 69;
			int startY = 74;
			int width = 3;
			int height = 14;

			MapEditor.TreeArea.FillSquareArea(startX, startY, width, height, true, false, false, false, false, false, false, false);

			// Custom Fix Needed
			// Create Array
			List<Tile> tileEdits = new List<Tile>();

			// Fix Right Side
			int rightSide = startX + width * 3;
			for (int y = 0; y < height; y++)
			{
				MapEditor.TreeArea.AddGrass(tileEdits, rightSide, startY + y);
			}
			// Add Left & Bottom Turn
			MapEditor.TreeArea.AddTurnLeftBottom(tileEdits, 68, 115);

			// Finalize Edits
			MapEditor.TreeArea.PatchMap(Game1.locations[1], tileEdits);

			return;
			/*
			// Create Array
			List<Tile> tileEdits = new List<Tile>();

			// Modding - Start Point
			int startX = 69;
			int startY = 74;
			
			// Modding - Add TileData Changes
			for (int x = 0; x < 3 * 3; x++)
			{
				for (int y = 0; y < 14 * 3; y++)
				{
					//squares.Add(new int[] { startX + x, startY + y});
					MapEditor.TreeArea.AddNoSpawn(tileEdits, Game1.locations[1], startX + x, startY + y);
				}
			}

			// Modding - Add TileId Changes
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 14; y++)
				{
					AddTreeArea_Center(tileEdits, startX + x * 3, startY + y * 3);
				}
			}

			// Modding - Add Sidding
			for (int y = 0; y < 14*3; y++)
			{
				AddTreeArea_LeftSide(tileEdits, startX - 1, startY + y);
				AddSquareNoSpawn(Game1.locations[1], startX - 1, startY + y);
			}

			// Modding - Add Fine Tuning
			AddTreeArea_LeftDownTurn(tileEdits, 68, 115);
			AddSquareNoSpawn(Game1.locations[1], 68, 115);
			//--------------------------------------------------------------------------

			// Finalize Edits
			//PatchMap(Game1.locations[1], tileEdits);

			return tileEdits;
			*/
		}
		private static void AddSector2()
		{
			MapEditor.TreeArea.FillSquareArea(52, 80, 4, 11, true, true, true, true, true, true, true, true);
		}

		private static void AddSector3()
		{
			MapEditor.TreeArea.FillSquareArea(43, 70, 5, 1, false, false, false, false, false, false, false, false);
		}
		private static void AddSector4()
		{
			MapEditor.TreeArea.FillSquareArea(34, 61, 3, 3, false, false, false, false, false, false, false, false);
		}


	}
}
 