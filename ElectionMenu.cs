using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;

namespace StardewElections
{
    public class ElectionMenu : IClickableMenu
    {

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
        }

        public override void draw(SpriteBatch b)
        {
            //Basic Menu background
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen,
                width, height, false, true);

        }

    }
}
