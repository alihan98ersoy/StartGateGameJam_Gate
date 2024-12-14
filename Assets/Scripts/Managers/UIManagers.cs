using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManagers : MonoBehaviourSingleton<UIManagers>
{
    public Sprite scannerSprite, crossSprite;

    public Image EkipmanImageUI, EkipmanCoolDown;

    public Text InteractionText;

    public float scannerCoolDown = 5f;
    public float crossCooldown = 5f;

    [HideInInspector]
    public EKIPMANLAR suankiEkipman = EKIPMANLAR.SCANNER;

    public void EkipmanIkonuDegistir(EKIPMANLAR enu) 
    {
        if (enu == EKIPMANLAR.SCANNER) 
        {
            suankiEkipman = enu;
            EkipmanImageUI.sprite = scannerSprite;
            EkipmanCoolDown.sprite = scannerSprite;
        }
        else if (enu == EKIPMANLAR.CROSS)
        {
            suankiEkipman = enu;
            EkipmanImageUI.sprite = crossSprite;
            EkipmanCoolDown.sprite = crossSprite;
        }
        else if (enu == EKIPMANLAR.COOLDOWN)
        {
            var timer = (suankiEkipman == EKIPMANLAR.SCANNER) ? scannerCoolDown : crossCooldown;
            var oncekiEkipman = suankiEkipman;
            suankiEkipman = enu;

            StartCoroutine(TimerBaslat());

            IEnumerator TimerBaslat()
            {
                var max = timer;
                while (timer > 0)
                {
                    EkipmanCoolDown.fillAmount = timer / max;
                    yield return new WaitForSeconds(0.5f);
                    timer -= 0.5f;
                }
            }
            EkipmanIkonuDegistir(oncekiEkipman);
        }
    }

    public void InteractionTextDegistir(string text) 
    {
        InteractionText.text = text;
    }
}

public enum EKIPMANLAR
{
    NONE,
    SCANNER,
    CROSS,
    COOLDOWN
}
