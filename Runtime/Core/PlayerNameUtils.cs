using UnityEngine;

namespace RKode.Utils {
public static class PlayerNameUtils {
    public const string PLAYER_NAME_KEY = "PlayerName";
    private static readonly string[] namePrefixes = new string[] {
        "Alpha", "Echo", "Shadow", "Nova", "Rogue", "Blitz",
        "Falcon", "Specter", "Crimson", "Vortex", "Lunar",
        "Comet", "Phantom", "Orbit", "Titan", "Player", 
    };

    public static string TryGetPlayerName() {
        return PlayerPrefs.GetString(PLAYER_NAME_KEY, string.Empty);
    }

    public static string TryGetExistingName() {
        var savedName = TryGetPlayerName();

        if (string.IsNullOrEmpty(savedName)) {
            string prefix = namePrefixes[Random.Range(0, namePrefixes.Length)];
            int number = Random.Range(1000, 9999);

            savedName = $"{prefix}_{number}";
            PlayerPrefs.SetString(PLAYER_NAME_KEY, savedName);
        }

        return savedName;
    }
}
}