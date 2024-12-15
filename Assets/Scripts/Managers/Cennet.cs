using System.Collections.Generic;
using UnityEngine;

public class Cennet : MonoBehaviourSingleton<Cennet>
{
    public float cennetinYuzdesi;

    public List<Ruh> cennettekiler = new List<Ruh>();

    public Cennet() 
    {
        cennettekiler.Add(new Ruh("100"));
    }
    public void CenneteEkle(Kurban kurban) 
    {
        cennettekiler.Add(kurban.ruhu);
        kurban.KurbanArtikMubarek();
        Sira.Instance.SiradanCıkar(kurban);

        CennetinYuzdesiHesapla();
    }

    public void CenneteEkle(Ruh ruh) 
    {
        cennettekiler.Add(ruh);
        CennetinYuzdesiHesapla();
    }

    private void CennetinYuzdesiHesapla()
    {
        int total = 0;
        foreach (var item in cennettekiler) 
        {
            total += int.Parse(item.yuzdekaciyi);
        }

        cennetinYuzdesi = total / cennettekiler.Count;
        UIManagers.Instance.karmaText.text = "%" + cennetinYuzdesi.ToString("0");
        UIManagers.Instance.karmaText2.text = "%" + cennetinYuzdesi.ToString("0");
        if (cennetinYuzdesi < 60) 
        {
            GameManager.Instance.GameOver();
        }
    }

    public void CennetReset()
    {
        cennettekiler.Clear();
        cennettekiler.Add(new Ruh("100"));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(" OnTriggerEnter collision" + other);
        if (other.GetComponent<Kurban>() != null)
        {
            CenneteEkle(other.GetComponent<Kurban>());
        }
    }
}
