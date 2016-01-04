using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class Firstui : MonoBehaviour
{
    public Button btn_grid;
    public Button btn_trans;
    private LWindowManager wm;

    void Awake()
    {
        wm = LSingleton.getInstance("LWindowManager") as LWindowManager;

        btn_grid.onClick.AddListener(() =>
        {
            wm.runWindow("WindowGridView", typeof(WindowGridView), WindowHierarchy.Normal);
        });

        btn_trans.onClick.AddListener(() =>
        {
            Application.LoadLevel("second");
        });
    }
}
