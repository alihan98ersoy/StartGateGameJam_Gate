using System.Collections.Generic;
using UnityEngine;

public class KurbanTarama : MonoBehaviour
{
    public List<GameObject> kurbaninParcalari;
    List<Material> kurbaninParcalarininMateriellari = new List<Material>();

    public Material taramaMaterial;

    public void TaramayiBaslat()
    {
        foreach (var item in kurbaninParcalari) 
        {
            kurbaninParcalarininMateriellari.Add(item.GetComponent<Renderer>().material);
            item.GetComponent<Renderer>().material = taramaMaterial;
        }
    }

    public void TaramayiBitir() 
    {
        foreach (var item in kurbaninParcalari)
        {
            item.GetComponent<Renderer>().material = kurbaninParcalarininMateriellari[0];
            kurbaninParcalarininMateriellari.RemoveAt(0);
        }
    }
}
