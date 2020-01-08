using Microsoft.Xna.Framework;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;

namespace StardewElections
{
    public class ModEntry : Mod
    {
        ModData data;
        ElectionAssetInitialization assetInitialization;

        ClickableComponent electionTab;

        int initialTab = -1;

        public override void Entry(IModHelper helper)
        {
            assetInitialization = new ElectionAssetInitialization();

            helper.Content.AssetEditors.Add(new ElectionMail(helper));
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonReleased += OnButtonReleased;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.Display.RenderedActiveMenu += OnRenderedActiveMenu;
            helper.Events.GameLoop.Saving += OnSaving;
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            data = Helper.Data.ReadSaveData<ModData>("stardewElections") ?? new ModData();
            assetInitialization.LoadAllAssets(Helper);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            assetInitialization.PlaceAssets();
        }

        private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            if (Game1.activeClickableMenu != null && e.Button.Equals(SButton.MouseLeft)
                && electionTab.containsPoint((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y))
            {
                Game1.activeClickableMenu = new ElectionMenu();
            }

            assetInitialization.AssetReactions(e, data);
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is GameMenu menu && data.enteredElection)
            {
                var pages = Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue();
                var tabs = Helper.Reflection.GetField<List<ClickableComponent>>(menu, "tabs").GetValue();

                initialTab = tabs.Count;

                electionTab = new ClickableComponent(new Rectangle
                    (menu.xPositionOnScreen + (tabs[1].bounds.Width * 9),
                    menu.yPositionOnScreen + 16, 64, 64),
                    "electionMenu",
                    "Elections"
                    );

                electionTab.myID = 12348;
                electionTab.leftNeighborID = 12347;
                electionTab.fullyImmutable = true;

                tabs.Add(electionTab);
                tabs[7].rightNeighborID = 12348;

                pages.Add((IClickableMenu)new ElectionMenu());
            }
        }

        private void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            //Todo: see if there are any methods that won't cover the cursor

            if (Game1.activeClickableMenu is GameMenu menu && menu.currentTab != GameMenu.mapTab && data.enteredElection)
            {
                var tabs = Helper.Reflection.GetField<List<ClickableComponent>>(menu, "tabs").GetValue();

                if (menu.invisible || initialTab == -1) return;
                if (tabs.Count <= initialTab) return;

                var tab = tabs[initialTab];

                Texture2D icon = assetInitialization.electionIcon;

                e.SpriteBatch.Draw(icon, new Vector2((float)tab.bounds.X, (float)(tab.bounds.Y
                    + (menu.currentTab == menu.getTabNumberFromName(tab.name) ? 8 : 0))),
                    new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White, 0.0f,
                    Vector2.Zero, 4f, SpriteEffects.None, 0.0001f);
            }
        }
        
        private void OnSaving(object sender, SavingEventArgs e)
        {
            Helper.Data.WriteSaveData("stardewElections", data);
        }
    }
}