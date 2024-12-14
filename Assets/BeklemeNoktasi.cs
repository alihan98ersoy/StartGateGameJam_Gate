using UnityEngine;

public class BeklemeNoktasi : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Kurban>() != null) 
        {
            Sira.Instance.SuruKapiyaVardiBeklet();
        }
    }
}
