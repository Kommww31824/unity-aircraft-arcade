using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 一键将 script2 计时器挂到场景，并创建 Timer Text / 启停按钮绑定。
/// 菜单：Tools → 计时器 → 一键配置场景
/// </summary>
public static class TimerSceneSetup
{
    [MenuItem("Tools/计时器/一键配置场景")]
    public static void Setup()
    {
        // 1. 挂载计时脚本到场景中的 GameObject
        var timer = Object.FindObjectOfType<script2>();
        if (timer == null)
        {
            var go = new GameObject("TimerController");
            timer = go.AddComponent<script2>();
            Undo.RegisterCreatedObjectUndo(go, "Create TimerController");
        }

        // 2. 确保有 Canvas 与 EventSystem（UI 点击需要）
        var canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            Undo.RegisterCreatedObjectUndo(canvasGo, "Create Canvas");
        }

        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
        }

        // 3. 创建 UI Text 并赋给 Timer Text 字段
        Text timerText = timer.timerText;
        if (timerText == null)
        {
            timerText = CreateUiText(canvas.transform, "TimerText", "0.00s",
                new Vector2(0f, 200f), new Vector2(400f, 80f), 48);
            Undo.RecordObject(timer, "Assign Timer Text");
            timer.timerText = timerText;
            EditorUtility.SetDirty(timer);
        }

        // 4. 创建启停按钮并绑定 StartTimer / StopTimer
        EnsureButton(canvas.transform, "StartButton", "开始", new Vector2(-120f, 80f),
            timer, nameof(script2.StartTimer));
        EnsureButton(canvas.transform, "StopButton", "停止", new Vector2(120f, 80f),
            timer, nameof(script2.StopTimer));

        Selection.activeGameObject = timer.gameObject;
        EditorGUIUtility.PingObject(timer.gameObject);
        Debug.Log("计时器场景配置完成：已挂载 script2，绑定 Timer Text，并创建 Start/Stop 按钮。");
    }

    static Text CreateUiText(Transform parent, string name, string content,
        Vector2 anchoredPos, Vector2 size, int fontSize)
    {
        var existing = parent.Find(name);
        if (existing != null)
        {
            var t = existing.GetComponent<Text>();
            if (t != null) return t;
        }

        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        go.transform.SetParent(parent, false);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);

        var rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;

        var text = go.GetComponent<Text>();
        text.text = content;
        text.font = GetDefaultFont();
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        return text;
    }

    static Font GetDefaultFont()
    {
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        return font;
    }

    static void EnsureButton(Transform parent, string name, string label, Vector2 anchoredPos,
        script2 target, string methodName)
    {
        Button button;
        var existing = parent.Find(name);
        if (existing != null)
        {
            button = existing.GetComponent<Button>();
            if (button == null)
                button = existing.gameObject.AddComponent<Button>();
        }
        else
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            Undo.RegisterCreatedObjectUndo(go, "Create " + name);

            var rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = new Vector2(160f, 50f);

            var image = go.GetComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

            button = go.GetComponent<Button>();

            var labelGo = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            labelGo.transform.SetParent(go.transform, false);
            var labelRt = labelGo.GetComponent<RectTransform>();
            labelRt.anchorMin = Vector2.zero;
            labelRt.anchorMax = Vector2.one;
            labelRt.offsetMin = Vector2.zero;
            labelRt.offsetMax = Vector2.zero;
            var labelText = labelGo.GetComponent<Text>();
            labelText.text = label;
            labelText.font = GetDefaultFont();
            labelText.fontSize = 28;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.color = Color.white;
        }

        Undo.RecordObject(button, "Bind " + methodName);
        button.onClick.RemoveAllListeners();
        if (methodName == nameof(script2.StartTimer))
            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, target.StartTimer);
        else
            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, target.StopTimer);

        EditorUtility.SetDirty(button);
    }
}
