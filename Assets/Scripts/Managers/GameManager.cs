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

    private void Start()
    {
        Cennet.Instance.CenneteEkle(new Ruh("100"));
        StartCoroutine(SurekliOlustur(5));
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

    IEnumerator SurekliOlustur(int sayi)
    {
        while (sayi > 0) 
        {
            YeniKurbanOlustur(RandomRuhOlustur());
            yield return new WaitForSeconds(spawnSuresi);
            sayi--;
        }
    }

    Ruh RandomRuhOlustur() 
    {
        int a = UnityEngine.Random.Range(0, 101);
        return new Ruh(a.ToString(), "ismi"+a );
    }

    internal void SiraBitti()
    {
        //throw new NotImplementedException();
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
            if (suankiKurbann == suankiKurban)
                UIManagers.Instance.ruhIsimText.text = "Taranýyor..";
            suankiKurbann.ruhu.isimTaraniyor = true;
            yield return new WaitForSeconds(3f);
            suankiKurbann.ruhu.isimgizlimi = false;
            suankiKurbann.ruhu.isimTaraniyor = false;
            Debug.Log("<!!> TaramayiBaslat suankiKurbann == suankiKurban" + (suankiKurbann == suankiKurban));
            if(suankiKurbann == suankiKurban) 
            {
                UIManagers.Instance.ruhIsimText.text = suankiKurbann.ruhu.ismi;
                UIManagers.Instance.ruhYuzdeText.text = "Taranýyor..";
            }

            suankiKurbann.ruhu.yuzdeTaraniyor = true;
            yield return new WaitForSeconds(sure - 3f);
            suankiKurbann.ruhu.yuzdegizlimi = false;
            suankiKurbann.ruhu.yuzdeTaraniyor = false;
            if (suankiKurbann == suankiKurban)
                UIManagers.Instance.RuhBilgisiGoster(suankiKurbann.ruhu);

            //UIManagers.Instance.EkipmanIkonuDegistir(EKIPMANLAR.SCANNER); UI manager yapýyor bunu
        }
    }
}

/*public class GecmisYasami
{
    public string ismi;
    public string meslegi;
    public string kacCM;
    public string kendisiHakkindaBilgi;
    public string olduguYasi;
    public string olumsebebi;
    //evli bekar mý falan burada

    public override string ToString()
    {
        return "KurbaninGecmisi:" +  ismi + "-" + meslegi + "-" + kacCM + "-" + kendisiHakkindaBilgi + "-" + olduguYasi + "-" + olumsebebi;
    }
}*/
