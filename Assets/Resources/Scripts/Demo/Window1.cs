using UnityEngine;
using UnityEngine.UI;

public class Window1 : LWindowBase
{
    public Button btn_close;

    public Window1()
    {
        
    }

    void Awake()
    {
        Debug.Log("Awake Window1 !");

        btn_close.onClick.AddListener(() =>
        {
            LWindowManager wm = LSingleton.getInstance("LWindowManager") as LWindowManager;
            wm.popWindow("Window1");
        });
    }
}