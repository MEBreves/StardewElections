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
        Vector2 boxPosition = new Vector2(39, 57);
        readonly SDate initialEntryDate = new SDate(1, "spring", 3);
        readonly SDate finalEntryDate = new SDate(4, "spring", 3);

        //Asset Single Tiles
        TileSheet candidateBoxTile;

        public void LoadAllAssets(IModHelper modHelper)
        {
            GameLocation town = Game1.getLocationFromName("Town");
            string candidateBoxPath = modHelper.Content.GetActualAssetKey("assets/Candidate_Box.png",
                    ContentSource.ModFolder);

            candidateBoxTile = new TileSheet(
                    id: "z-candidate-box", // a unique ID for the tilesheet
                    map: town.map,
                    imageSource: candidateBoxPath,
                    sheetSize: new Size(16, 16), // the tile size of your tilesheet image.
                    tileSize: new Size(16, 16)
            );
        }

        public void PlaceAssets()
        {
            GameLocation town = Game1.getLocationFromName("Town");

            if (town.map.GetTileSheet("z-candidate-box") == null &&
                SDate.Now() >= initialEntryDate && SDate.Now() <= finalEntryDate)
            {
                Location candidateBoxPosition = new Location((int)boxPosition.X, (int)boxPosition.Y);
                Layer buildingLayer = town.map.GetLayer("Buildings");

                town.map.AddTileSheet(candidateBoxTile);
                town.map.LoadTileSheets(Game1.mapDisplayDevice);

                buildingLayer.Tiles[candidateBoxPosition] = new
                    StaticTile(buildingLayer, candidateBoxTile, BlendMode.Alpha, 0);
            }

            if (SDate.Now() == finalEntryDate.AddDays(1) &&
                town.map.GetTileSheet("z-candidate-box") != null)
            {
                town.map.RemoveTileSheetDependencies(candidateBoxTile);
                town.map.RemoveTileSheet(candidateBoxTile);
            }

        }

        public void CandidateBoxReaction(ButtonReleasedEventArgs e, ModData saveData)
        {
            if (SDate.Now() >= initialEntryDate && SDate.Now() <= finalEntryDate)
            {
                if (saveData.enteredElection && e.Button.IsActionButton()
                && e.Cursor.GrabTile.Equals(boxPosition)
                && Game1.player.currentLocation == Game1.getLocationFromName("Town"))
                {
                    Game1.drawObjectDialogue("You have already entered the election!");
                }

                else if (!saveData.enteredElection && e.Button.IsActionButton()
                    && e.Cursor.GrabTile.Equals(boxPosition)
                    && Game1.player.currentLocation == Game1.getLocationFromName("Town"))
                {
                    Response[] responses =
                    {
                      new Response("100", "Yes"),
                      new Response("200", "No")
                    };

                    Game1.getLocationFromName("Town").createQuestionDialogue("Would you " +
                        "like to enter the election?", responses, delegate (Farmer _, string answer)
                        {
                            switch (answer)
                            {
                                case "100":
                                    saveData.enteredElection = true;
                                    break;
                                case "200":
                                //do nothing;
                                break;
                            }
                        });
                }
            }
            
        }
        
    }
}
