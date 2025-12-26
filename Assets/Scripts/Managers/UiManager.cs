using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject uiPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TurnOffPanel()
    {
        uiPanel.SetActive(false);
    }
}
