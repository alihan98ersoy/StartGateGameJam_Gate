using System.Collections.Generic;
using UnityEngine;

public class Sira : MonoBehaviourSingleton<Sira>
{
    public List<Kurban> kurbanlikSirasi;

    public void SirayaEkle(Kurban kurban) 
    {
        kurbanlikSirasi.Add(kurban);
    }

    public void SiradanCıkar(Kurban kurban) 
    {
        bool kurbanCikti = false;
        for (int i = 0; i < kurbanlikSirasi.Count; i++)
        {
            if (!kurbanCikti)
            {
                if (kurbanlikSirasi[i] == kurban)
                {
                    kurbanCikti = true;
                    kurbanlikSirasi.RemoveAt(i);
                    kurban.KurbaniKurbanEt();
                }
            }
            else 
            {
                kurbanlikSirasi[i].Ilerle();
            }
        }
        if (!kurbanCikti) 
        {
            Debug.LogError("HATA KURBAN LiSTEDE YOK." + kurban);
        }

        if(kurbanlikSirasi.Count == 0) 
        {
            GameManager.Instance.SiraBitti();
        }
    }

    public void KurbanlikSirasiOlustur(List<Kurban> kurbanListesi) 
    {

    }
}
