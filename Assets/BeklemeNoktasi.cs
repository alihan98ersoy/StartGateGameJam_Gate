using UnityEngine;

public class BeklemeNoktasi : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Kurban>() != null) 
        {
            if(other.GetComponent<Kurban>().bidahaBekleme == false)
                Sira.Instance.SuruKapiyaVardiBeklet(other.GetComponent<Kurban>());
        }
    }
}
