using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text leaderboardText;

    public void Populate(List<LeaderboardEntry> entries)
    {
        if (leaderboardText == null)
        {
            Debug.LogWarning("Leaderboard text not assigned.");
            return;
        }
        
        leaderboardText.text = "";

        for (var i = 0; i < entries.Count; i++)
        {
            var rank = $"{i + 1}.";
            var line = $"{rank} {entries[i].name}\t{entries[i].score:N0} DKK";
            leaderboardText.text += line + "\n";
        }
    }
}
