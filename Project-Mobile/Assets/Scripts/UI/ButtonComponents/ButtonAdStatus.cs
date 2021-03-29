using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAdStatus : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        StartCoroutine(SetButtonStatus());
    }

    private IEnumerator SetButtonStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(.3f);
            button.interactable = Connection.IsDeviceConnected();
            yield return null;
        }
    }
}
