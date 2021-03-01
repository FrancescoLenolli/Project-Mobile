using UnityEngine;
using UnityEngine.UI;

public class ButtonAdStatus : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        button.interactable = Connection.IsDeviceConnected();
    }
}
