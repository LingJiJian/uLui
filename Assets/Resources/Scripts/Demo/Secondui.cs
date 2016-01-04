using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class Secondui : MonoBehaviour {

    public Button btn_msg;
    public Button btn_trans;
    public Button btn_anim;
    public Button btn_clean;
    private LWindowManager wm;
    protected Animator teddyAnim;

    void Awake() {

        wm = LSingleton.getInstance("LWindowManager") as LWindowManager;
        teddyAnim = GameObject.Find("Teddy").GetComponent<Animator>();

        btn_msg.onClick.AddListener(() =>
        {
            wm.runWindow("MsgBox", typeof(MsgBox), WindowHierarchy.Normal);
        });

        btn_trans.onClick.AddListener(() =>
        {
            Application.LoadLevel("first");
        });

        btn_anim.onClick.AddListener(() =>
        {
            teddyAnim.SetBool("run_idle", false);
            teddyAnim.SetBool("idle_run", true);
        });

        btn_clean.onClick.AddListener(() =>
        {
            teddyAnim.SetBool("idle_run", false);
            teddyAnim.SetBool("run_idle", true);
        });
	}
}
