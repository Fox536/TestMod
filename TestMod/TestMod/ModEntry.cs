using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using xTile.Tiles;
using xTile;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace TestMod
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod //, IAssetEditor
	{
		#region Variables
		private static string		ModVersion = "0.04";
		private static ModConfig	modConfig;
		private static Map			FarmMap;
		private static bool			mapUpdated = false;
		private static IMonitor		ModMonitor;
		#endregion

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

			// Add Events
			SaveEvents.AfterLoad += SaveEvents_AfterLoad;
			SaveEvents.AfterSave += SaveEvents_AfterSave;
			SaveEvents.AfterReturnToTitle += SaveEvents_AfterReturnToTitle;

			TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;

			ControlEvents.KeyPressed += this.ControlEvents_KeyPress;


			ModMonitor = this.Monitor;
		}

		/*********
        ** Private methods
        *********/
		#region Events
		/// <summary>The method invoked when the player presses a keyboard button.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
		{
			if (Context.IsWorldReady) // save is loaded
			{
				//print($"{Game1.player.name} pressed {e.KeyPressed}.");
				if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.F7)
					print($"{Game1.player.currentLocation}: {Game1.player.currentLocation.Name}, {Game1.player.getTileLocation()}, {Game1.player.FacingDirection}");
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
			if (modConfig == null)
			{
				// Create/Read Config
				CreateConfig();
			}
			else
			{
				this.Helper.WriteJsonFile($"data/{Constants.SaveFolderName}.json", modConfig);
			}
		}
		private void SaveEvents_AfterReturnToTitle(object sender, EventArgs e)
		{
			// Clear Config
			modConfig = null;
			mapUpdated = false;
		}
		private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
		{
			// Check for New Game
			if (modConfig == null)
			{
				// Create/Read Config
				CreateConfig();
			}

			// Add Map Edits
			RunPatches();

			AddObjects();
		}
		#endregion

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
			if (!mapUpdated)
			{
				if (Game1.locations[1] is Farm)
				{
					Farm farm = Game1.locations[1] as Farm;

					// Insure that the player is using the correct Map
					if (Game1.whichFarm == 4)
					{
						print("Starting Map Override");
						FarmMap = this.Helper.Content.Load<Map>(@"Content\Maps\Farm_Combat_Fox536.xnb", ContentSource.ModFolder);
						farm.Map = FarmMap;

						for (int i = 0; i < farm.Map.TileSheets.Count; i++)
						{
							print($"{farm.Map.TileSheets[i]}, {farm.Map.TileSheets[i].Id}, {farm.Map.TileSheets[i].ImageSource}");
						}

						// Update Seasonal Tileset
						UpdateSeasonalTileset(farm);

						print($"Adding Map Edits");

						if (modConfig.AddTreePlot1)
							AddTreePlots1(farm);

						if (modConfig.AddTreePlot2)
							AddTreePlots2(farm);

						if (modConfig.AddTreePlot3)
							AddTreePlots3(farm);

						if (modConfig.AddTreePlot4)
							AddTreePlots4(farm);

						if (modConfig.AddTreePlot5)
							AddTreePlots5(farm);

						if (modConfig.AddTreePlot6)
							AddTreePlots6(farm);

						if (modConfig.AddTreePlot7)
							AddTreePlots7(farm);

						if (modConfig.AddTreePlot8)
							AddTreePlots8(farm);

						if (modConfig.AddTreePlot9)
							AddTreePlots9(farm);

						if (modConfig.AddMineArea)
							AddMine(farm);

						// Bridge Replacer
						//ReplaceBridge(farm);
						
						print($"Finished Adding Map Edits");
					}
					
					// Add Farm Expansion
					AddFarmExpansion(farm);
				}

				// Prevent Another Update
				mapUpdated = true;
			}

			if (Game1.locations[1] is Farm)
			{
				Farm farm = Game1.locations[1] as Farm;
				UpdateSeasonalTileset(farm);
			}
		}

		private void UpdateSeasonalTileset(GameLocation farm)
		{
			// Update Seasonal Tileset
			//farm.seasonUpdate(Game1.currentSeason);
			foreach (TileSheet tilesheet in farm.Map.TileSheets)
			{
				if (tilesheet.ImageSource.Contains("spring_outdoorsTileSheet"))
					tilesheet.ImageSource= Game1.currentSeason + "_outdoorsTileSheet";
			}
			print(Game1.currentSeason + "_outdoorsTileSheet");
		}

		// Tree Sectors
		#region TreePlots
		/// <summary>
		/// Adds the Tree Plot #1
		/// </summary>
		private void AddTreePlots1(Farm farm)
		{
			print($"Adding Sector 1");
			// Variables
			int startX = 69;
			int startY = 74;
			int width = 3;
			int height = 14;

			// Normal Edits
			MapEditor.TreeArea.FillSquareArea(farm, startX, startY, width, height, true, false, false, false, false, false, false, false);

			// Custom Patch Needed for this sector
			// Create Array
			List<Tile> tileEdits = new List<Tile>();

			// Fix Right Side
			int rightSide = startX + width * 3;
			for (int y = 0; y < height * 3; y++)
			{
				MapEditor.TreeArea.AddGrass(tileEdits, rightSide, startY + y);
			}
			// Add Left & Bottom Turn
			MapEditor.TreeArea.AddTurnLeftBottom(tileEdits, 68, 115);

			// Finalize Edits
			MapEditor.TreeArea.PatchMap(farm, tileEdits);
		}
		/// <summary>
		/// Adds the Tree Plot #2
		/// </summary>
		private void AddTreePlots2(Farm farm)
		{
			print($"Adding Sector 2");
			MapEditor.TreeArea.FillSquareArea(farm, 52, 80, 4, 11, true, true, true, true, true, true, true, true);
		}
		/// <summary>
		/// Adds the Tree Plot #3
		/// </summary>
		private void AddTreePlots3(Farm farm)
		{
			print($"Adding Sector 3");
			MapEditor.TreeArea.FillSquareArea(farm, 43, 70, 5, 1, false, false, false, false, false, false, false, false);
		}
		/// <summary>
		/// Adds the Tree Plot #4
		/// </summary>
		private void AddTreePlots4(Farm farm)
		{
			print($"Adding Sector 4");
			MapEditor.TreeArea.FillSquareArea(farm, 34, 61, 3, 3, false, false, false, false, false, false, false, false);
		}
		/// <summary>
		/// Adds the Tree Plot #5
		/// </summary>
		private void AddTreePlots5(Farm farm)
		{
			print($"Adding Sector 5");
			MapEditor.TreeArea.FillSquareArea(farm, 19, 67, 3, 1, false, false, false, false, false, false, false, false);
		}
		/// <summary>
		/// Adds the Tree Plot #6
		/// </summary>
		private void AddTreePlots6(Farm farm)
		{
			print($"Adding Sector 6");
			MapEditor.TreeArea.FillSquareArea(farm, 81, 25, 3, 1, false, false, false, false, false, false, false, false);
		}
		/// <summary>
		/// Adds the Tree Plot #7
		/// </summary>
		private void AddTreePlots7(Farm farm)
		{
			print($"Adding Sector 7");
			MapEditor.TreeArea.FillSquareArea(farm, 103, 27, 3, 4, false, false, false, false, false, false, false, false);
			MapEditor.TreeArea.FillSquareArea(farm, 112, 33, 1, 2, false, false, false, false, false, false, false, false);
		}
		/// <summary>
		/// Adds the Tree Plot #8
		/// </summary>
		private void AddTreePlots8(Farm farm)
		{
			print($"Adding Sector 8");
			MapEditor.TreeArea.FillSquareArea(farm, 104, 111, 5, 1, false, false, false, false, false, false, false, false);
		}
		/// <summary>
		/// Adds the Tree Plot #9
		/// </summary>
		private void AddTreePlots9(Farm farm)
		{
			print($"Adding Sector 9");
			MapEditor.TreeArea.FillSquareArea(farm, 70, 41, 1, 3, false, false, false, false, false, false, false, false);
		}
		#endregion
		#region MineArea
		/// <summary>
		/// Patches the map, to add the secondary Mine Area.
		/// </summary>
		private void AddMine(Farm farm)
		{
			AddMineBack(farm);
			AddMineBuilding(farm);
			AddMineFront(farm);

			// Add NoSpawn to prevent Trees from spawning in the mine.
			int startX = 5;
			int startY = 94;
			int width = 21;
			int height = 22;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					farm.setTileProperty(startX + x, startY + y, "Back", "NoSpawn", "Tree");
				}
			}

			// Add NoSpawn and NPC Barrier to Prevent issues around the mine.
			farm.setTileProperty(4, 97, "Back", "NPCBarrier", "T");
			farm.setTileProperty(4, 97, "Back", "NoSpawn", "ALL");

			farm.setTileProperty(25, 116, "Back", "NPCBarrier", "T");
			farm.setTileProperty(25, 116, "Back", "NoSpawn", "ALL");
			for (int x = 0; x < width; x++)
			{
				farm.setTileProperty(4 + x, 116, "Back", "NPCBarrier", "T");
				farm.setTileProperty(4 + x, 116, "Back", "NoSpawn", "ALL");
			}
		}
		/// <summary>
		/// Patches the map to add the Back Layer of the Mine.
		/// </summary>
		private void AddMineBack(Farm farm)
		{
			List<Tile> tileEdits = new List<Tile>();
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 94, 1, 3, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 114, 1, 2, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 115, 1, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 94, 1, 1, 250, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 94, 19, 1, 251, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 95, 20, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 26, 94, 1, 1, 252, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 26, 95, 1, 20, 225, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 26, 115, 1, 1, 250, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 96, 7, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 96, 8, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 97, 1, 19, 175, TLayer.Back);



			// Stairs
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 96, 1, 5, 339, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 14, 96, 1, 5, 338, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 15, 96, 1, 5, 339, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 16, 96, 1, 5, 338, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 96, 1, 5, 339, TLayer.Back);

			// Grass (Beside Stairs
			MapEditor.TreeArea.AddTileSquare(tileEdits, 12, 97, 1, 3, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 97, 1, 3, 175, TLayer.Back);

			// Soil
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 97, 7, 17, 680, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 114, 6, 1, 680, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 115, 18, 1, 680, TLayer.Back);



			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 97, 6, 18, 680, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 12, 100, 7, 15, 680, TLayer.Back);

			// Finalize Edits
			MapEditor.TreeArea.PatchMap(farm, tileEdits);

		}
		/// <summary>
		/// Patches the map to add the Building Layer of the Mine.
		/// </summary>
		private void AddMineBuilding(Farm farm)
		{
			List<Tile> tileEdits = new List<Tile>();

			// Left Side
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 97, 1, 1, 294, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 98, 1, 1, 319, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 99, 1, 1, 344, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 100, 1, 1, 369, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 101, 1, 13, 364, TLayer.Buildings);

			// Impassible Layer
			MapEditor.TreeArea.AddTileSquare(tileEdits, 4, 97, 1, 20, 175, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 115, 2, 1, 175, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 116, 19, 1, 175, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 114, 1, 1, 175, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 115, 1, 1, 415, TLayer.Buildings);

			// Top
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 97, 1, 1, 295, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 98, 1, 1, 320, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 99, 1, 1, 345, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 100, 1, 1, 370, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 97, 6, 1, 468, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 98, 6, 1, 493, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 99, 6, 1, 518, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 100, 6, 1, 543, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 96, 1, 1, 444, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 97, 1, 1, 469, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 98, 1, 1, 494, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 99, 1, 1, 519, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 13, 100, 1, 1, 544, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 96, 1, 1, 441, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 97, 1, 1, 466, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 98, 1, 1, 491, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 99, 1, 1, 516, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 17, 100, 1, 1, 541, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 97, 1, 1, 467, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 98, 1, 1, 492, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 99, 1, 1, 517, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 18, 100, 1, 1, 542, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 97, 5, 1, 468, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 98, 5, 1, 493, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 99, 5, 1, 518, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 19, 100, 5, 1, 543, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 24, 97, 1, 1, 290, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 24, 98, 1, 1, 315, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 24, 99, 1, 1, 340, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 24, 100, 1, 1, 365, TLayer.Buildings);


			// Right
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 97, 1, 1, 291, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 98, 1, 1, 316, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 99, 1, 1, 341, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 100, 1, 15, 366, TLayer.Buildings);


			// Finalize Edits
			MapEditor.TreeArea.PatchMap(farm, tileEdits);
		}
		/// <summary>
		/// Patches the map to add the Front Layer of the Mine.
		/// </summary>
		private void AddMineFront(Farm farm)
		{
			List<Tile> tileEdits = new List<Tile>();

			// Bottom
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 114, 1, 1, 389, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 7, 115, 1, 1, 390, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 8, 115, 17, 1, 414, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 25, 115, 1, 1, 415, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 116, 20, 1, 175, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 6, 115, 1, 1, 175, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 5, 114, 1, 1, 175, TLayer.Front);

			// Finalize Edits
			MapEditor.TreeArea.PatchMap(farm, tileEdits);
		}
		#endregion
		
		// Not working yet...
		// for some reason it causes passability issues
		private void ReplaceBridge(Farm farm)
		{
			List<Tile> tileEdits = new List<Tile>();

			// Layer - Back
			// New Stuff
			MapEditor.TreeArea.AddTileSquare(tileEdits, 61, 56, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 56, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 63, 56, 1, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 59, 70, 1, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 69, 60, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 69, 61, 1, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 70, 71, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 74, 71, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 75, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 76, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 78, 72, 1, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 70, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 71, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 72, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 73, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 74, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 75, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 76, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 77, 72, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 78, 72, 1, 1, 175, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 71, 1, 1, 175, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 63, 71, 1, 1, 175, TLayer.Back);
			
			// Dirt - Left
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 60, 1, 13, 225, TLayer.Back);

			// Dirt - Middle
			MapEditor.TreeArea.AddTileSquare(tileEdits, 65, 60, 3, 11, 227, TLayer.Back);

			// Dirt - Right
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 60, 1, 11, 228, TLayer.Back);

			// Grass Area
			MapEditor.TreeArea.AddTileSquare(tileEdits, 65, 54, 1, 1, 251, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 66, 54, 1, 1, 251, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 54, 1, 1, 253, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 53, 1, 1, 178, TLayer.Back);
			
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 55, 1, 5, 338, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 65, 55, 2, 5, 339, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 55, 1, 5, 338, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 63, 60, 1, 1, 352, TLayer.Back);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 65, 71, 1, 3, 338, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 66, 71, 2, 3, 339, TLayer.Back);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 71, 1, 3, 338, TLayer.Back);


			// Layer - Building
			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 56, 1, 1, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 63, 56, 1, 1, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 61, 1, 9, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 61, 1, 9, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 69, 68, 2, 2, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 65, 73, 3, 1, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 69, 71, 1, 3, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 70, 72, 1, 9, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 74, 71, 2, 1, -1, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 71, 2, 1, -1, TLayer.Buildings);


			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 57, 2, 1, 468, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 58, 2, 1, 493, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 59, 2, 1, 518, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 62, 60, 2, 1, 543, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 55, 1, 1, 438, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 56, 1, 1, 444, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 57, 1, 1, 469, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 58, 1, 1, 494, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 59, 1, 1, 519, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 60, 1, 1, 544, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 54, 1, 1, 439, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 55, 1, 1, 416, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 56, 1, 1, 441, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 57, 1, 1, 466, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 58, 1, 1, 491, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 59, 1, 1, 516, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 67, 60, 1, 1, 541, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 54, 1, 1, 369, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 55, 1, 1, 369, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 56, 1, 1, 444, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 57, 1, 1, 469, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 58, 1, 1, 494, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 59, 1, 1, 519, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 60, 1, 1, 544, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 58, 70, 1, 1, 412, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 59, 70, 5, 1, 413, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 70, 1, 1, 438, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 71, 1, 3, 419, TLayer.Buildings);

			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 70,  1, 1, 439, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 71,  1, 3, 416, TLayer.Buildings);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 69, 70, 10, 1, 413, TLayer.Buildings);
			
			MapEditor.TreeArea.AddTileSquare(tileEdits, 79, 71,  1, 2, 419, TLayer.Buildings);
			
			// Layer - Front
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 72, 1, 1,  -1, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 72, 1, 1,  -1, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 69, 69, 1, 1,  -1, TLayer.Front);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 79, 70, 1, 1, 438, TLayer.Front);

			// Layer - AlwaysFront
			MapEditor.TreeArea.AddTileSquare(tileEdits, 64, 55, 1, 5, -1, TLayer.AlwaysFront);
			MapEditor.TreeArea.AddTileSquare(tileEdits, 68, 55, 1, 5, -1, TLayer.AlwaysFront);


			// Finalize Edits
			MapEditor.TreeArea.PatchMap(farm, tileEdits);

			// Add Passable Edits

			for (int y = 0; y < 9; y++)
			{
				farm.setTileProperty(64, 61 + y, "Back", "Passable", "T");
				farm.setTileProperty(68, 61 + y, "Back", "Passable", "T");
			}
			farm.setTileProperty(69, 68, "Back", "Passable", "T");
			farm.setTileProperty(70, 68, "Back", "Passable", "T");
			farm.setTileProperty(69, 69, "Back", "Passable", "T");
			farm.setTileProperty(70, 69, "Back", "Passable", "T");

		}

		#region Daily Spawns
		/// <summary>
		/// Adds Spawnable Objects
		/// </summary>
		private void AddObjects()
		{
			Farm farm = (Farm)Game1.locations[1];
			if (modConfig.doSpawnOre)
				AddMineObjs(farm);

			if (modConfig.doSpawnBoulders)
				SpawnBoulders(farm);

			if (modConfig.doSpawnTrees)
				SpawnTrees(farm);

			if (modConfig.doSpawnStumps)
				SpawnStumps(farm);

			if (modConfig.doSpawnLogs)
				SpawnLogs(farm);
		}

		private void SpawnBoulders(Farm farm)
		{
			foreach (Vector2 point in modConfig.BoulderLocations)
			{
				SpawnBoulder(farm, point);
			}
		}
		private void SpawnBoulder(Farm farm, Vector2 point)
		{
			ClearResourceClump(ref farm.resourceClumps, point);
			farm.addResourceClumpAndRemoveUnderlyingTerrain(ResourceClump.boulderIndex, 2, 2, point);
		}

		private void SpawnTrees(Farm farm)
		{
			// Add Config Option Here
			foreach (Vector2 point in modConfig.TreeLocations)
			{
				SpawnTree(farm, point);
			}
		}
		private void SpawnTree(Farm farm, Vector2 point)
		{
			StardewValley.TerrainFeatures.Tree t = new Tree(1, 5);
			t.seasonUpdate(true);
			ClearResourceClump(ref farm.resourceClumps, point);
			TerrainFeature feature = null;
			if (farm.terrainFeatures.TryGetValue(point, out feature))
			{
				if (feature.GetType() != t.GetType())
				{
					farm.terrainFeatures.Clear();
					farm.terrainFeatures.Add(point, t);
				}
			}
			else
				farm.terrainFeatures.Add(point, t);
		}

		private void SpawnLogs(Farm farm)
		{
			foreach (Vector2 point in modConfig.LogLocations)
			{
				SpawnLog(farm, point);
			}
		}
		private void SpawnLog(Farm farm, Vector2 point)
		{
			ClearResourceClump(ref farm.resourceClumps, point);
			farm.addResourceClumpAndRemoveUnderlyingTerrain(ResourceClump.hollowLogIndex, 2, 2, point);
		}

		private void SpawnStumps(Farm farm)
		{
			foreach (Vector2 point in modConfig.StumpLocations)
			{
				SpawnStump(farm, point);
			}
		}
		private void SpawnStump(Farm farm, Vector2 point)
		{
			ClearResourceClump(ref farm.resourceClumps, point);
			farm.addResourceClumpAndRemoveUnderlyingTerrain(ResourceClump.stumpIndex, 2, 2, point);
		}
		
		private void SpawnGrassPoints(Farm farm)
		{
			foreach (Vector2 point in modConfig.GetGrassArea())
			{
				SpawnGrass(farm, point);
			}
		}
		private void SpawnGrass(Farm farm, Vector2 point)
		{
			TerrainFeature check;
			if (farm.terrainFeatures.TryGetValue(point, out check))
			{
				if (check is Grass)
				{
					((Grass)check).numberOfWeeds = 4;
				}
			}
			else
			{
				farm.terrainFeatures.Add(point, new Grass(Grass.springGrass, 4));
			}

		}
		#endregion

		#region Mine Methods
		private void AddMineObjs(Farm farm)
		{
			// Create Mine Area if needed
			// Mine Area
			if (modConfig.AddMineArea)
			{
				Random randomGen = new Random();
				foreach (Vector2 tile in modConfig.GetMineArea())
				{
					//calculate ore spawn
					if (Game1.player.hasSkullKey)
					{
						//5% chance of spawn ore
						if (randomGen.NextDouble() < modConfig.oreChance)
						{
							addRandomOre(ref farm, ref randomGen, 4, tile);
							continue;
						}
					}
					else
					{
						//check mine level
						if (Game1.player.deepestMineLevel > 80) //gold level
						{
							if (randomGen.NextDouble() < modConfig.oreChance)
							{
								addRandomOre(ref farm, ref randomGen, 3, tile);
								continue;
							}
						}
						else if (Game1.player.deepestMineLevel > 40) //iron level
						{
							if (randomGen.NextDouble() < modConfig.oreChance)
							{
								addRandomOre(ref farm, ref randomGen, 2, tile);
								continue;
							}
						}
						else
						{
							if (randomGen.NextDouble() < modConfig.oreChance)
							{
								addRandomOre(ref farm, ref randomGen, 1, tile);
								continue;
							}
						}
					}

					//if ore doesnt spawn then calculate gem spawn
					//1% to spawn gem
					if (randomGen.NextDouble() < modConfig.gemChance)
					{
						//0.1% chance of getting mystic stone
						if (Game1.player.hasSkullKey)
							if (randomGen.Next(0, 100) < 1)
							{
								farm.setObject(tile, createOre("mysticStone", tile));
								continue;
							}
							else
							if (randomGen.Next(0, 500) < 1)
							{
								farm.setObject(tile, createOre("mysticStone", tile));
								continue;
							}

						switch (randomGen.Next(0, 100) % 8)
						{
							case 0: farm.setObject(tile, createOre("gemStone", tile)); break;
							case 1: farm.setObject(tile, createOre("diamond", tile)); break;
							case 2: farm.setObject(tile, createOre("ruby", tile)); break;
							case 3: farm.setObject(tile, createOre("jade", tile)); break;
							case 4: farm.setObject(tile, createOre("amethyst", tile)); break;
							case 5: farm.setObject(tile, createOre("topaz", tile)); break;
							case 6: farm.setObject(tile, createOre("emerald", tile)); break;
							case 7: farm.setObject(tile, createOre("aquamarine", tile)); break;
							default: break;
						}
						continue;
					}
				}
			}
		}
		static void ClearResourceClump(ref List<ResourceClump> input, Vector2 RCLocation)
		{
			for (int i = 0; i < input.Count; i++)
			{
				ResourceClump RC = input[i];
				if (RC.tile == RCLocation)
				{
					input.RemoveAt(i);
					i--;
				}
			}
		}
		static void addRandomOre(ref Farm input, ref Random randomGen, int highestOreLevel, Vector2 tileLocation)
		{
			switch (randomGen.Next(0, 100) % highestOreLevel)
			{
				case 0: input.setObject(tileLocation, createOre("copperStone", tileLocation)); break;
				case 1: input.setObject(tileLocation, createOre("ironStone", tileLocation)); break;
				case 2: input.setObject(tileLocation, createOre("goldStone", tileLocation)); break;
				case 3: input.setObject(tileLocation, createOre("iridiumStone", tileLocation)); break;
				default: break;
			}
		}
		static StardewValley.Object createOre(string oreName, Vector2 tileLocation)
		{
			switch (oreName)
			{
				case "mysticStone":
					return new StardewValley.Object(tileLocation, 46, "Stone", true, false, false, false);
				case "gemStone":
					return new StardewValley.Object(tileLocation, (Game1.random.Next(7) + 1) * 2, "Stone", true, false, false, false);
				case "diamond":
					return new StardewValley.Object(tileLocation, 2, "Stone", true, false, false, false);
				case "ruby":
					return new StardewValley.Object(tileLocation, 4, "Stone", true, false, false, false);
				case "jade":
					return new StardewValley.Object(tileLocation, 6, "Stone", true, false, false, false);
				case "amethyst":
					return new StardewValley.Object(tileLocation, 8, "Stone", true, false, false, false);
				case "topaz":
					return new StardewValley.Object(tileLocation, 10, "Stone", true, false, false, false);
				case "emerald":
					return new StardewValley.Object(tileLocation, 12, "Stone", true, false, false, false);
				case "aquamarine":
					return new StardewValley.Object(tileLocation, 14, "Stone", true, false, false, false);
				case "iridiumStone":
					return new StardewValley.Object(tileLocation, 765, 1);
				case "goldStone":
					return new StardewValley.Object(tileLocation, 764, 1);
				case "ironStone":
					return new StardewValley.Object(tileLocation, 290, 1);
				case "copperStone":
					return new StardewValley.Object(tileLocation, 751, 1);
				default:
					return null;
			}
		}
		#endregion

		#region FarmExpansionPatch
		private void AddFarmExpansion(Farm farm)
		{
			bool isLoaded = this.Helper.ModRegistry.IsLoaded("Advize.FarmExpansion");
			print("Mod loaded: " + isLoaded.ToString());

			if (!isLoaded)
				return;

			if (!modConfig.UsingFarmExpansionPatch)
				return;

			print("Loading FarmExpansionPatch");

			int tsID = 0;
			List<Tile> tiles = new List<Tile>();

			tiles.Add(new Tile(TLayer.Back, 2, 33, 346, tsID)); tiles.Add(new Tile(TLayer.Back, 2, 34, 346, tsID)); tiles.Add(new Tile(TLayer.Back, 2, 35, 346, tsID));
			tiles.Add(new Tile(TLayer.Back, 0, 38, 537, tsID)); tiles.Add(new Tile(TLayer.Back, 1, 38, 537, tsID)); tiles.Add(new Tile(TLayer.Back, 2, 38, 618, tsID));
			tiles.Add(new Tile(TLayer.Back, 0, 39, 587, tsID)); tiles.Add(new Tile(TLayer.Back, 1, 39, 587, tsID)); tiles.Add(new Tile(TLayer.Back, 0, 41, 587, tsID));
			tiles.Add(new Tile(TLayer.Back, 1, 41, 587, tsID)); tiles.Add(new Tile(TLayer.Back, 2, 41, 587, tsID));

			tiles.Add(new Tile(TLayer.Buildings, 1, 33, 377, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 34, 175, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 34, 175, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 34, 444, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 35, 467, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 35, 468, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 35, 469, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 36, 492, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 36, 493, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 36, 371, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 37, 517, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 37, 518, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 37, 519, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 38, 542, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 38, 543, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 38, 544, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 39, -1, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 39, -1, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 39, -1, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 40, -1, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 40, -1, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 40, -1, tsID)); tiles.Add(new Tile(TLayer.Buildings, 0, 41, -1, tsID)); tiles.Add(new Tile(TLayer.Buildings, 1, 41, -1, tsID));
			tiles.Add(new Tile(TLayer.Buildings, 2, 41, -1, tsID));

			tiles.Add(new Tile(TLayer.Front, 0, 35, -1, tsID)); tiles.Add(new Tile(TLayer.Front, 2, 36, 494, tsID));

			tiles.Add(new Tile(TLayer.Back, 3, 39, 206, tsID));

			// Finalize Edits
			MapEditor.TreeArea.PatchMap(farm, tiles);

			if (modConfig.AddBothEntrances)
			{
				GameLocation farmExpansion = null;
				foreach (GameLocation location in Game1.locations)
				{
					if (location.Name == "FarmExpansion")
					{
						print("expansion found!!!");
						farmExpansion = location;
						break;
					}
				}
				if (farmExpansion != null)
				{
					print("expansion edit start");
					tiles = new List<Tile>();

					int width  = modConfig.FarmExpansionWidth - 1;
					int height = modConfig.FarmExpansionHeight - 1;

					print($"width: {width}, height: {height}");
					
					tiles.Add(new Tile(TLayer.Back, width - 3, 49, 617, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 2, 49, 570, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 1, 49, 568, tsID));
					tiles.Add(new Tile(TLayer.Back, width, 49, 567, tsID));

					tiles.Add(new Tile(TLayer.Back, width - 3, 50, 433, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 2, 50, 433, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 1, 50, 433, tsID));
					tiles.Add(new Tile(TLayer.Back, width, 50, 433, tsID));

					tiles.Add(new Tile(TLayer.Back, width - 3, 51, 616, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 2, 51, 745, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 1, 51, 745, tsID));
					tiles.Add(new Tile(TLayer.Back, width, 51, 745, tsID));

					tiles.Add(new Tile(TLayer.Back, width - 2, 52, 683, tsID));
					tiles.Add(new Tile(TLayer.Back, width - 1, 52, 684, tsID));
					tiles.Add(new Tile(TLayer.Back, width, 52, 684, tsID));


					// Building Layer
					tiles.Add(new Tile(TLayer.Buildings, width - 2, 48, 411, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width - 1, 48, 358, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width, 48, 360, tsID));

					tiles.Add(new Tile(TLayer.Buildings, width - 2, 49, 436, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width - 1, 49, 383, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width, 49, 385, tsID));

					tiles.Add(new Tile(TLayer.Buildings, width - 2, 50, -1, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width - 1, 50, -1, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width, 50, -1, tsID));

					tiles.Add(new Tile(TLayer.Buildings, width - 2, 51, -1, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width - 1, 51, -1, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width, 51, -1, tsID));

					tiles.Add(new Tile(TLayer.Buildings, width - 2, 52, 361, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width - 1, 52, 358, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width, 52, 360, tsID));

					tiles.Add(new Tile(TLayer.Buildings, width - 1, 53, 383, tsID));
					tiles.Add(new Tile(TLayer.Buildings, width, 53, 385, tsID));

					// Front
					tiles.Add(new Tile(TLayer.Front, width - 1, 45, -1, tsID));
					tiles.Add(new Tile(TLayer.Front, width, 45, -1, tsID));
					
					tiles.Add(new Tile(TLayer.Front, width - 1, 46, -1, tsID));
					tiles.Add(new Tile(TLayer.Front, width, 46, -1, tsID));

					tiles.Add(new Tile(TLayer.Front, width - 1, 47, -1, tsID));
					tiles.Add(new Tile(TLayer.Front, width, 47, -1, tsID));

					tiles.Add(new Tile(TLayer.Front, width - 1, 48, -1, tsID));
					tiles.Add(new Tile(TLayer.Front, width, 48, -1, tsID));

					// Finalize Edits
					MapEditor.TreeArea.PatchMap(farmExpansion, tiles);

					print("expansion edit finished");

					farm.warps.Add(new Warp(1, 39, "FarmExpansion", width - 1, 50, false));
					farm.warps.Add(new Warp(1, 40, "FarmExpansion", width - 1, 50, false));
					farm.warps.Add(new Warp(1, 41, "FarmExpansion", width - 1, 50, false));

					farmExpansion.warps.Add(new Warp(width, 50, "Farm", 2, 40, false));
					farmExpansion.warps.Add(new Warp(width, 51, "Farm", 2, 40, false));
				}
			}

		}
		#endregion

		/// <summary>
		/// Fix any Warp Points as needed
		/// </summary>
		static void ChangeWarpPoints()
		{
			foreach (GameLocation GL in Game1.locations)
			{
				if (modConfig.Warps_Forest.X != -1)
				{
					if (GL is Forest)
					{
						foreach (Warp w in GL.warps)
						{
							if (w.TargetName.ToLower().Contains("farm"))
							{
								w.TargetX = (int)modConfig.Warps_Forest.X;
								w.TargetY = (int)modConfig.Warps_Forest.Y;
							}
						}
					}
				}

				if (modConfig.Warps_Backwoods.X != -1)
				{
					if (GL.name.ToLower().Contains("backwood"))
					{
						foreach (Warp w in GL.warps)
						{
							if (w.TargetName.ToLower().Contains("farm"))
							{
								w.TargetX = (int)modConfig.Warps_Backwoods.X;
								w.TargetY = (int)modConfig.Warps_Backwoods.Y;
							}
						}
					}
				}

				if (modConfig.Warps_Busstop.X != -1)
				{
					if (GL.name.ToLower().Contains("busstop"))
					{
						foreach (Warp w in GL.warps)
						{
							if (w.TargetName.ToLower().Contains("farm"))
							{
								w.TargetX = (int)modConfig.Warps_Busstop.X;
								w.TargetY = (int)modConfig.Warps_Busstop.Y;
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Debug Print, requires modConfig.debug to equal true.
		/// </summary>
		/// <param name="text">Text to print.</param>
		private static void print(string text)
		{
			if ((ModMonitor != null) && (modConfig.debug))
			{
				ModMonitor.Log(text);
			}
		}
	}
}
 
