using UnityEngine;
using UnityEngine.UI;

public class PanelOpener : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Button;
    
    public void OpenPanel()
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
            Button.GetComponentInChildren<Text>().text = "View Hint";
        }
        else
        {
            Panel.SetActive(true);
            Button.GetComponentInChildren<Text>().text = "Close Hint";
        }
    }
}
