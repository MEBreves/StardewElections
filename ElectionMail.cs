using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace StardewElections
{
    public class ElectionMail : IAssetEditor
    {
        public ElectionMail(IModHelper helper) : base()
        {
            helper.Events.GameLoop.DayStarted += this.SendMailOnDay;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data\\mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            var data = asset.AsDictionary<string, string>().Data;

            data["ElectionNotice"] = "Good morning dear!^ It's election season again, and " +
                "I've volunteered to do the moderating this time. As always, Lewis " +
                "still wants to be mayor, but that doesn't mean that the elections " +
                "are set. If you'd like to run against Lewis, put an entry card in the " +
                "box in front of Pierre's. I will be collecting the entries on Spring 4. " +
                "Good luck! ^ ^ -Evelyn" +
                "[#]Election Notice";
        }

        private void SendMailOnDay(object sender, DayStartedEventArgs e)
        {
            SDate initialEntryDate = new SDate(28, "winter", 2);
            
            if (SDate.Now() == initialEntryDate)
            {
                Game1.player.mailForTomorrow.Add("ElectionNotice");
            }
        }
    }
}
