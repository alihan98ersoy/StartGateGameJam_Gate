using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagers : MonoBehaviourSingleton<UIManagers>
{
    public Sprite scannerSprite, crossSprite;

    public Image EkipmanImageUI, EkipmanCoolDown;

    public Text InteractionText, ruhIsimText, ruhYuzdeText, karmaText, karmaText2;

    public GameObject ruhBilgiPanel;

    public GameObject CompletePanel;
    public Text completeTitleText, completeButtonText;


    public float scannerCoolDown = 10f;
    public float crossCooldown = 5f;

    [HideInInspector]
    public EKIPMANLAR suankiEkipman = EKIPMANLAR.SCANNER;

    public void EkipmanIkonuDegistir(EKIPMANLAR enu) 
    {
        Debug.Log("<!!> EkipmanIkonuDegistir enu:"+enu);
        if (enu == EKIPMANLAR.SCANNER) 
        {
            suankiEkipman = enu;
            EkipmanImageUI.sprite = scannerSprite;
            EkipmanCoolDown.sprite = scannerSprite;

            if (ruhBilgiPanel.activeSelf)
                InteractionTextGuncelle();

        }
        else if (enu == EKIPMANLAR.CROSS)
        {
            suankiEkipman = enu;
            EkipmanImageUI.sprite = crossSprite;
            EkipmanCoolDown.sprite = crossSprite;
            if (ruhBilgiPanel.activeSelf)
                InteractionTextGuncelle();
        }
        else if (enu == EKIPMANLAR.COOLDOWN)
        {
            //Debug.Log("<!!> enu == EKIPMANLAR.COOLDOWN" + (suankiEkipman == EKIPMANLAR.SCANNER) + scannerCoolDown + ((suankiEkipman == EKIPMANLAR.SCANNER)? scannerCoolDown : crossCooldown));
            var timer = (suankiEkipman == EKIPMANLAR.SCANNER) ? scannerCoolDown : crossCooldown;
            var oncekiEkipman = suankiEkipman;
            suankiEkipman = enu;

            if (ruhBilgiPanel.activeSelf)
                InteractionTextGuncelle();

            StartCoroutine(TimerBaslat());

            IEnumerator TimerBaslat()
            {
                //Debug.Log("<!!> TimerBaslat:" + timer);
                var max = timer;
                while (timer > 0)
                {
                    //Debug.Log("<!!> cooldown fill:" + timer / max);
                    EkipmanCoolDown.fillAmount = timer / max;
                    yield return new WaitForSeconds(0.1f);
                    timer -= 0.1f;
                }
                EkipmanIkonuDegistir(oncekiEkipman);
            }
        }
    }

    public void InteractionTextGuncelle() 
    {
        if (suankiEkipman == EKIPMANLAR.SCANNER)
        {
            InteractionTextDegistir("Kurbaný Taramak için E ye bas.");
        }
        else if (suankiEkipman == EKIPMANLAR.CROSS)
        {
            InteractionTextDegistir("Kurban Etmek için E ye bas.");
        }
        else if (suankiEkipman == EKIPMANLAR.COOLDOWN)
        {
            InteractionTextDegistir("Soðuyor");
        }
    }

    public void InteractionTextDegistir(string text) 
    {
        InteractionText.text = text;
    }

    public void RuhBilgisiGoster(Ruh ruh) 
    {
        ruhBilgiPanel.SetActive(true);
        ruhIsimText.text = ruh.isimTaraniyor? "Taranýyor.." :  "Ýsim: " + ((ruh.isimgizlimi) ? "?" : ruh.ismi);
        ruhYuzdeText.text = ruh.yuzdeTaraniyor ? "Taranýyor.." : "Karma: " + ((ruh.yuzdegizlimi) ? "?" : ruh.yuzdekaciyi);
        ruhYuzdeText.color = ruh.yuzdegizlimi ? Color.white : (int.Parse(ruh.yuzdekaciyi) < GameManager.Instance.KarmaSinir) ? Color.red : Color.green;
    }

    public void RuhBilgisiGizle() 
    {
        ruhBilgiPanel.SetActive(false);
    }

    public void WaveButtonClicked() 
    {
        if(completeButtonText.text == "Yeniden Baþla") 
        {
            SceneManager.LoadScene(0);
            /*Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cennet.Instance.CennetReset();
            GameManager.Instance.WaveBaslat(0);
            CompletePanel.SetActive(false);*/
        }
        else if (completeButtonText.text == "Yeni Wave")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.Instance.WaveBaslat(GameManager.Instance.Wave + 1);
            CompletePanel.SetActive(false);
        }
    }


    internal void Gameover()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        CompletePanel.SetActive(true);
        completeTitleText.text = "GAME OVER";
        completeTitleText.color = Color.red;
        completeButtonText.text = "Yeniden Baþla";
    }

    internal void WaveComplete(int wave)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        CompletePanel.SetActive(true);
        completeTitleText.text = "TEBRÝKLER";
        completeTitleText.color = Color.green;
        completeButtonText.text = "Yeni Wave";
    }
}

public enum EKIPMANLAR
{
    NONE,
    SCANNER,
    CROSS,
    COOLDOWN
}
