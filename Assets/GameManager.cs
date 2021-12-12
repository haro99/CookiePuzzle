using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public int X, Y;
}
public class GameManager : MonoBehaviour
{
    public const int setsize = 9;
    public GameObject[] Cookies;
    public GameObject[,] Sets = new GameObject[setsize, setsize];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Sets[i, j] = Instantiate(Cookies[Random.Range(0, Cookies.Length)], new Vector3(-4, -4, 0) + new Vector3(j, i, 0), Quaternion.identity);
                SetDate date = Sets[i, j].GetComponent<SetDate>();
                date.x = j;
                date.y = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check(GameObject TouchCookie)
    {
        SetDate date = TouchCookie.GetComponent<SetDate>();
        Debug.Log(date.x +" "+date.y);
        Destroy(Sets[date.y, date.x]);
        CheckCookie(date, TouchCookie.tag);
    }

    /// <summary>
    /// クッキーの処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="cookiename"></param>
    private void CheckCookie(SetDate date, string cookiename)
    {
        Position positon = new Position();
        positon.X = date.x; positon.Y = date.y;
        Stack<Position> Stacks = new Stack<Position>();
        Stack<Position> Relocation = new Stack<Position>();
        Stacks.Push(positon);
        //Debug.Log(Stacks.Count);

        int point = 0;
        while (Stacks.Count > 0)
        {
            Position nowpoition = Stacks.Pop();
            point += 100;
            Debug.Log(nowpoition.X+" "+nowpoition.Y);
            Destroy(Sets[nowpoition.Y, nowpoition.X]);
            Relocation.Push(nowpoition);
            Sets[nowpoition.Y, nowpoition.X] = null;

            //同じCookieがあるかどうか探す
            if (CheckSize(nowpoition.X, nowpoition.Y + 1) && SetObject(nowpoition.X, nowpoition.Y + 1)&& Checknumber(nowpoition.X, nowpoition.Y + 1, cookiename))
            {
                Debug.Log("上範囲内");
                Position uppositon = nowpoition;
                uppositon.Y++;
                Stacks.Push(uppositon);
            }
            if (CheckSize(nowpoition.X + 1, nowpoition.Y) && SetObject(nowpoition.X + 1, nowpoition.Y) && Checknumber(nowpoition.X + 1, nowpoition.Y, cookiename))
            {
                Debug.Log("左範囲内");
                Position leftpositon = nowpoition;
                leftpositon.X++;
                Stacks.Push(leftpositon);
            }
            if (CheckSize(nowpoition.X, nowpoition.Y - 1) && SetObject(nowpoition.X, nowpoition.Y - 1) && Checknumber(nowpoition.X, nowpoition.Y - 1, cookiename))
            {
                Debug.Log("下範囲内");
                Position downposition = nowpoition;
                downposition.Y--;
                Stacks.Push(downposition);
            }
            if (CheckSize(nowpoition.X - 1, nowpoition.Y) && SetObject(nowpoition.X - 1, nowpoition.Y) && Checknumber(nowpoition.X - 1, nowpoition.Y, cookiename))
            {
                Debug.Log("左範囲内");
                Position rightposition = nowpoition;
                rightposition.X--;
                Stacks.Push(rightposition);
            }
        }
        Debug.Log(point);

        Debug.Log("ストック" + Relocation.Count);
        while (Relocation.Count > 0)
        {
            Position Setposition = Relocation.Pop();
            Debug.Log(Setposition.X + " " + Setposition.Y);
            GameObject SetObj = Instantiate(Cookies[Random.Range(0, Cookies.Length)], new Vector3(-4, -4, 0) + new Vector3(Setposition.X, Setposition.Y), Quaternion.identity);
            SetDate setdate = SetObj.GetComponent<SetDate>();
            setdate.x = Setposition.X; setdate.y = Setposition.Y;
            Sets[Setposition.Y, Setposition.X] = SetObj;
        }
    }

    /// <summary>
    /// 範囲内処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckSize(int x, int y)
    {
        if (0 <= x 
            && setsize > x
            && 0 <= y
            && setsize > y)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// セット確認
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool SetObject(int x, int y)
    {
        if (Sets[y, x])
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 種類チェック
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool Checknumber(int x, int y, string name)
    {
        if(name == Sets[y, x].tag)
        {
            return true;
        }
        return false;
    }
}

