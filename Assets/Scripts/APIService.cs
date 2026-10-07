using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIService : MonoBehaviour
{
    public static APIService Instance { get; private set; }

    [Header("Server Instellingen")]
    public string apiBaseUrl = "https://jouw-webserver.nl/api/v1";
    public string authToken = "BEARER_TOKEN_HERE";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SendSessionData(TriageSessionData data)
    {
        StartCoroutine(PostSessionCoroutine(data));
    }

    private IEnumerator PostSessionCoroutine(TriageSessionData data)
    {
        string json = JsonUtility.ToJson(data);
        string endpoint = $"{apiBaseUrl}/sessions";

        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {authToken}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Sessie succesvol opgeslagen in database! Server response: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Fout bij versturen van sessiedata naar API: {request.error}");
            }
        }
    }
}