using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RemoteAppControl : MonoBehaviour
{
    public string statusURL = "https://raw.githubusercontent.com/Bonny8055/My-Web/main/pages/status.txt";

    public GameObject deactivatePanel;

    void Start()
    {
        deactivatePanel.SetActive(false);

        StartCoroutine(StartCheck());
    }

    IEnumerator StartCheck()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(CheckStatus());
    }

    IEnumerator CheckStatus()
    {
        Debug.Log("Checking server...");

        UnityWebRequest www = UnityWebRequest.Get(statusURL);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Network Error: " + www.error);
            yield break;
        }

        string status = www.downloadHandler.text.Trim();

        Debug.Log("Server returned: " + status);

        if (status == "inactive")
        {
            DisableApplication();
        }
    }

    void DisableApplication()
    {
        Debug.Log("Application disabled");

        if(deactivatePanel != null)
            deactivatePanel.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;

        StartCoroutine(CloseApp());
    }

    IEnumerator CloseApp()
    {
        yield return new WaitForSecondsRealtime(5f);
        Application.Quit();
    }
}