using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class UIMultiScroller : MonoBehaviour
{
    public delegate void VoidDelegate(int index);
    public VoidDelegate OnCreateItem;
    public delegate void GODelegate(int index, GameObject go);
    public GODelegate OnSetItemData;

    public delegate void OnScrollValueChange();
    public OnScrollValueChange OnScrollChange;

    public enum Arrangement { Horizontal, Vertical, }
    public Arrangement _movement = Arrangement.Horizontal;

    //单行或单列的Item数量
    [Range(1, 20)]
    public int maxPerLine = 5;

    //content缩进距离
    [Range(0, 50)]
    public int contentOffX = 25;

    //content缩进距离
    [Range(0, 50)]
    public int contentOffY = 25;

    //默认加载的行数，一般比可显示行数大2~3行
    [Range(0, 20)]
    public int viewCount = 6;
    public RectTransform _content;

    //Item的宽高
    public GameObject itemPrefab; // scrollview的cell
    public int cellWidth = 0;
    public int cellHeight = 0;

    private int _index = -1; // 当前滑动到那个行/列
    public List<UIMultiScrollIndex> _itemList; // 显示的item
    private int _dataCount; // item总个数
    private Queue<UIMultiScrollIndex> _unUsedQueue;  //将未显示出来的Item存入未使用队列里面，等待需要使用的时候直接取出
    public ScrollRect ScrollRectGo;
    public bool m_Fly = true;
    private int m_FlyCount = 0;


    void Start()
    {

    }

    public void DealFly() {
        if (m_Fly) {
            --m_FlyCount;
            if (0 >= m_FlyCount) {
                m_Fly = false;
                m_FlyCount = -1;
            }
        }
    }

    public void Clear() {
        if (null != _itemList) {
            for (int i = 0; i < _itemList.Count; i++) {
                DestroyImmediate(_itemList[i].gameObject);
            }

            _itemList.Clear();
        }

        if (null != _content) {
            foreach (Transform item in _content.transform) {
                DestroyImmediate(item.gameObject);
            }
        }

        if (null != _unUsedQueue) {
            for (int i = _unUsedQueue.Count; i > 0; i--) {
                UIMultiScrollIndex item = _unUsedQueue.Dequeue();
                if (null != item) {
                    DestroyImmediate(item);
                }
            }

            _unUsedQueue.Clear();
        }

        _index = -1;
        _dataCount = 0;
        UpdateTotalWidth();
    }

    public void InitData(int nCount) {
        Clear();
        _index = -1;
        _itemList = new List<UIMultiScrollIndex>();
        _unUsedQueue = new Queue<UIMultiScrollIndex>();
        DataCount = nCount;
        m_FlyCount = DataCount > maxPerLine * viewCount ? maxPerLine * viewCount : DataCount;
        OnValueChange(Vector2.zero);
        ScrollBarDelayDis(0);
    }
    private void ScrollBarDelayDis(float delay = 2.0f) {
        switch (_movement) {
            case Arrangement.Horizontal: {
                    if (null != ScrollRectGo && null != ScrollRectGo.horizontalScrollbar) {
                        ScrollRectGo.horizontalScrollbar.image.DOComplete();
                        if (0 >= delay) {
                            ScrollRectGo.horizontalScrollbar.image.DOComplete();
                        }
                        ScrollRectGo.horizontalScrollbar.image.DOFade(1, 0);
                        ScrollRectGo.horizontalScrollbar.image.DOFade(0, delay);
                    }
                }
                break;
            case Arrangement.Vertical: {
                    if (null != ScrollRectGo && null != ScrollRectGo.verticalScrollbar) {
                        if (0 >= delay) {
                            ScrollRectGo.verticalScrollbar.image.DOComplete();
                        }
                        ScrollRectGo.verticalScrollbar.image.DOFade(1, 0);
                        ScrollRectGo.verticalScrollbar.image.DOFade(0, delay);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void OnValueChange(Vector2 pos)
    {
        if (null != OnScrollChange) {
            OnScrollChange();
        }

        if (null != ScrollRectGo) {
            ScrollBarDelayDis();
        }

        int index = GetPosIndex();
        if (_index != index && index > -1)
        {
            _index = index;
            for (int i = _itemList.Count; i > 0; i--)
            {
                UIMultiScrollIndex item = _itemList[i - 1];
                if (item.Index < index * maxPerLine || (item.Index >= (index + viewCount) * maxPerLine))
                {
                    //GameObject.Destroy(item.gameObject);
                    _itemList.Remove(item);
                    _unUsedQueue.Enqueue(item);
                }
            }
            for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
            {
                if (i < 0) continue;
                if (i > _dataCount - 1) continue;
                bool isOk = false;
                foreach (UIMultiScrollIndex item in _itemList)
                {
                    if (item.Index == i) isOk = true;
                }
                if (isOk) continue;
                int Index = i;
                OnCreateItem(Index);
            }
        }
    }

    /// <summary>
    /// 提供给外部的方法，添加指定位置的Item
    /// </summary>
    public void AddItem<T>(int index, T item) where T : WndItem {
        if (index > _dataCount)
        {
            Debug.LogError("添加错误:" + index);
            return;
        }
        AddItemIntoPanel(index, item);
        DataCount += 1;
    }

    /// <summary>
    /// 提供给外部的方法，删除指定位置的Item
    /// </summary>
    public void DelItem<T>(int index, T item) where T : WndItem {
        if (index < 0 || index > _dataCount - 1)
        {
            Debug.LogError("删除错误:" + index);
            return;
        }
        DelItemFromPanel(index, item);
        DataCount -= 1;
    }

    private void AddItemIntoPanel<T>(int index, T item) where T : WndItem {
        for (int i = 0; i < _itemList.Count; i++)
        {
            UIMultiScrollIndex cell = _itemList[i];
            if (cell.Index >= index) cell.Index += 1;
        }
        CreateItem(index, item);
    }

    private void DelItemFromPanel<T>(int index, T item) where T : WndItem {
        int maxIndex = -1;
        int minIndex = int.MaxValue;
        for (int i = _itemList.Count; i > 0; i--)
        {
            UIMultiScrollIndex cell = _itemList[i - 1];
            if (cell.Index == index)
            {
                GameObject.Destroy(item.gameObject);
                _itemList.Remove(cell);
            }
            if (cell.Index > maxIndex)
            {
                maxIndex = cell.Index;
            }
            if (cell.Index < minIndex)
            {
                minIndex = cell.Index;
            }
            if (cell.Index > index)
            {
                cell.Index -= 1;
            }
        }
        if (maxIndex < DataCount - 1)
        {
            CreateItem(maxIndex, item);
        }
    }

    public T CreateItem<T>(int index, T item) where T : WndItem {
        UIMultiScrollIndex itemBase;
        if (_unUsedQueue.Count > 0)
        {
            itemBase = _unUsedQueue.Dequeue();
            item = itemBase.GetComponent<T>();
        }
        else
        {
			ResourceManger.LoadWndItem(typeof(T).ToString(), _content, false,
				(g) =>{
					if(g != null){
						item = g.GetComponent<T>();
					}});
			itemBase = item.GetComponent<UIMultiScrollIndex>();
        }

        itemBase.Scroller = this;
        itemBase.Index = index;

        if (m_Fly) {
            itemBase.SetFlyIndex(maxPerLine, viewCount - 1, _movement);
        }

        DealFly();
        _itemList.Add(itemBase);


        // 通知修改数据
        OnSetItemData(index, itemBase.gameObject);

        return item;
    }

    private int GetPosIndex()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return Mathf.FloorToInt((_content.anchoredPosition.x  - 0.1f)/ -(cellWidth));
            case Arrangement.Vertical:
                return Mathf.FloorToInt((_content.anchoredPosition.y + 0.1f) / (cellHeight));
        }
        return 0;
    }

    public Vector3 GetPosition(int i)
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return new Vector3(contentOffX + cellWidth * (i / maxPerLine), -contentOffY - (cellHeight) * (i % maxPerLine), 0f);
            case Arrangement.Vertical:
                return new Vector3(contentOffX + cellWidth * (i % maxPerLine), -contentOffY - (cellHeight) * (i / maxPerLine), 0f);
        }
        return Vector3.zero;
    }

    public int DataCount
    {
        get { return _dataCount; }
        set
        {
            _dataCount = value;
            UpdateTotalWidth();
        }
    }

    private void UpdateTotalWidth()
    {
        int lineCount = Mathf.CeilToInt((float)_dataCount / maxPerLine);
        switch (_movement)
        {
            case Arrangement.Horizontal:
                _content.sizeDelta = new Vector2(cellWidth * lineCount + contentOffX, _content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, cellHeight * lineCount + contentOffY);
                break;
        }
    }
}
