using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 一键在当前场景创建/修复计分 UI（Canvas + 左上角 TMP 文本 + ui_script）。
/// 菜单：Tools → 计分UI → 一键创建或修复计分UI
/// </summary>
public static class ScoreUISetup
{
    [MenuItem("Tools/计分UI/一键创建或修复计分UI")]
    public static void Setup()
    {
        // 1. 画布
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Undo.RegisterCreatedObjectUndo(canvasGo, "Create Canvas");
        }

        // 2. EventSystem（UI 点击需要）
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
        }

        // 3. 复用已有计分文本，找不到就新建
        var ui = Object.FindObjectOfType<ui_script>();
        TextMeshProUGUI text = ui != null ? ui.scoreText : null;
        if (text == null)
            text = FindScoreText(canvas.transform);
        if (text == null)
            text = CreateScoreText(canvas.transform);

        // 4. 固定到屏幕左上角，保证 Game 视图可见
        var rt = text.rectTransform;
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(40f, -40f);
        rt.sizeDelta = new Vector2(800f, 300f);

        text.fontSize = 40;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.TopLeft;
        text.enableWordWrapping = true;
        text.raycastTarget = false;
        if (text.font == null)
        {
            // 直接访问 TMP_Settings.defaultFontAsset 在 TMP 资源缺失时会抛 NullReferenceException
            text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            if (text.font == null && TMP_Settings.instance != null)
                text.font = TMP_Settings.defaultFontAsset;
        }
        text.text = "Score: 0\nHistory: None\nBest: 0";

        // 5. 挂载并绑定计分脚本
        if (ui == null)
            ui = text.GetComponent<ui_script>();
        if (ui == null)
            ui = Undo.AddComponent<ui_script>(text.gameObject);
        ui.scoreText = text;

        EditorUtility.SetDirty(text);
        EditorUtility.SetDirty(ui);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(text.gameObject.scene);

        Selection.activeGameObject = text.gameObject;
        Debug.Log("计分 UI 已创建/修复：Canvas + 左上角 TMP 文本 + ui_script。");
    }

    static TextMeshProUGUI FindScoreText(Transform parent)
    {
        var all = parent.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var t in all)
        {
            if (t.name == "ScoreText" || t.name == "Text (TMP)")
                return t;
        }
        return null;
    }

    static TextMeshProUGUI CreateScoreText(Transform parent)
    {
        var go = new GameObject("ScoreText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        Undo.RegisterCreatedObjectUndo(go, "Create ScoreText");
        return go.GetComponent<TextMeshProUGUI>();
    }
}
