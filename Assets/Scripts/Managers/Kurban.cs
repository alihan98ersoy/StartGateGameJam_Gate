using System.Collections;
using UnityEngine;

public class Kurban : MonoBehaviour
{
    float ilerleTimer = 1f;
    float ilerlemeMesafesi = 1f;

    public Ruh ruhu;

    public void Initialize(Ruh ruhu)
    {
        this.ruhu = ruhu;
    }

    private void Start()
    {
        StartCoroutine(SurekliIlerle());
    }

    IEnumerator SurekliIlerle() 
    {
        Ilerle();
        yield return new WaitForSeconds(ilerleTimer);
        StartCoroutine(SurekliIlerle());
    }

    public void Ilerle() 
    {
        transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + ilerlemeMesafesi);
    }

    public void KurbaniKurbanEt() 
    {
        Debug.Log("Kurban artık yok");
        Destroy(gameObject);
    }

    public void KurbanArtikMubarek() 
    {
        Debug.Log("Kurban artık mubarek");
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return "Kurban" + ruhu;
    }
}

public class Ruh 
{
    public string ismi;
    public float yuzdekaciyi;

    public Ruh(float yuzdekaciyi, string ismi = "")
    {
        this.ismi = ismi;
        this.yuzdekaciyi = yuzdekaciyi;
    }

    public override string ToString()
    {
        return "Ruh" + ismi;
    }
}
