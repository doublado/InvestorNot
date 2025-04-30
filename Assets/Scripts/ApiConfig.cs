using UnityEngine;

[CreateAssetMenu(fileName = "ApiConfig", menuName = "Config/API")]
public class ApiConfig : ScriptableObject
{
    public string baseUrl = "http://localhost:8080";
    public string apiSecret = "your-api-secret";
}
