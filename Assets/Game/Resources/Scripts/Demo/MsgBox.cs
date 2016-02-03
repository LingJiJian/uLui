using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MsgBox : LWindowBase {

    public Button btn_close;
    private LWindowManager wm;

	void Awake () {

        wm = LSingleton.getInstance("LWindowManager") as LWindowManager;

        btn_close.onClick.AddListener(() =>
        {
            wm.popWindow(this);
        });

	}
	
}
