using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject scenarioPanel;
    public GameObject ideaPanel;
    public GameObject evaluationPanel;
    public GameObject leaderboardPanel;
    public GameObject loadingPanel;
    public GameObject errorPanel;

    [Header("Scenario Panel")]
    public TMP_Text textScenario;

    [Header("Evaluation Panel")]
    public TMP_Text textInvestmentDecision;
    public TMP_Text textAmount;
    public TMP_Text textComment;

    [Header("Loading Panel")]
    public TMP_Text textLoading;

    [Header("Error Panel")]
    public TMP_Text textError;

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
        ShowMainMenu();
    }

    private void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        scenarioPanel.SetActive(false);
        ideaPanel.SetActive(false);
        evaluationPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        loadingPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }

    public void ShowScenario(string scenarioText)
    {
        HideAllPanels();
        if (textScenario != null) textScenario.text = scenarioText;
        scenarioPanel.SetActive(true);
    }

    public void ShowIdeaInput()
    {
        HideAllPanels();
        ideaPanel.SetActive(true);
    }

    public void ShowEvaluation(string decision, int amount, string comment)
    {
        HideAllPanels();
        if (textInvestmentDecision != null) textInvestmentDecision.text = decision;
        if (textAmount != null) textAmount.text = $"Investeret: {amount:N0} DKK";
        if (textComment != null) textComment.text = comment;
        evaluationPanel.SetActive(true);
    }

    public void ShowLeaderboard()
    {
        HideAllPanels();
        leaderboardPanel.SetActive(true);
    }

    public void ShowLoading(string message = "Henter data...")
    {
        HideAllPanels();
        if (textLoading != null) textLoading.text = message;
        loadingPanel.SetActive(true);
    }

    public void ShowError(string message = "Noget gik galt.")
    {
        HideAllPanels();
        if (textError != null) textError.text = message;
        errorPanel.SetActive(true);
    }
    
    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
