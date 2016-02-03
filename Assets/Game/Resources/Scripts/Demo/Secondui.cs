using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class Secondui : MonoBehaviour {

    public Button btn_msg;
    public Button btn_trans;
    public Button btn_anim;
    public Button btn_clean;
    private LWindowManager _wm;
    protected Animator _teddyAnim;

    void Start() {

        _wm = LSingleton.getInstance("LWindowManager") as LWindowManager;
        _teddyAnim = GameObject.Find("Teddy").GetComponent<Animator>();

        btn_msg.onClick.AddListener(() =>
        {
            _wm.runWindow("MsgBox", WindowHierarchy.Normal);
        });

        btn_trans.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("first");
        });

        btn_anim.onClick.AddListener(() =>
        {
            _teddyAnim.SetBool("run_idle", false);
            _teddyAnim.SetBool("idle_run", true);
        });

        btn_clean.onClick.AddListener(() =>
        {
            _teddyAnim.SetBool("idle_run", false);
            _teddyAnim.SetBool("run_idle", true);
        });
	}
}
