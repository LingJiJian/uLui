using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Lui;

public class Firstui : MonoBehaviour
{
    public Button btn_grid;
    public Button btn_trans;
    public Button btn_guide;
    public LControlView ctrlView;
    public LTableView tblView;
    public LScrollView scrolView;
    public LRichText rtfView;
    public LPageView pageView;
    public LListView listView;
    public LGridView gridView;
	public LProgress progView;
    public LExpandListView expandView;
    public LTableView tbl_drag;
    public GameObject dragSelect;
    public GameObject panel_root;

    private LWindowManager _wm;

    void Start()
    {
        _wm = LWindowManager.GetInstance();

        btn_grid.onClick.AddListener(() =>
        {
            ArrayList list = new ArrayList();
            list.Add(123);
            list.Add("测试内容");
            _wm.runWindow("Prefabs/WindowGridView.prefab", WindowHierarchy.Normal, list);
        });

        btn_trans.onClick.AddListener(() =>
        {
            _wm.LoadSceneAsync("second",(float p)=>
            {
                Debug.Log("进度 " + p);
            });
        });

        btn_guide.onClick.AddListener(() =>
        {
            bool isBlock = panel_root.GetComponent<CanvasGroup>().blocksRaycasts;
            panel_root.GetComponent<CanvasGroup>().blocksRaycasts = !isBlock;
            Text textComp = btn_guide.transform.FindChild("Text").gameObject.GetComponent<Text>();
            textComp.text = isBlock ? "关闭遮罩" : "开启遮罩";
        });

        ctrlView.onControlHandler = (float ox, float oy,bool isFinish) =>
        {
            //Debug.Log(string.Format("offsetX={0} offsetY={1}", ox, oy));
        };

        scrolView.onMoveCompleteHandler = () =>
        {
            Debug.Log(" scrolView.onMoveCompleteHandler ");
        };
        
        tblView.cellsSize = new Vector2(150, 40);
        tblView.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 40 * 5);
        tblView.cellsCount = 100;
        tblView.SetCellHandle((int idx, GameObject obj) =>
        {
            obj.GetComponent<Text>().text = idx.ToString();
        });
        tblView.reloadData();

        //rtfView.insertElement("hello world!!", Color.blue, 25, true, false, Color.blue, "数据");
        //rtfView.insertElement("测试文本内容!!", Color.red, 15, false, true, Color.blue, "");
        //rtfView.insertElement("Atlas/face/01", 5f, "");
        //rtfView.insertElement("The article comes from the point of the examination", Color.green, 15, true, false, Color.blue, "");
        //		rtfView.insertElement("Atlas/face/0201.png", "");
        //rtfView.insertElement(1);
        //rtfView.insertElement("outline and newline", Color.yellow, 20, false, true, Color.blue, "");
        rtfView.parseRichDefaultString(
            "<lab txt=\"hello world!!\" color=#ffff00 data=数据 />"+
            "<lab txt=\"测试文本内容\" isUnderLine=true size=40/><anim path=Atlas/face/01 fps=5.0/>"+
            "<newline /><img path=Atlas/face/0201/>"+
            "<lab txt=\"The article comes from the point of the \" color=#ff0000 />"+
            "<lab txt=\"Examination\" color=#ff0000 isOutLine=true/>");
        rtfView.onClickHandler = (string data) =>
        {
            Debug.Log("data " + data);
        };
        //rtfView.reloadData();

        pageView.cellsSize = new Vector2(150, 100);
        pageView.cellsCount = 14;
        pageView.SetCellHandle((int idx,GameObject obj) =>
        {
            obj.transform.FindChild("Text").GetComponent<Text>().text = idx.ToString();
        });
        pageView.onPageChangedHandler = (int pageIdx) =>
        {
            Debug.Log("page " + pageIdx);
        };
        pageView.reloadData();

