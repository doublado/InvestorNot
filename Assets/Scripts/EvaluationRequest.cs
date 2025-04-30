using System;

[Serializable]
public class EvaluationRequest
{
    public string scenario;
    public string idea;

    public EvaluationRequest(string scenario, string idea)
    {
        this.scenario = scenario;
        this.idea = idea;
    }
}
