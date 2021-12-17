using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayUI : BaseUI
{
    // >>>> ������ ���� ������Ʈ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    [Header("Ÿ�̸� �̹���")]
    [Header("=============================================")]
    public Image IMG_Timer;

    [Header("���� �ؽ�Ʈ")]
    public TextMeshProUGUI TXT_Score;

    [Header("��ġ ��ȸ �̹�����")]
    public Image[] IMG_TouchChances;

    [Header("��ȸ ���� & ���� �̹���")]
    public Sprite SPR_ChanceAvailable;
    public Sprite SPR_ChanceUsed;

    [Header("��ġ ��ư ������")]
    public GameObject Prefab_TouchBtn;

    [Header("�� UI���� ������ ����ŷ ������Ʈ")]
    public PendulumMovement Viking;




    // >>>> �ܺ� ���� ������ ���� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    [Header("���� ���� �ð�")]
    [Header("=============================================")]
    public float TIME_LIMIT = 60f;

    [Header("��ġ ��ư ���͹�")]
    public float INTERVAL_TOUCH_BTN = 0.15f;

    [Header("���� ���(���� ���� ���� ����)")]
    [Range(1f, 10f)]
    public float PerfectCoef = 5f;




    // >>>> ���� ������Ʈ, ������Ʈ >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    private RectTransform tf_rect;
    GameObject btn;



    // >>>> ���� ���� ���� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    private float timer;                                // ���� ���� Ÿ�̸�
    private float timer_touchInterval;                  // ��ġ ��ư ���͹� Ÿ�̸�
    private int count_change;                           // ���� ��ġ ���� Ƚ��
    private bool bTouchBtnSpawned;                      // ��ġ ��ư ���� ����




    // >>>> �Լ� >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    // Start is called before the first frame update
    void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        // ������Ʈ �ʱ�ȭ
        tf_rect = GetComponent<RectTransform>();

        // ���� �ʱ�ȭ
        timer = TIME_LIMIT;
        timer_touchInterval = INTERVAL_TOUCH_BTN;
        count_change = 5;
        bTouchBtnSpawned = false;
        GameStateManager.Inst.score = 0;

        // UI �ʱ�ȭ
        IMG_Timer.fillAmount = timer / TIME_LIMIT;
        TXT_Score.text = GameStateManager.Inst.score.ToString();
        foreach(Image img in IMG_TouchChances)
            img.sprite = SPR_ChanceAvailable;
        
        // ���� ������Ʈ Ȱ��ȭ
        Viking.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        SpawnTouchBtn();
    }

    private void UpdateTimer()
    {
        // Ÿ�̸Ӹ� ���ҽ�Ű�ų�, �ð��� �� ������ ��� â���� �̵��Ѵ�.
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            IMG_Timer.fillAmount = timer / TIME_LIMIT;
        }
        else
        {
            StartUI_Manager.Inst.ChangeUI(UI_Type.Finish);
        }
    }

    public void UpdateScore(int value)
    {
        GameStateManager.Inst.score += value;
        TXT_Score.text = GameStateManager.Inst.score.ToString();
    }

    public void DecreaseChance()
    {
        // �ε��� ���� ������ ���� ��谪 üũ
        if(count_change <= 0)   return;

        count_change--;
        IMG_TouchChances[count_change].sprite = SPR_ChanceUsed;
    }

    private void SpawnTouchBtn()
    {
        // ���� Ÿ�̸Ӹ� ���ҽ�Ű�� �����Ѵ�.
        if(timer_touchInterval > 0f)
        {
            timer_touchInterval -= Time.deltaTime;
            return;
        }

        // Ȱ��ȭ�� ��ġ ��ư�� �̹� �ִٸ� �����Ѵ�.
        if(bTouchBtnSpawned)    return;

        // == ��ġ�� ��ư�� �����Ѵ�. ==================================
        // ������ ��ġ�� �����´�(���� ��ǥ)
        Vector3 spawnPoint = Viking.GetRandomTouchBtnSpawnPoint();

        // ������ ���� ��ǥ�� UI anchored ��ǥ�� ��ȯ�Ѵ�.
        Vector2 anchoredPosition = WorldPositionToCanvasAnchoredPosition(spawnPoint);

        // ��ư�� ���� ��Ű�� ��ġ �̺�Ʈ�� �����Ų��.
        btn = Instantiate(Prefab_TouchBtn, transform);
        btn.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        btn.GetComponent<Button>().onClick.AddListener(OnClickTouchBtn);

        // ���� ���θ� true�� �ٲ��ش�.
        bTouchBtnSpawned = true;
    }

    private Vector2 WorldPositionToCanvasAnchoredPosition(Vector3 worldPosition)
    {
        Vector2 anchoredPosition;

        worldPosition = Camera.main.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tf_rect, worldPosition, null, out anchoredPosition);

        return anchoredPosition;
    }

    public void OnClickTouchBtn()
    {
        // ��ư�� anchoredPosition�� ���Ѵ�.
        Vector2 btnPos = btn.GetComponent<RectTransform>().anchoredPosition;

        // �Ӹ��� ������ ���� ��ǥ�� anchoredPosition���� ��ȯ�Ѵ�.
        Vector2 headPos = WorldPositionToCanvasAnchoredPosition(Viking.GetHeadPosition());
        Vector2 tailPos = WorldPositionToCanvasAnchoredPosition(Viking.GetTailPosition());

        // �Ӹ��� ������ �� ����� ���� �������� ������ �ø���.
        bool bIsNearHead = true;
        float distance = 0f;
        if((headPos - btnPos).sqrMagnitude <= (tailPos - btnPos).sqrMagnitude)
        {
            // �Ӹ��� �� ����� ���
            bIsNearHead = true;
            distance = Vector2.Distance(headPos, btnPos) / PerfectCoef;
            GameStateManager.Inst.score += (distance < 1f) ? 1000 : (int)(1000f / distance);
        }
        else
        {
            // ������ �� ����� ���
            bIsNearHead = false;
            distance = Vector2.Distance(tailPos, btnPos) / PerfectCoef;
            GameStateManager.Inst.score += (distance < 1f) ? 500 : (int)(500 / distance);
        }
        TXT_Score.text = GameStateManager.Inst.score.ToString();

        // === ������ ���� ����ŷ�� ���� ���� ��½�Ű�ų� �϶���Ų��. =====================================
        // ���� ����� �������� ��� ������ ��ٷο�����.(�� ������ �Ȱ�ġ�� ����� �������� ��� ������ �ʹ� ��������.)
        float increaseCoef = Mathf.Lerp(6f, 1f, Mathf.InverseLerp(1f, 10f, PerfectCoef));
        if(distance / increaseCoef > 1f)
        {
            // ���� ������ ���� ���. ���� 30% ���δ�.
            Viking.SetPolarRotation(Viking.CurrentPolarRot * 0.7f);
            Debug.Log("Failed!");
        }
        else
        {
            // ���� ������ ���� ���. ��꿡 ���� ���� ������Ų��.
            if(bIsNearHead)
            {
                // �Ӹ��� �ִ� 15, �ּ� 10��ŭ ���� ����Ѵ�.
                float increaseAngle = Mathf.Lerp(15f, 10f, distance / increaseCoef);
                if(distance < 1f)   increaseCoef = 15f;
                Viking.SetPolarRotation(Viking.CurrentPolarRot + increaseAngle);
                Debug.Log("Head Increase Angle: " + increaseAngle);
            }
            else
            {
                // ������ ������ 5����ŭ ���� ����Ѵ�.
                Viking.SetPolarRotation(Viking.CurrentPolarRot + 5f);
                Debug.Log("Tail Increase: 5");
            }
        }

        // ��ġ ��ȸ�� �谨�Ѵ�.
        DecreaseChance();

        // ��ư ���� ���θ� �����ϰ� Ŭ���� ��ư�� �����Ѵ�.
        bTouchBtnSpawned = false;
        Destroy(btn);

        // ��ġ ��ȸ�� �� ��ٸ� ���â UI�� ����ش�.
        if(count_change == 0)
            StartUI_Manager.Inst.ChangeUI(UI_Type.Finish);
    }
}
