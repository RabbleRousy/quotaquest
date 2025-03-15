using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderBoard : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject panel;
    public TMPro.TMP_InputField inputField;
    private string currentRanking;
    public void SubmitScore(int score)
    {
        string name = inputField.text;
        string url = string.Format("http://dreamlo.com/lb/Qg6zHLUBDUa4V9NCwxBjSwZa8SGsDj-0WWfjLr1pSomQ/add/{0}/{1}", name, score);
        StartCoroutine(GetRequest(url));
    }
    
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                Debug.Log($"Response: {webRequest.downloadHandler.text}");
            }
        }
    }
    
    IEnumerator PrintLeaderboard()
    {
        string uri = "http://dreamlo.com/lb/67d4b50a8f40bbc224907ea9/xml";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                ReadXml(webRequest.downloadHandler.text);
            }
        }
    }
    
    void ReadXml(string xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        XmlNodeList nodes = xmlDoc.SelectNodes("//entry");

        int rank = 0;
        currentRanking = "";
        foreach (XmlNode entryNode in nodes)
        {
            if (rank == 10) break;
            if (entryNode != null)
            {
                rank++;
                string name = entryNode["name"].InnerText;
                int score = int.Parse(entryNode["score"].InnerText);
                int seconds = int.Parse(entryNode["seconds"].InnerText);
                string text = entryNode["text"].InnerText;
                string date = entryNode["date"].InnerText;

                currentRanking += "" + rank + ". " + name + " failed at $" + score + " quota\n";
            }
        }
        scoreText.text = currentRanking;
        panel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            scoreText.text = "Honorable mention: Flo $9862687 surplus\n" + currentRanking;
        }
    }

    public void Print()
    {
        StartCoroutine(PrintLeaderboard());
    }
}
