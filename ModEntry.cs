using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewElections
{
    public class ModEntry : Mod
    {
        ModData data;
        ElectionAssetPlacement placement;

        public override void Entry(IModHelper helper)
        {
            placement = new ElectionAssetPlacement();

            helper.Content.AssetEditors.Add(new ElectionMail(helper));
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonReleased += OnButtonReleased;
            helper.Events.GameLoop.Saving += OnSaving;
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            data = Helper.Data.ReadSaveData<ModData>("stardewElections") ?? new ModData();
            placement.LoadAllAssets(Helper);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            placement.PlaceAssets();

            if (!data.menuNotif && data.enteredElection)
            {
                HUDMessage menuMessage = new HUDMessage("The election menu is now available" +
                    "for use!", 2);
                Game1.addHUDMessage(menuMessage);
                data.menuNotif = true;
            }
        }

        private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            placement.CandidateBoxReaction(e, data);
        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            Helper.Data.WriteSaveData("stardewElections", data);
        }
    }
}