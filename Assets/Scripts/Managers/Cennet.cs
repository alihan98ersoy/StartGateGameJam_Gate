using System.Collections.Generic;
using UnityEngine;

public class Cennet : MonoBehaviourSingleton<Cennet>
{
    public float cennetinYuzdesi;

    public List<Ruh> cennettekiler = new List<Ruh>();

    public Cennet() 
    {
        cennettekiler.Add(new Ruh(100));
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
        float total = 0;
        foreach (var item in cennettekiler) 
        {
            total += item.yuzdekaciyi;
        }

        cennetinYuzdesi = total / cennettekiler.Count;
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
