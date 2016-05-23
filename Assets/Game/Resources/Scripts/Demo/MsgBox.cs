using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MsgBox : LWindowBase {

    public Button btn_close;
    private LWindowManager wm;

	void Awake () {

        wm = LWindowManager.GetInstance() ;

        btn_close.onClick.AddListener(() =>
        {
            wm.popWindow(this);
        });

	}
	
}
