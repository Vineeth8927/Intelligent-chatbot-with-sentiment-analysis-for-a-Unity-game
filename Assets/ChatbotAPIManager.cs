using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatbotAPIManager : MonoBehaviour
{
    private string apiURL = "http://colab.research.google.com/drive/1GpW2S4yhJuaWQxAujr48PbeL5rJX60Qb#scrollTo=f_mmat6u2mVN:5000/get_response";

    public IEnumerator GetChatbotResponse(string userInput, System.Action<string> callback)
    {
        // Prepare the JSON data
        string jsonData = "{\"user_input\":\"" + userInput + "\"}";

        // Create a new UnityWebRequest for POST
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response
            string responseText = request.downloadHandler.text;
            // Extract the response string from JSON
            string response = JsonUtility.FromJson<ChatbotResponse>(responseText).response;
            callback(response);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    [System.Serializable]
    private class ChatbotResponse
    {
        public string response;
    }
}
