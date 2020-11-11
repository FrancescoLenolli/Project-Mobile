using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float msec = 0.0f;
    private float fps = 0.0f;
    private GUIContent content = new GUIContent();
    private GUIContent content2 = new GUIContent();

    public Texture2D texture2D;

    private void Start()
    {
        content.text = GameManager.Instance.canSaveData ? "Save Data Enabled" : "Save Data Disabled";
        content2.text = "Reset Data";
    }
    private void Update()
    {
        if (GameManager.Instance.canDebug)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            msec = deltaTime * 1000.0f;
            fps = 1.0f / deltaTime;
        }
    }

    private void OnGUI()
    {
        if (GameManager.Instance.canDebug)
        {
            int w = 150;
            int h = 75;
            GUIStyle style = new GUIStyle();
            Rect rect = new Rect(0, 0, w, h);
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = h / 3;
            style.normal.textColor = Color.black;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);

            GUIStyle buttonStyle = new GUIStyle();
            buttonStyle.normal.background = MakeTex(w, h, Color.white);
            buttonStyle.normal.textColor = Color.black;
            buttonStyle.fontSize = h / 4;
            buttonStyle.alignment = TextAnchor.MiddleLeft;
            if (GUI.Button(new Rect(0, h, w, h), content, buttonStyle))
            {
                GameManager.Instance.canSaveData = !GameManager.Instance.canSaveData;
                content.text = GameManager.Instance.canSaveData ? "Save Data Enabled" : "Save Data Disabled";
            }

            buttonStyle.fontSize = h / 3;
            if (GUI.Button(new Rect(0, h * 2, w, h), content2, buttonStyle))
            {
                GameManager.Instance.ResetData();
            }
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
