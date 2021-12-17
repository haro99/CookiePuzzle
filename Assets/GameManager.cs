using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public int X, Y;
}
public enum Mode {
    Wait,
    Play
}
public class GameManager : MonoBehaviour
{
    public const int setsize = 9;
    public GameObject[] Cookies;
    public GameObject[,] Sets = new GameObject[setsize, setsize];
    public int[] drops = new int[setsize];
    public Mode Status;
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
        Status = Mode.Play;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check(GameObject TouchCookie)
    {
        if (Status == Mode.Wait)
            return;

        SetDate date = TouchCookie.GetComponent<SetDate>();
        Debug.Log(date.x +" "+date.y);
        Destroy(Sets[date.y, date.x]);
        Status = Mode.Wait;
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
        //Stack<Position> Relocation = new Stack<Position>();
        Stacks.Push(positon);
        //Debug.Log(Stacks.Count);
        Debug.Log(date.x + " " + date.y);

        Debug.Log("探索開始");

        int point = 0;
        //通った履歴
        bool[,] history = new bool[9, 9];
        history[positon.X, positon.Y] = true;
        while (Stacks.Count > 0)
        {
            Position nowpoition = Stacks.Pop();
            point += 100;
            Debug.Log(nowpoition.X+","+nowpoition.Y);
            Destroy(Sets[nowpoition.Y, nowpoition.X]);
            //Relocation.Push(nowpoition);
            drops[nowpoition.X]++;
            Sets[nowpoition.Y, nowpoition.X] = null;

            //同じCookieがあるかどうか探す
            if (CheckSize(nowpoition.X, nowpoition.Y + 1) && !history[nowpoition.X, nowpoition.Y + 1]&& Checknumber(nowpoition.X, nowpoition.Y + 1, cookiename))
            {
                Debug.Log("上範囲内");
                Position uppositon = nowpoition;
                uppositon.Y++;
                Stacks.Push(uppositon);
                history[nowpoition.X, nowpoition.Y + 1] = true;
            }
            if (CheckSize(nowpoition.X + 1, nowpoition.Y) && !history[nowpoition.X + 1, nowpoition.Y] && Checknumber(nowpoition.X + 1, nowpoition.Y, cookiename))
            {
                Debug.Log("左範囲内");
                Position leftpositon = nowpoition;
                leftpositon.X++;
                Stacks.Push(leftpositon);
                history[nowpoition.X + 1, nowpoition.Y] = true;
            }
            if (CheckSize(nowpoition.X, nowpoition.Y - 1) && !history[nowpoition.X, nowpoition.Y - 1] && Checknumber(nowpoition.X, nowpoition.Y - 1, cookiename))
            {
                Debug.Log("下範囲内");
                Position downposition = nowpoition;
                downposition.Y--;
                Stacks.Push(downposition);
                history[nowpoition.X, nowpoition.Y - 1] = true;
            }
            if (CheckSize(nowpoition.X - 1, nowpoition.Y) && !history[nowpoition.X - 1, nowpoition.Y] && Checknumber(nowpoition.X - 1, nowpoition.Y, cookiename))
            {
                Debug.Log("左範囲内");
                Position rightposition = nowpoition;
                rightposition.X--;
                Stacks.Push(rightposition);
                history[nowpoition.X - 1, nowpoition.Y] = true;
            }
        }
        Debug.Log(point);

        //落ちるクッキーの位置更新
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j  < 9; j ++)
            {
                if (!Sets[j, i])
                {
                    for (int n = j + 1; n < 9; n++)
                    {
                        if(Sets[n, i])
                        {
                            Sets[j, i] = Sets[n, i];
                            SetDate set = Sets[j, i].GetComponent<SetDate>();
                            set.x = i;
                            set.y = j;
                            Sets[n, i] = null;
                            break;
                        }
                    }
                }
            }
        }

        Debug.Log("ドロップ開始");
        StartCoroutine(Drop());
        //Debug.Log("ストック" + Relocation.Count);
        //while (Relocation.Count > 0)
        //{
        //    Position Setposition = Relocation.Pop();
        //    Debug.Log(Setposition.X + " " + Setposition.Y);
        //    GameObject SetObj = Instantiate(Cookies[Random.Range(0, Cookies.Length)], new Vector3(-4, -4, 0) + new Vector3(Setposition.X, Setposition.Y), Quaternion.identity);
        //    SetDate setdate = SetObj.GetComponent<SetDate>();
        //    setdate.x = Setposition.X; setdate.y = Setposition.Y;
        //    Sets[Setposition.Y, Setposition.X] = SetObj;
        //}
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

    IEnumerator Drop()
    {

        for (int i = 0; i < drops.Length; i++)
        {
            for (int j = drops[i]; j > 0; j--)
            {
                GameObject dropCookie = Instantiate(Cookies[Random.Range(0, Cookies.Length)], new Vector3(-4, 6, 0) + new Vector3(i, 0, 0), Quaternion.identity);
                Sets[9 - j, i] = dropCookie;
                Debug.Log(i + " " + (9 - j));
                dropCookie.GetComponent<SetDate>().x = i;
                dropCookie.GetComponent<SetDate>().y = 9 - j;
                yield return new WaitForSeconds(0.3f);
            }
            drops[i] = 0;
        }

        yield return new WaitForSeconds(1);
        Status = Mode.Play;
    }
}

