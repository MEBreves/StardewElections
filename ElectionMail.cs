using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace StardewElections
{
    public class ElectionMail : IAssetEditor
    {
        IModHelper modhelper;

        public ElectionMail(IModHelper helper)
        {
            modhelper = helper;
            helper.Events.GameLoop.DayStarted += this.SendMailOnDay;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data\\mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            var data = asset.AsDictionary<string, string>().Data;

            data["Election Notice"] = "Good morning dear!^ It's election season again, and " +
                "I've volunteered to do the moderating this time. As always, Lewis " +
                "still wants to be mayor, but that doesn't mean that the elections " +
                "are set. If you'd like to run against Lewis, put an entry card in the " +
                "box in front of Pierre's. I will be collecting the entries on Spring 4. " +
                "Good luck! ^ ^ Evelyn";
        }

        private void SendMailOnDay(object sender, DayStartedEventArgs e)
        {
            var initialEntryDate = new SDate(1, "spring");
            
            if (SDate.Now() == initialEntryDate)
            {
                Game1.player.mailbox.Add("Election Notice");
                Game1.player.mailReceived.Add("Election Notice");
            }
        }
    }
}
