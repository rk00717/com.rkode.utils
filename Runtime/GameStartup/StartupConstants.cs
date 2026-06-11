using RKode.Utils;

namespace RKode.Startup {
public static class StartupConstants {
    public const string CONFIG_PATH = "RKode/ProjectConfig";

    public static readonly string CAN_LOAD_KEY = ProjectScopedKey.Get($"RKode.Startup.CanLoadMain");
    public static readonly string PREVIOUS_SCENE_KEY = ProjectScopedKey.Get($"RKode.Startup.PreviousScene");
    public static readonly string REDIRECT_COMPLETED_KEY = ProjectScopedKey.Get($"RKode.Startup.RedirectCompleted");

	static StartupConstants() {
		
	}
}
}