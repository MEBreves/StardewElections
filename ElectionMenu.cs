using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley;

namespace StardewElections
{
    public class ElectionMenu : IClickableMenu
    {
        IModHelper modHelper;

        public ElectionMenu(IModHelper helper) 
        {
            modHelper = helper;

            helper.Events.Display.MenuChanged += OnMenuChanged;
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is GameMenu)
            {
                xPositionOnScreen = Game1.activeClickableMenu.xPositionOnScreen;
                yPositionOnScreen = Game1.activeClickableMenu.yPositionOnScreen + 10;
                height = Game1.activeClickableMenu.height;
            }
        }

    }
}
