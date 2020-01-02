using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using xTile.Dimensions;
using xTile.Layers;
using xTile.Tiles;

namespace StardewElections
{
    public class ElectionAssetPlacement
    {
        IModHelper modHelper;

        //Asset Single Tiles
        TileSheet candidateBoxTile;

        public ElectionAssetPlacement(IModHelper helper)
        {
            modHelper = helper;
            helper.Events.GameLoop.SaveLoaded += LoadAllAssets;
            helper.Events.GameLoop.DayStarted += PlaceAssets;
        }

        private void LoadAllAssets(object sender, SaveLoadedEventArgs e)
        {
            GameLocation town = Game1.getLocationFromName("Town");
            string candidateBoxPath = modHelper.Content.GetActualAssetKey("assets/Candidate_Box.png",
                    ContentSource.ModFolder);

            candidateBoxTile = new TileSheet(
                    id: "z-candidate-box", // a unique ID for the tilesheet
                    map: town.map,
                    imageSource: candidateBoxPath,
                    sheetSize: new Size(32, 64), // the tile size of your tilesheet image.
                    tileSize: new Size(16, 16)
            );
        }

        private void PlaceAssets(object sender, DayStartedEventArgs e)
        {
            var initialEntryDate = new SDate(1, "spring");
            var finalEntryDate = new SDate(4, "spring");

            if (SDate.Now() >= initialEntryDate && SDate.Now() <= finalEntryDate)
            {
                Location candidateBoxPosition = new Location(10, 20);
                GameLocation town = Game1.getLocationFromName("Town");
                Layer buildingLayer = town.map.GetLayer("Buildings");

                town.map.AddTileSheet(candidateBoxTile);
                town.map.LoadTileSheets(Game1.mapDisplayDevice);

                buildingLayer.Tiles[candidateBoxPosition] = new
                    StaticTile(buildingLayer, candidateBoxTile, BlendMode.Alpha, 0);

                modHelper.Events.Input.ButtonPressed += CheckLocation;
            }
        }

        private void CheckLocation(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.IsActionButton() && e.Cursor.GrabTile.Equals(new Vector2(10,20))
                && Game1.player.currentLocation == Game1.getLocationFromName("Town"))
            {
                List<Response> responses = new List<Response>();
                responses.Add(new Response("100", "Yes"));
                responses.Add(new Response("200", "No"));

                Game1.drawObjectQuestionDialogue("Would you like to enter the elections?",
                    responses);

                //Todo: figure out how to act after a choice is selected

            }
        }
    }
}
