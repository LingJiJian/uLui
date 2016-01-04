using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using Lui;

public class Firstui : MonoBehaviour
{
    public Button btn_grid;
    public Button btn_trans;
    public LControlView ctrlView;
    public LTableView tblView;
    public LScrollView scrolView;
    public LRichText rtfView;
    public LPageView pageView;
    public LListView listView;
    public LGridView gridView;

    private LWindowManager wm;

    void Start()
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

        ctrlView.onControlHandler = (float ox, float oy) =>
        {
            Debug.Log(string.Format("offsetX={0} offsetY={1}", ox, oy));
        };

        scrolView.onMoveCompleteHandler = () =>
        {
            Debug.Log(" scrolView.onMoveCompleteHandler ");
        };
        
        tblView.cellsSize = new Vector2(150, 40);
        tblView.cellTemplate.node = Resources.Load("Prefabs/tbl_cell") as GameObject;
        tblView.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 40 * 5);
        tblView.cellsCount = 100;
        tblView.onDataSourceAdapterHandler = (LTableViewCell cell, int idx) =>
        {
            if (cell == null)
            {
                cell = new LTableViewCell();
                cell.node = (GameObject)Instantiate(tblView.cellTemplate.node);
            }
            cell.node.GetComponent<Text>().text = idx.ToString();
            return cell;
        };
        tblView.reloadData();

        rtfView.insertElement("hello world!!", Color.blue, 25, true, false, Color.blue, "数据");
        rtfView.insertElement("测试文本内容!!", Color.red, 15, false, true, Color.blue, "");
        rtfView.insertElement("Image/face01", 5f, "");
        rtfView.insertElement("The article comes from the point of the examination", Color.green, 15, true, false, Color.blue, "");
        rtfView.insertElement("Image/face02/1", "");
        rtfView.insertElement(1);
        rtfView.insertElement("outline and newline", Color.yellow, 20, false, true, Color.blue, "");
        rtfView.onClickHandler = (string data) =>
        {
            Debug.Log("data " + data);
        };
        rtfView.reloadData();

        pageView.cellsSize = new Vector2(150, 100);
        pageView.cellTemplate.node = Resources.Load("Prefabs/page_cell") as GameObject;
        pageView.cellsCount = 14;
        pageView.onDataSourceAdapterHandler = (LTableViewCell cell, int idx) =>
        {
            if (cell == null)
            {
                cell = new LTableViewCell();
                cell.node = (GameObject)Instantiate(pageView.cellTemplate.node, Vector3.zero, pageView.cellTemplate.node.transform.rotation);
            }
            cell.node.transform.FindChild("Text").GetComponent<Text>().text = idx.ToString();
            return cell;
        };
        pageView.onPageChangedHandler = (int pageIdx) =>
        {
            Debug.Log("page " + pageIdx);
        };
        pageView.reloadData();

        RectTransform rtran = listView.GetComponent<RectTransform>();
        listView.bounceBox = new Rect(listView.transform.position.x,
                             listView.transform.position.y,
                             rtran.rect.width,
                             rtran.rect.height);

        listView.itemTemplate = Resources.Load("Prefabs/list_cell") as GameObject;
        listView.limitNum = 10; //not must to set limitNum
        for (int i = 0; i < 30; i++)
        {
            GameObject item = listView.dequeueItem();
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40 + Random.Range(0, 40));
            item.GetComponent<Text>().text = i.ToString();
            listView.insertNodeAtLast(item);
        }
        listView.reloadData();

        gridView.cellsSize = new Vector2(100, 100);
        gridView.cellTemplate.node = Resources.Load("Prefabs/grid_cell") as GameObject;
        gridView.cols = 4;
        gridView.cellsCount = 100;
        gridView.onDataSourceAdapterHandler = (LGridViewCell cell, int idx) =>
        {
            if (cell == null)
            {
                cell = new LGridViewCell();
                cell.node = (GameObject)Instantiate(gridView.cellTemplate.node);
            }
            cell.node.GetComponent<Text>().text = idx.ToString();
            return cell;
        };
        gridView.reloadData();
    }
}
