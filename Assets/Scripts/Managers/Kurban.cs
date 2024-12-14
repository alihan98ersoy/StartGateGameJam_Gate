using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kurban : MonoBehaviour
{
    float ilerleTimer = 1f;
    float ilerlemeMesafesi = 1f;
    Rigidbody rb;

    Animator animator;

    public bool bekle = false;
    public bool bidahaBekleme = false;
    public Ruh ruhu;
    private Vector3 targetPosition;
    private float stoppingDistance = 0.1f;
    private float forceStrength = 1f;

    bool isMouseOn = true;

    public void Initialize(Ruh ruhu)
    {
        this.ruhu = ruhu;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody> ();
        animator = GetComponent<Animator> ();
        StartCoroutine(SurekliIlerle());
    }
    void FixedUpdate()
    {
        Vector3 direction = targetPosition - rb.position;

        // Eğer hedefe yaklaştıysak kuvvet uygulamayı bırak
        if (direction.magnitude > stoppingDistance & !bekle || bidahaBekleme)
        {
            animator.SetBool("Waliking", true);
            Debug.Log("<!!> direction.normalized * forceStrength" + direction.normalized * forceStrength);
            rb.AddForce(direction.normalized * forceStrength);
        }
        else if (bekle) 
        {
            animator.SetBool("Waliking", false);
            rb.linearVelocity = Vector3.zero;
        }
    }

    IEnumerator SurekliIlerle() 
    {
        Debug.Log("<!!> SurekliIlerle");
        Ilerle();
        yield return new WaitForSeconds(ilerleTimer);
        StartCoroutine(SurekliIlerle());
    }

    public void Ilerle() 
    {
        Debug.Log("<!!> Ilerle");
        targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + ilerlemeMesafesi);
        //transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + ilerlemeMesafesi);
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

    public void KurbanTaramayiBaslat()
    {
        Debug.Log("KurbanTaramayiBaslat");
        GetComponent<KurbanTarama>().TaramayiBaslat();
    }

    public void KurbanTaramayiBitir()
    {
        Debug.Log("KurbanTaramayiBitir");
        if(this != null)
            GetComponent<KurbanTarama>().TaramayiBitir();
    }

    public override string ToString()
    {
        return "Kurban" + ruhu;
    }

    void OnMouseOver()
    {
        isMouseOn = true;
        GameManager.Instance.FareKurbanınÜzerineGeldi(this);
        Debug.Log("OnMouseOver: " + gameObject.name);
    }
    void OnMouseExit()
    {
        isMouseOn = false;
        StartCoroutine(Bekle(0.1f));

        IEnumerator Bekle(float sayi)
        {
            while (sayi > 0)
            {
                if (isMouseOn)
                    break;

                yield return new WaitForSeconds(0.01f);
                sayi -= 0.01f;
                if (sayi <= 0)
                    FareExitOlduKesin();
            }
        }

        void FareExitOlduKesin() 
        {
            GameManager.Instance.FareKurbaninUzerindenCikti();
            Debug.Log("OnMouseExit: " + gameObject.name);
        }
    }
}

public class Ruh 
{
    public string ismi;
    public string yuzdekaciyi;

    public bool isimgizlimi = true;
    public bool yuzdegizlimi = true;

    public bool isimTaraniyor = false;
    public bool yuzdeTaraniyor = false;

    public Ruh(string yuzdekaciyi, string ismi = "")
    {
        this.ismi = ismi;
        this.yuzdekaciyi = yuzdekaciyi;
    }

    public override string ToString()
    {
        return "Ruh" + ismi;
    }
}
