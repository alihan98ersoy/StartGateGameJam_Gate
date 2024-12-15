using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public int KarmaSinir = 60;
    public GameObject kurbanPrefab;
    public Transform spawnPoint;

    public float spawnSuresi = 1f;

    public Kurban suankiKurban = null;

    int Wave = 0;
    public float kapidaBekle = 6f;

    private void Start()
    {
        Cennet.Instance.CenneteEkle(new Ruh("100"));
        WaveBaslat(Wave);
    }

    public void YeniKurbanOlustur(Ruh ruh) 
    {
        // Yeni kurban GameObject oluþtur
        GameObject kurbanObject = Instantiate(kurbanPrefab, spawnPoint.position, Quaternion.identity);

        // Kurban'ý baþlat
        Kurban newKurban = kurbanObject.GetComponent<Kurban>();
        if (newKurban != null)
        {
            newKurban.Initialize(ruh);
            Sira.Instance.SirayaEkle(newKurban);
            Debug.Log($"{name} sýraya eklendi.");
        }
        else
        {
            Debug.LogError("Kurban bileþeni eksik!");
        }
    }

    IEnumerator SurekliOlustur(int wave)
    {
        foreach(var item in Waves.GetWave(wave)) 
        {
            YeniKurbanOlustur(item);
            yield return new WaitForSeconds(spawnSuresi);
        }
    }

    Ruh RandomRuhOlustur() 
    {
        int a = UnityEngine.Random.Range(0, 101);
        return new Ruh(a.ToString(), "ismi"+a );
    }

    public void FareKurbanýnÜzerineGeldi(Kurban kurban) 
    {
        if (suankiKurban != null)
            return;
        Cursor.visible = true;
        suankiKurban = kurban;
        UIManagers.Instance.RuhBilgisiGoster(kurban.ruhu);

        if (UIManagers.Instance.suankiEkipman == EKIPMANLAR.SCANNER) 
        {
            UIManagers.Instance.InteractionTextDegistir("Kurbaný Taramak için E ye bas.");
        }
        else if (UIManagers.Instance.suankiEkipman == EKIPMANLAR.CROSS)
        {
            UIManagers.Instance.InteractionTextDegistir("Kurban Etmek için E ye bas.");
        }
        else if (UIManagers.Instance.suankiEkipman == EKIPMANLAR.COOLDOWN)
        {
            UIManagers.Instance.InteractionTextDegistir("Soðuyor");
        }
    }

    public void FareKurbaninUzerindenCikti() 
    {
        suankiKurban = null;
        UIManagers.Instance.InteractionTextDegistir("");
        UIManagers.Instance.RuhBilgisiGizle();
    }

    internal void KurbaniTara(Kurban suankiKurbann)
    {
        UIManagers.Instance.EkipmanIkonuDegistir(EKIPMANLAR.COOLDOWN);

        StartCoroutine(TaramayiBaslat(UIManagers.Instance.scannerCoolDown));

        IEnumerator TaramayiBaslat(float sure)
        {
            suankiKurbann.KurbanTaramayiBaslat();

            if (suankiKurbann == suankiKurban)
                UIManagers.Instance.ruhIsimText.text = "Taranýyor..";
            suankiKurbann.ruhu.isimTaraniyor = true;
            yield return new WaitForSeconds(1f);
            suankiKurbann.ruhu.isimgizlimi = false;
            suankiKurbann.ruhu.isimTaraniyor = false;
            Debug.Log("<!!> TaramayiBaslat suankiKurbann == suankiKurban" + (suankiKurbann == suankiKurban));
            if(suankiKurbann == suankiKurban) 
            {
                UIManagers.Instance.ruhIsimText.text = suankiKurbann.ruhu.ismi;
                UIManagers.Instance.ruhYuzdeText.text = "Taranýyor..";
            }

            suankiKurbann.ruhu.yuzdeTaraniyor = true;
            yield return new WaitForSeconds(sure - 1f);
            suankiKurbann.ruhu.yuzdegizlimi = false;
            suankiKurbann.ruhu.yuzdeTaraniyor = false;
            if (suankiKurbann == suankiKurban)
                UIManagers.Instance.RuhBilgisiGoster(suankiKurbann.ruhu);

            suankiKurbann.KurbanTaramayiBitir();
            //UIManagers.Instance.EkipmanIkonuDegistir(EKIPMANLAR.SCANNER); UI manager yapýyor bunu
        }
    }

    internal void KurbanEt(Kurban suankiKurban)
    {
        UIManagers.Instance.EkipmanIkonuDegistir(EKIPMANLAR.COOLDOWN);
        suankiKurban.KurbaniKurbanEt();
        Sira.Instance.SiradanCýkar(suankiKurban);
        //suankiKurban.KurbaniKurbanEt();
    }

    public void WaveBaslat(int wave) 
    {
        Wave = wave;
        StartCoroutine(SurekliOlustur(Wave));
    }

    internal void SiraBitti()
    {
        UIManagers.Instance.WaveComplete(Wave);
    }

    internal void GameOver()
    {
        UIManagers.Instance.Gameover();
    }
}

public static class Waves 
{

    public static List<Ruh> GetWave(int wave) 
    {
        switch (wave) 
        {
            case 0: return new List<Ruh>()
        {
            new Ruh("86", "Joseph"),
            new Ruh("79", "James"),
            new Ruh("68", "Kim"),
            new Ruh("33", "Stephan"),
            new Ruh("93", "josh"),
        };
            case 1:
                return new List<Ruh>()
        {
            new Ruh("86", "Joseph"),
            new Ruh("79", "James"),
            new Ruh("68", "Kim"),
            new Ruh("33", "Stephan"),
            new Ruh("93", "josh"),
        };
            default: return new List<Ruh>()
        {
            new Ruh("86", "Joseph"),
            new Ruh("79", "James"),
            new Ruh("68", "Kim"),
            new Ruh("33", "Stephan"),
            new Ruh("93", "josh"),
        };


        }
    }
}
