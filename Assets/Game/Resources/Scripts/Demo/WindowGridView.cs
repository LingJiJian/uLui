using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Lui;

public class WindowGridView : LWindowBase
{
    public Button btn_close;
    public LGridPageView gridPageView;
    private LWindowManager wm;

    public WindowGridView()
    {
        this.disposeType = WindowDispose.Normal;
    }

    void Start()
    {
        btn_close.onClick.AddListener(() =>
        {
            LWindowManager wm = LWindowManager.GetInstance();
            wm.popWindow(this);
        });

        gridPageView.cellsSize = new Vector2(400, 400);
		gridPageView.cellTemplate.node = LLoadBundle.GetInstance().LoadAsset<GameObject>("Ab/prefabs-grid_cell_prefab.ab", "Prefabs/grid_cell.prefab");
        gridPageView.cols = 4;
        gridPageView.rows = 4;
        gridPageView.gridCellsCount = 100;
        gridPageView.gridCellsSize = new Vector2(100, 100);
        gridPageView.onGridDataSourceAdapterHandler = (LGridPageViewCell cell, int idx) =>
        {
            if (cell == null)
            {
                cell = new LGridPageViewCell();
                cell.node = (GameObject)Instantiate(gridPageView.cellTemplate.node, Vector3.zero, gridPageView.cellTemplate.node.transform.rotation);
            }
            cell.node.GetComponent<Text>().text = idx.ToString();
            cell.node.SetActive(idx != LScrollView.INVALID_INDEX);
            return cell;
        };
        gridPageView.reloadData();
    }

    public override void open(ArrayList list)
    {
        base.open(list);
        if (list!=null)
        {
            foreach (var item in list)
            {
                Debug.Log("数据:"+item.ToString());
            }
        }
    }

    public override void close()
    {
        base.close();
    }
}