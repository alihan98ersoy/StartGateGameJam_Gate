using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public GameObject kurbanPrefab;
    public Transform spawnPoint;

    public float spawnSuresi = 1f;

    public Kurban suankiKurban = null;

    private void Start()
    {
        Cennet.Instance.CenneteEkle(new Ruh(100));

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
        float a = UnityEngine.Random.Range(0, 101);
        return new Ruh(a, "ismi"+a );
    }

    internal void SiraBitti()
    {
        //throw new NotImplementedException();
    }

    public void FareKurbanýnÜzerineGeldi(Kurban kurban) 
    {
        if (suankiKurban != null)
            return;

        suankiKurban = kurban;
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
            UIManagers.Instance.InteractionTextDegistir("");
        }
    }

    public void FareKurbaninUzerindenCikti() 
    {
        suankiKurban = null;
        UIManagers.Instance.InteractionTextDegistir("");
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
