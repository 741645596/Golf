using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(CanvasRenderer))]
public class PowerItem : Graphic
{
    private VertexHelper vertextHelper;
    public int maxStep = 30;
    public float gy = 0.1f;
    public float gx = 1.0f;

    public Vector2 ScreenA = new Vector2(10, 10);
    public Vector2 ScreenB = new Vector2(100, 300);


    //重绘UI的Mesh
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        DrawPentagon(vh);
    }


    //绘制五边形
    private void DrawPentagon(VertexHelper vh)
    {
        Vector2 outVec;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, ScreenA, null, out outVec);

        Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
        Color32 color = (Color32)this.color;
        vh.Clear();
        Vector2 top = new Vector2(pixelAdjustedRect.x, pixelAdjustedRect.y);
        float dx = pixelAdjustedRect.width / (maxStep * 2);
        float dy = pixelAdjustedRect.height / (maxStep * 2);

        //Vector2 top = new Vector2(outVec.x, outVec.y);
        //float dx = (ScreenB.x - ScreenA.x) / (maxStep * 2);
        //float dy = (ScreenB.y - ScreenA.y) / (maxStep * 2);



        float f = 1.0f / (2 * maxStep);
        for (int i = 0; i <= maxStep; i++)
        {
            if (i * 2 < maxStep)
            {
                float xdis = RunXDistance(i);
                float dis = RunYDistance(i);
                vh.AddVert(new Vector3(top.x + xdis * dx, top.y + dis * dy), color, new Vector2(0, dis * f));
                vh.AddVert(new Vector3(top.x + (2 * maxStep - xdis) * dx, top.y + dis * dy), color, new Vector2(1, dis * f));
            }
            else
            {   
                float dis = 2 * maxStep - RunYDistance(maxStep - i);
                float xdis = RunXDistance(maxStep - i);
                vh.AddVert(new Vector3(top.x + (maxStep -i) * dx, top.y + dis * dy), color, new Vector2(0, dis * f));
                vh.AddVert(new Vector3(top.x + (2 * maxStep - xdis) * dx, top.y + dis * dy), color, new Vector2(1, dis * f));
            }
        }

        for (int i = 0; i < maxStep; i++)
        {
            vh.AddTriangle(2 * i , 2 * i + 1, 2 * (i + 1));
            vh.AddTriangle(2 * (i + 1), 2 * i + 1, 2 * (i + 1) + 1);
        }
    }
    /// <summary>
    /// 类似抛物线公式
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private float RunYDistance(int i)
    {
        return i * i  * gy;
    }

    /// <summary>
    /// 类似抛物线公式
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private float RunXDistance(int i)
    {
        return i * gx;
    }
}
