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
		private ModConfig modConfig;

		private static List<Tile> FarmEdit1 = new List<Tile>()
		{

		};

		//Meticulously planned tile edits for Farm.xnb
		/*Create array of custom tile objects with layer, x & y location, and new texture information
            (-1 as a texture means set the tile to null) */
			/*
		private static List<Tile> FarmEdits = new List<Tile>()
		{
			new Tile(0, 0, 38, 175), new Tile(0, 1, 38, 175), new Tile(0, 0, 43, 537),
			new Tile(0, 1, 43, 537), new Tile(0, 2, 43, 586), new Tile(0, 0, 44, 566),
			new Tile(0, 1, 44, 537), new Tile(0, 2, 44, 618), new Tile(0, 0, 45, 587),
			new Tile(0, 1, 45, 473), new Tile(0, 0, 46, 587), new Tile(0, 1, 46, 587),
			new Tile(0, 0, 48, 175), new Tile(0, 1, 48, 175),

			new Tile(TLayer.Buildings, 0, 39, 175), new Tile(1, 1, 39, 175), new Tile(1, 2, 39, 444),
			new Tile(TLayer.Buildings, 0, 40, 446), new Tile(1, 1, 40, 468), new Tile(1, 2, 40, 469),
			new Tile(TLayer.Buildings, 0, 41, 492), new Tile(1, 1, 41, 493), new Tile(1, 2, 41, 494),
			new Tile(TLayer.Buildings, 0, 42, 517), new Tile(1, 1, 42, 518), new Tile(1, 2, 42, 519),
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
		*/


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

			//ModConfig config = helper.ReadConfig<ModConfig>();

			// Add Events
			SaveEvents.AfterLoad += SaveEvents_AfterLoad;
			SaveEvents.AfterSave += SaveEvents_AfterSave;
			SaveEvents.AfterReturnToTitle += SaveEvents_AfterReturnToTitle;
			TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;


			MapEditor.TreeArea._this = this;


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
			// Create/Read Config
			CreateConfig();

			// Add Map Edits
			RunPatches();
		}
		private void SaveEvents_AfterSave(object sender, EventArgs e)
		{
			// Save Config File
			if (modConfig != null)
				this.Helper.WriteJsonFile($"data/{Constants.SaveFolderName}.json", modConfig);
		}
		private void SaveEvents_AfterReturnToTitle(object sender, EventArgs e)
		{
			// Clear Config
			modConfig = null;
		}
		private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
		{
			// Check for New Game
			if (modConfig == null)
			{
				// Create/Read Config
				CreateConfig();

				// Add Map Edits
				RunPatches();
			}
		}

		/// <summary>
		/// Handles the Config Creation/Loading
		/// </summary>
		private void CreateConfig()
		{
			// Create/Read Config
			modConfig = this.Helper.ReadJsonFile<ModConfig>($"data/{Constants.SaveFolderName}.json") ?? new ModConfig();
			if (modConfig != null)
				// Write Config in case it's Needed
				this.Helper.WriteJsonFile($"data/{Constants.SaveFolderName}.json", modConfig);
		}
		
		/// <summary>
		/// Adds the Map Edits
		/// </summary>
		private void RunPatches()
		{
			this.Monitor.Log($"Tilesheet Count: {Game1.locations[1].map.TileSheets.Count}");


			//this.Monitor.Log($"{FarmEdits.Count}");
			this.Monitor.Log($"Adding Map Edits");

			if (modConfig.AddTreeSector1)
				AddSector1();

			if (modConfig.AddTreeSector2)
				AddSector2();

			if (modConfig.AddTreeSector3)
				AddSector3();

			if (modConfig.AddTreeSector4)
				AddSector4();

			if (modConfig.AddTreeSector5)
				AddSector5();

			if (modConfig.AddTreeSector6)
				AddSector6();

			if (modConfig.AddTreeSector7)
				AddSector7();

			if (modConfig.AddTreeSector8)
				AddSector8();

			if (modConfig.AddTreeSector9)
				AddSector9();


			AddMine();

			this.Monitor.Log($"Finished Adding Tree Sectors");
			//MapEditor.TreeArea.PatchMap(Game1.locations[1], tileEdits);
		}


		// Tree Sectors
		private void AddSector1()
		{
			this.Monitor.Log($"Adding Sector 1");
			// Variables
			int startX = 69;
			int startY = 74;
			int width = 3;
			int height = 14;

			// Normal Edits
			MapEditor.TreeArea.FillSquareArea(startX, startY, width, height, true, false, false, false, false, false, false, false);
			
			// Custom Patch Needed for this sector
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
		}
		private void AddSector2()
		{
			this.Monitor.Log($"Adding Sector 2");
			MapEditor.TreeArea.FillSquareArea(52, 80, 4, 11, true, true, true, true, true, true, true, true);
		}
		private void AddSector3()
		{
			this.Monitor.Log($"Adding Sector 3");
			MapEditor.TreeArea.FillSquareArea(43, 70, 5, 1, false, false, false, false, false, false, false, false);
		}
		private void AddSector4()
		{
			this.Monitor.Log($"Adding Sector 4");
			MapEditor.TreeArea.FillSquareArea(34, 61, 3, 3, false, false, false, false, false, false, false, false);
		}

		// New Check for issues
		private void AddSector5()
		{
			this.Monitor.Log($"Adding Sector 5");
			MapEditor.TreeArea.FillSquareArea(19, 67, 3, 1, false, false, false, false, false, false, false, false);
		}
		private void AddSector6()
		{
			this.Monitor.Log($"Adding Sector 6");
			MapEditor.TreeArea.FillSquareArea(81, 25, 3, 1, false, false, false, false, false, false, false, false);
		}
		private void AddSector7()
		{
			this.Monitor.Log($"Adding Sector 7");
			MapEditor.TreeArea.FillSquareArea(103, 27, 3, 4, false, false, false, false, false, false, false, false);
			MapEditor.TreeArea.FillSquareArea(112, 33, 1, 2, false, false, false, false, false, false, false, false);
		}
		private void AddSector8()
		{
			this.Monitor.Log($"Adding Sector 8");
			MapEditor.TreeArea.FillSquareArea(104, 111, 5, 1, false, false, false, false, false, false, false, false);
		}
		private void AddSector9()
		{
			this.Monitor.Log($"Adding Sector 9");
			MapEditor.TreeArea.FillSquareArea(70, 41, 1, 3, false, false, false, false, false, false, false, false);
		}

		private void AddMine()
		{
			AddMineBack();
			AddMineBuilding();

			int startX	= 5;
			int startY	= 94;
			int width	= 21;
			int height	= 22;
			
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Game1.locations[1].setTileProperty(startX + x, startY + y, "Back", "NoSpawn", "Tree");
				}
			}

			Game1.locations[1].setTileProperty(4, 97, "Back", "NPCBarrier", "T");
			Game1.locations[1].setTileProperty(25, 116, "Back", "NPCBarrier", "T");
			for (int x = 0; x < width; x++)
			{
				Game1.locations[1].setTileProperty(4, 116, "Back", "NPCBarrier", "T");
			}
		}
		private void AddMineBack()
		{
			List<Tile> tileEdits = new List<Tile>();
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 94, 1, 3, 175, 0);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 114, 1, 2, 175, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 115, 1, 1, 175, 0);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 94, 1, 1, 250, 0);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 94, 19, 1, 251, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 95, 20, 1, 175, 0);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 26, 94, 1, 1, 252, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 26, 95, 1, 20, 225, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 26, 115, 1, 1, 250, 0);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 96, 7, 1, 175, 0);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 96, 8, 1, 175, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 97, 1, 19, 175, 0);
			


			// Stairs
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 96, 1, 5, 339, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 14, 96, 1, 5, 338, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 15, 96, 1, 5, 339, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 16, 96, 1, 5, 338, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 96, 1, 5, 339, 0);

			// Grass (Beside Stairs
			MapEditor.TreeArea.AddTileSquare(tileEdits, 12, 97, 1, 3, 175, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 97, 1, 3, 175, 0);

			// Soil
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 97,   7, 17, 680, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 114,  6,  1, 680, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 115, 18,  1, 680, 0);

			

			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 97, 6, 18, 680, 0);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 12, 100, 7, 15, 680, 0);

			// Finalize Edits
			MapEditor.TreeArea.PatchMap(Game1.locations[1], tileEdits);

		}
		private void AddMineBuilding()
		{
			List<Tile> tileEdits = new List<Tile>();

			// Left Side
			MapEditor.TreeArea.AddTileSquare(tileEdits,  5,  97, 1,  1, 294, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits,  5,   98, 1,  1, 319, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  5,   99, 1,  1, 344, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  5,  100, 1,  1, 369, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  5,  101, 1, 13, 364, TLayer.Buildings);

			// Bottom
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6, 114,  1,  1, 389, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  7, 115,  1,  1, 390, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  8, 115, 17,  1, 414, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 115,  1,  1, 415, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6, 116, 20,  1, 175, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6, 115,  1,  1, 175, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  5, 114,  1,  1, 175, TLayer.Front);

			// Impassible Layer
			MapEditor.TreeArea.AddTileSquare(tileEdits,  4,  97,  1, 20, 175, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  5, 115,  2,  1, 175, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  7, 116, 19,  1, 175, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits,  5, 114,  1,  1, 175, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 115,  1,  1, 415, TLayer.Buildings);

			// Top
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6,  97, 1,  1, 295, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6,  98, 1,  1, 320, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6,  99, 1,  1, 345, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  6, 100, 1,  1, 370, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits,  7,  97, 6,  1, 468, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  7,  98, 6,  1, 493, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  7,  99, 6,  1, 518, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits,  7, 100, 6,  1, 543, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 13,  96, 1,  1, 444, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13,  97, 1,  1, 469, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13,  98, 1,  1, 494, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13,  99, 1,  1, 519, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 100, 1,  1, 544, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 17,  96, 1,  1, 441, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17,  97, 1,  1, 466, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17,  98, 1,  1, 491, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17,  99, 1,  1, 516, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 100, 1,  1, 541, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 18,  97, 1,  1, 467, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18,  98, 1,  1, 492, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18,  99, 1,  1, 517, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 100, 1,  1, 542, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 19,  97, 5,  1, 468, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 19,  98, 5,  1, 493, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 19,  99, 5,  1, 518, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 100, 5,  1, 543, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 24,  97, 1,  1, 290, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 24,  98, 1,  1, 315, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 24,  99, 1,  1, 340, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 24, 100, 1,  1, 365, TLayer.Buildings);


			// Right
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25,  97, 1,  1, 291, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25,  98, 1,  1, 316, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25,  99, 1,  1, 341, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 100, 1, 15, 366, TLayer.Buildings);


			// Finalize Edits
			MapEditor.TreeArea.PatchMap(Game1.locations[1], tileEdits);
		}
	}
}
 