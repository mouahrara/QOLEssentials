using mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Handlers;

namespace mouahrarasModuleCollection.TweaksAndFeatures.UserInterface.Zoom.Utilities
{
	internal class MenusPatchUtility
	{
		internal static void EnterFarmViewPostfix()
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;
			ModEntry.Helper.Events.GameLoop.UpdateTicking += UpdateTickingHandler.Apply;
		}

		internal static void LeaveFarmViewPostfix()
		{
			if (!ModEntry.Config.UserInterfaceZoom)
				return;
			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHandler.Apply;
			ZoomUtility.Reset();
		}
	}
}
