using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class APIService : MonoBehaviour
{
    public static APIService Instance { get; private set; }

    private string _apiBaseUrl;
    private string _authToken;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadEnvironmentVariables();
    }

    public void SendSessionData(TriageSessionData data)
    {
        StartCoroutine(PostSessionCoroutine(data));
    }

    private IEnumerator PostSessionCoroutine(TriageSessionData data)
    {
        string json = JsonUtility.ToJson(data);
        string endpoint = $"{_apiBaseUrl}/sessions";

        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_authToken}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(
                    $"Sessie succesvol opgeslagen in database! " +
                    $"Server response: {request.downloadHandler.text}"
                );
            }
            else
            {
                Debug.LogError(
                    $"Fout bij versturen van sessiedata naar API: {request.error}"
                );
            }
        }
    }

    private void LoadEnvironmentVariables()
    {
        string envPath = Path.Combine(Application.dataPath, "..", ".env");

        if (!File.Exists(envPath))
        {
            Debug.LogError($".env bestand niet gevonden: {envPath}");
            return;
        }

        foreach (string line in File.ReadAllLines(envPath))
        {
            string trimmedLine = line.Trim();

            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
            {
                continue;
            }

            string[] parts = trimmedLine.Split('=', 2);

            if (parts.Length != 2)
            {
                continue;
            }

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            switch (key)
            {
                case "API_BASE_URL":
                    _apiBaseUrl = value;
                    break;

                case "AUTH_TOKEN":
                    _authToken = value;
                    break;
            }
        }

        if (string.IsNullOrEmpty(_apiBaseUrl))
        {
            Debug.LogError("API_BASE_URL ontbreekt in .env");
        }

        if (string.IsNullOrEmpty(_authToken))
        {
            Debug.LogError("AUTH_TOKEN ontbreekt in .env");
        }
    }
}