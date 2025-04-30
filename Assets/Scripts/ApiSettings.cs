using UnityEngine;

public static class ApiSettings
{
    private static ApiConfig _config;

    public static ApiConfig Config
    {
        get
        {
            if (_config != null) return _config;
            _config = Resources.Load<ApiConfig>("ApiConfig");
            if (_config == null)
            {
                Debug.LogError("ApiConfig asset not found in Resources folder.");
            }
            return _config;
        }
    }
}
