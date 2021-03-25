using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManage : MonoBehaviour
{
    public enum UIState //select 选择阵营, fixUp 布置, run 运行
    {
        select, fixUp, run
    }
    public UIState CurrentState;//当前页
    public Button redBtn;//红方Btn
    public Button blueBtn;//蓝方Btn
    public Button backBtn;//返回Btn
    public Button pawnBtn1;//小兵1Btn
    public Button pawnBtn2;//小兵2Btn
    public Button pawnBtn3;//小兵3Btn
    public Button againBtn;//重新布置/开始游戏Btn
    public Text again_t;//重新布置/开始游戏Text
    public Text win_t;//胜利Text
    public Text pawnHp_t;//小兵——血量value
    public Text pawAtk_t;//小兵——攻击value
    public Text pawnSpeed_t;//小兵——速度value
    public Image currentpawnImage;//选中小兵的详情框
    public GameObject[] Pawn;//小兵[]
    public Transform[] target;//巡逻目标点[]
    private int currentPawn;//选中的小兵
    private string pawntag;//阵营Tag ： Blue/Red
    private bool win;//蓝方赢：true；红方赢：false

    // Start is called before the first frame update
    void Start()
    {
        BindView();
        CurrentState = UIState.select;
    }
    public void BindView()
    {
        redBtn = transform.Find("RedBtn").GetComponent<Button>();
        blueBtn = transform.Find("BlueBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        pawnBtn1 = transform.Find("PawnBtn 01").GetComponent<Button>();
        pawnBtn2 = transform.Find("PawnBtn 02").GetComponent<Button>();
        pawnBtn3 = transform.Find("PawnBtn 03").GetComponent<Button>();
        againBtn = transform.Find("AgainBtn").GetComponent<Button>();
        again_t = againBtn.transform.Find("Text").GetComponent<Text>();
        win_t = transform.Find("Text").GetComponent<Text>();
        currentpawnImage = transform.Find("Image").GetComponent<Image>();
        pawnHp_t = currentpawnImage.transform.Find("Hp_value").GetComponent<Text>();
        pawAtk_t = currentpawnImage.transform.Find("Atk_value").GetComponent<Text>();
        pawnSpeed_t = currentpawnImage.transform.Find("Speed_value").GetComponent<Text>();
        redBtn.onClick.AddListener(RedBtnOnClick);
        blueBtn.onClick.AddListener(BlueOnClick);
        backBtn.onClick.AddListener(BackBtnOnClick);
        pawnBtn1.onClick.AddListener(PawnBtn1OnClick);
        pawnBtn2.onClick.AddListener(PawnBtn2OnClick);
        pawnBtn3.onClick.AddListener(PawnBtn3OnClick);
        againBtn.onClick.AddListener(AgainBtnOnClick);
    }
    //布置红方小兵
    public void RedBtnOnClick()
    {
        pawntag = "Red";
        CurrentState = UIState.fixUp;
        CurrentPawn();
    }
    //布置蓝方小兵
    public void BlueOnClick()
    {
        pawntag = "Blue";
        CurrentState = UIState.fixUp;
        CurrentPawn();
    }
    //返回选择阵营页
    public void BackBtnOnClick()
    {
        CurrentState = UIState.select;
    }
    //选择小兵1
    public void PawnBtn1OnClick()
    {
        currentPawn = 0;
        CurrentPawn();
    }
    //选择小兵2
    public void PawnBtn2OnClick()
    {
        currentPawn = 1;
        CurrentPawn();
    }
    //选择小兵3
    public void PawnBtn3OnClick()
    {
        currentPawn = 2;
        CurrentPawn();
    }
    //开始游戏/重新布置
    public void AgainBtnOnClick()
    {
        if (again_t.text == "开始游戏~")//开始游戏
        {
            CurrentState = UIState.run;
            //重置所有小兵状态
            foreach (var item in GameObject.FindGameObjectsWithTag("Blue"))
            {
                if (item.GetComponent<Pawn1>() != null)
                {
                    item.GetComponent<Pawn1>().ChangeIdle();
                }
                else if (item.GetComponent<Pawn2>() != null)
                {
                    item.GetComponent<Pawn2>().ChangeIdle();
                }

            }
            //重置所有小兵状态
            foreach (var item in GameObject.FindGameObjectsWithTag("Red"))
            {
                if (item.GetComponent<Pawn1>() != null)
                {
                    item.GetComponent<Pawn1>().ChangeIdle();
                }
                else if (item.GetComponent<Pawn2>() != null)
                {
                    item.GetComponent<Pawn2>().ChangeIdle();
                }
            }
        }
        else//重新布置
        {
            CurrentState = UIState.select;
            again_t.text = "开始游戏~";
            againBtn.gameObject.SetActive(false);
            //蓝方胜清除在场所有蓝方小兵，反之...
            if (win)
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("Blue"))
                {
                    Destroy(item);
                }
            }
            else
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("Red"))
                {
                    Destroy(item);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case UIState.select:
                Select();
                break;
            case UIState.fixUp:
                FixUp();
                break;
            case UIState.run:
                Run();
                break;
        }
    }
    //UIState：选择阵营
    public void Select()
    {
        if (!redBtn.IsActive())
        {
            redBtn.gameObject.SetActive(true);
        }
        if (!blueBtn.IsActive())
        {
            blueBtn.gameObject.SetActive(true);
        }

        if (pawnBtn1.IsActive())
        {
            pawnBtn1.gameObject.SetActive(false);
        }
        if (pawnBtn2.IsActive())
        {
            pawnBtn2.gameObject.SetActive(false);
        }
        if (pawnBtn3.IsActive())
        {
            pawnBtn3.gameObject.SetActive(false);
        }
        if (backBtn.IsActive())
        {
            backBtn.gameObject.SetActive(false);
        }
        if (currentpawnImage.IsActive())
        {
            currentpawnImage.gameObject.SetActive(false);
        }
        if (win_t.IsActive())
        {
            win_t.gameObject.SetActive(false);
        }
        if ((GameObject.FindWithTag("Blue") != null) && (GameObject.FindWithTag("Red") != null))
        {
            if (!againBtn.IsActive())
            {
                againBtn.gameObject.SetActive(true);
            }
        }
    }
    //UIState：布置小兵
    public void FixUp()
    {
        if (redBtn.IsActive())
        {
            redBtn.gameObject.SetActive(false);
        }
        if (blueBtn.IsActive())
        {
            blueBtn.gameObject.SetActive(false);
        }
        if (againBtn.IsActive())
        {
            againBtn.gameObject.SetActive(false);
        }
        if (!pawnBtn1.IsActive())
        {
            pawnBtn1.gameObject.SetActive(true);
        }
        if (!pawnBtn2.IsActive())
        {
            pawnBtn2.gameObject.SetActive(true);
        }
        if (!pawnBtn3.IsActive())
        {
            pawnBtn3.gameObject.SetActive(true);
        }
        if (!currentpawnImage.IsActive())
        {
            currentpawnImage.gameObject.SetActive(true);
        }
        if (!backBtn.IsActive())
        {
            backBtn.gameObject.SetActive(true);
        }
        PlacePawn();
    }
    //UIState：开始游戏
    public void Run()
    {
        if (redBtn.IsActive())
        {
            redBtn.gameObject.SetActive(false);
        }
        if (blueBtn.IsActive())
        {
            blueBtn.gameObject.SetActive(false);
        }
        if (againBtn.IsActive())
        {
            againBtn.gameObject.SetActive(false);
        }

        //场上不再有蓝方小兵，红方胜
        if ((GameObject.FindWithTag("Blue") == null))
        {
            if (!againBtn.IsActive())
            {
                again_t.text = "重新布置";
                againBtn.gameObject.SetActive(true);
                win = false;
                win_t.gameObject.SetActive(true);
                win_t.text = "红方胜利";
                win_t.color = Color.red;
            }
        }
        //反之场上不再有红方小兵，蓝方胜
        else if ((GameObject.FindWithTag("Red") == null))
        {
            if (!againBtn.IsActive())
            {
                again_t.text = "重新布置";
                againBtn.gameObject.SetActive(true);
                win = true;
                win_t.gameObject.SetActive(true);
                win_t.text = "蓝方胜利";
                win_t.color = Color.blue;
            }
        }
    }
    //右键布置小兵
    private void PlacePawn()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Pawn[currentPawn].tag = pawntag;
                if (Pawn[currentPawn].GetComponent<Pawn1>() != null)
                {
                    Pawn1 pawn = Pawn[currentPawn].GetComponent<Pawn1>();
                    pawn.target = target;
                }
                else if (Pawn[currentPawn].GetComponent<Pawn2>() != null)
                {
                    Pawn2 pawn = Pawn[currentPawn].GetComponent<Pawn2>();
                    pawn.target = target;
                }
                Instantiate(Pawn[currentPawn], new Vector3(hit.point.x, 0, hit.point.z), transform.rotation);
            }
        }
    }
    ///显示小兵数值
    private void CurrentPawn()
    {
        Pawn[currentPawn].tag = pawntag;
        if (Pawn[currentPawn].GetComponent<Pawn1>() != null)
        {
            Pawn1 pawn = Pawn[currentPawn].GetComponent<Pawn1>();
            pawnHp_t.text = pawn.hp.ToString();
            pawAtk_t.text = pawn.attack.ToString();
            pawnSpeed_t.text = pawn.speed.ToString();
        }
        else if (Pawn[currentPawn].GetComponent<Pawn2>() != null)
        {
            Pawn2 pawn = Pawn[currentPawn].GetComponent<Pawn2>();
            pawnHp_t.text = pawn.hp.ToString();
            pawAtk_t.text = pawn.attack.ToString();
            pawnSpeed_t.text = pawn.speed.ToString();
        }
    }
}
