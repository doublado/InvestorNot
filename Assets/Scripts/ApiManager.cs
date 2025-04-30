using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiManager
{
    private static string BaseUrl => ApiSettings.Config.baseUrl;
    private static string ApiSecret => ApiSettings.Config.apiSecret;

    public static async Task<string> GetScenarioAsync()
    {
        using var req = UnityWebRequest.Get($"{BaseUrl}/scenario");
        AddAuth(req);

        await req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            throw new Exception(req.error ?? "Scenario request failed.");

        var response = JsonUtility.FromJson<ScenarioResponse>(req.downloadHandler.text);
        return response.scenario;
    }

    public static async Task<EvaluationResponse> EvaluateIdeaAsync(string scenario, string idea)
    {
        var payload = new EvaluationRequest(scenario, idea);
        var json = JsonUtility.ToJson(payload);

        using var req = new UnityWebRequest($"{BaseUrl}/evaluate", "POST");
        var body = Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        AddAuth(req);

        await req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            throw new Exception(req.error ?? "Evaluation failed.");

        return JsonUtility.FromJson<EvaluationResponse>(req.downloadHandler.text);
    }

    public static async Task<List<LeaderboardEntry>> GetLeaderboardAsync()
    {
        using var req = UnityWebRequest.Get($"{BaseUrl}/leaderboard");
        AddAuth(req);

        await req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            throw new Exception(req.error ?? "Leaderboard request failed.");

        var jsonArray = req.downloadHandler.text;
        var wrapper = JsonUtility.FromJson<LeaderboardWrapper>("{\"entries\":" + jsonArray + "}");
        return wrapper.entries;
    }

    public static async Task SubmitScoreAsync(string name, int score)
    {
        var payload = new LeaderboardEntry { name = name, score = score };
        var json = JsonUtility.ToJson(payload);

        using var req = new UnityWebRequest($"{BaseUrl}/leaderboard/submit", "POST");
        var body = Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        AddAuth(req);

        await req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            throw new Exception(req.error ?? "Submit failed.");
    }

    private static void AddAuth(UnityWebRequest req)
    {
        req.SetRequestHeader("X-API-SECRET", ApiSecret);
    }

    [Serializable] private class ScenarioResponse { public string scenario; }
    [Serializable] public class EvaluationResponse
    {
        public string investmentDecision;
        public int amount;
        public string comment;
    }

    [Serializable] private class LeaderboardWrapper { public List<LeaderboardEntry> entries; }
}
