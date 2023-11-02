using mouahrarasModuleCollection.FarmView.Zoom.Hooks;

namespace mouahrarasModuleCollection.FarmView.Zoom.Utilities
{
	internal class MenusPatchUtility
	{
		internal static void EnterFarmViewPostfix()
		{
			if (!ModEntry.Config.FarmViewZoom)
				return;
			ModEntry.Helper.Events.GameLoop.UpdateTicking += UpdateTickingHook.Apply;
		}

		internal static void LeaveFarmViewPostfix()
		{
			if (!ModEntry.Config.FarmViewZoom)
				return;
			ModEntry.Helper.Events.GameLoop.UpdateTicking -= UpdateTickingHook.Apply;
			ZoomUtility.Reset();
		}
	}
}
