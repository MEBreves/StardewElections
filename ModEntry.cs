using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using xTile.Layers;
using xTile.Tiles;

namespace StardewElections
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Content.AssetEditors.Add(new ElectionMail(helper));
            ElectionAssetPlacement placement = new ElectionAssetPlacement(helper);
        }
    }
}