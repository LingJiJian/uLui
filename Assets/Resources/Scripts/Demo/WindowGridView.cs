using UnityEngine;
using UnityEngine.UI;

public class WindowGridView : LWindowBase
{
    public Button btn_close;
    private LWindowManager wm;

    public WindowGridView()
    {
        this.disposeType = WindowDispose.Normal;
    }

    void Awake()
    {
        btn_close.onClick.AddListener(() =>
        {
            LWindowManager wm = LSingleton.getInstance("LWindowManager") as LWindowManager;
            wm.popWindow(this);
        });
    }
}