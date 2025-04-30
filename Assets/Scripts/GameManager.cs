using UnityEngine;
using System;
using System.Threading.Tasks;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance { get; set; }

    [Header("Managers")]
    public LeaderboardManager leaderboardManager;

    [Header("UI References")]
    public TMP_InputField ideaInputField;
    public TMP_InputField nameInputField;
    
    private string _currentScenario;
    private string _currentInvestmentDecision;
    private int _currentScore;
    private string _currentComment;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        GoToMainMenu();
    }

    private static void GoToMainMenu()
    {
        UIManager.Instance.ShowMainMenu();
    }

    public async void OnPlayPressed()
    {
        try
        {
            UIManager.Instance.ShowLoading("Genererer scenarie...");
            _currentScenario = await ApiManager.GetScenarioAsync();
            UIManager.Instance.ShowScenario(_currentScenario);
        }
        catch (Exception e)
        {
            Debug.LogError($"Scenario API failed: {e.Message}");
            UIManager.Instance.ShowError("Kunne ikke hente scenariet.");
        }
    }

    public void OnScenarioContinuePressed()
    {
        UIManager.Instance.ShowIdeaInput();
    }

    public async void OnIdeaSubmitted()
    {
        var idea = ideaInputField != null ? ideaInputField.text.Trim() : "";

        if (string.IsNullOrWhiteSpace(idea))
        {
            UIManager.Instance.ShowError("Du skal skrive en idé først.");

            await Task.Delay(2500); // wait 2.5 seconds
            UIManager.Instance.ShowIdeaInput(); // return to idea panel
            return;
        }
        
        OnIdeaSubmitted(idea);
        ideaInputField.text = "";
    }

    private async void OnIdeaSubmitted(string idea)
    {
        try
        {
            UIManager.Instance.ShowLoading("Vurderer idé...");
            var result = await ApiManager.EvaluateIdeaAsync(_currentScenario, idea);
            _currentInvestmentDecision = result.investmentDecision;
            _currentScore = result.amount;
            _currentComment = result.comment;
            
            UIManager.Instance.ShowEvaluation(
                result.investmentDecision,
                result.amount,
                result.comment
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Evaluation API failed: {e.Message}");
            UIManager.Instance.ShowError("Vurdering fejlede.");
        }
    }

    public async void OnEvaluationContinuePressed()
    {
        try
        {
            var playerName = nameInputField != null ? nameInputField.text.Trim() : "";

            if (string.IsNullOrWhiteSpace(playerName))
            {
                UIManager.Instance.ShowError("Du skal indtaste dit navn.");
                await Task.Delay(2500);
                UIManager.Instance.ShowEvaluation(
                    decision: _currentInvestmentDecision, 
                    amount: _currentScore,
                    comment: _currentComment
                );
                return;
            }
            
            UIManager.Instance.ShowLoading("Sender resultat til leaderboard...");
            await ApiManager.SubmitScoreAsync(playerName, _currentScore);
            await ShowLeaderboardAsync();
            nameInputField.text = "";
        }
        catch (Exception e)
        {
            Debug.LogError($"Submit score failed: {e.Message}");
            UIManager.Instance.ShowError("Kunne ikke indsende score.");
        }
    }

    public async void OnMainMenuLeaderboardPressed()
    {
        try
        {
            UIManager.Instance.ShowLoading("Henter leaderboard...");
            await ShowLeaderboardAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"Leaderboard fetch failed: {e.Message}");
            UIManager.Instance.ShowError("Kunne ikke hente leaderboard.");
        }
    }

    private async Task ShowLeaderboardAsync()
    {
        var entries = await ApiManager.GetLeaderboardAsync();
        leaderboardManager.Populate(entries);
        UIManager.Instance.ShowLeaderboard();
    }

    public void OnLeaderboardBackPressed()
    {
        GoToMainMenu();
    }
}