        listView.limitNum = 10; //not must to set limitNum
        for (int i = 0; i < 30; i++)
        {
            GameObject item = listView.dequeueItem();
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40 + Random.Range(0, 40));
            item.GetComponent<Text>().text = i.ToString();
            listView.insertNodeAtLast(item);
        }
        listView.reloadData();

        expandView.nodeNum = 20;
        expandView.nodeItemNum = 1;
        expandView.prepare();
        expandView.expand(0);
        expandView.expand(1);
        expandView.reloadData();

        gridView.cellsSize = new Vector2(100, 100);
        gridView.cols = 4;
        gridView.cellsCount = 100;
        gridView.SetCellHandle((int idx,GameObject obj) =>
        {
            obj.GetComponent<Text>().text = idx.ToString();
        });
        gridView.reloadData();

		progView.setValue (10);
		progView.startProgress (80, 1.0f);

        //drag1.onBeginDrag = (Vector2 position) =>
        //{
        //    dragSelect.SetActive(true);
        //    dragSelect.transform.Find("sprite").GetComponent<Image>().sprite = drag1.transform.Find("sprite").GetComponent<Image>().sprite;
        //    dragSelect.transform.position = position;
        //};
        //drag1.onDrag = (Vector2 position) =>
        //{
        //    dragSelect.transform.position = position;
        //};
        //drag1.onEndDrag = (Vector2 position) =>
        //{
        //    dragSelect.SetActive(false);
        //    if(Vector2.Distance(dragSelect.transform.position, drag2.transform.position) < 50)
        //    {
        //        Sprite oldSprite = drag1.transform.Find("sprite").GetComponent<Image>().sprite;
        //        drag1.transform.Find("sprite").GetComponent<Image>().sprite = drag2.transform.Find("sprite").GetComponent<Image>().sprite;
        //        drag2.transform.Find("sprite").GetComponent<Image>().sprite = oldSprite;
        //    }
        //};

        //drag2.onBeginDrag = (Vector2 position) =>
        //{
        //    dragSelect.SetActive(true);
        //    dragSelect.transform.Find("sprite").GetComponent<Image>().sprite = drag2.transform.Find("sprite").GetComponent<Image>().sprite;
        //    dragSelect.transform.position = position;
        //};
        //drag2.onDrag = (Vector2 position) =>
        //{
        //    dragSelect.transform.position = position;
        //};
        //drag2.onEndDrag = (Vector2 position) =>
        //{
        //    dragSelect.SetActive(false);
        //    if (Vector2.Distance(drag1.transform.position, dragSelect.transform.position) < 50)
        //    {
        //        Sprite oldSprite = drag2.transform.Find("sprite").GetComponent<Image>().sprite;
        //        drag2.transform.Find("sprite").GetComponent<Image>().sprite = drag1.transform.Find("sprite").GetComponent<Image>().sprite;
        //        drag1.transform.Find("sprite").GetComponent<Image>().sprite = oldSprite;
        //    }
        //};

        tbl_drag.SetCellHandle((int idx, GameObject obj) =>
        {
            obj.transform.Find("drag").GetComponent<LDragView>().onBeginDrag = (Vector2 position) =>
            {
                dragSelect.SetActive(true);
                dragSelect.transform.position = position;
                dragSelect.transform.Find("sprite").GetComponent<Image>().sprite = obj.transform.Find("sprite").GetComponent<Image>().sprite;
            };
            obj.transform.Find("drag").GetComponent<LDragView>().onDrag = (Vector2 position) =>
            {
                dragSelect.transform.position = position;
            };
            obj.transform.Find("drag").GetComponent<LDragView>().onEndDrag = (Vector2 position) =>
            {
                dragSelect.SetActive(false);
                GameObject dragTarget = null;
                for (int i=0;i<3;i++)
                {
                    GameObject drag = GameObject.Find("drag" + (i + 1));
                    if (Vector2.Distance(drag.transform.position, dragSelect.transform.position) < 50)
                    {
                        dragTarget = drag;
                        break;
                    }
                }
                if (dragTarget != null)
                {
                    Sprite oldSprite = obj.transform.Find("sprite").GetComponent<Image>().sprite;
                    obj.transform.Find("sprite").GetComponent<Image>().sprite = dragTarget.transform.Find("sprite").GetComponent<Image>().sprite;
                    dragTarget.transform.Find("sprite").GetComponent<Image>().sprite = oldSprite;
                }
            };
        });
        tbl_drag.reloadData();

    }
}
