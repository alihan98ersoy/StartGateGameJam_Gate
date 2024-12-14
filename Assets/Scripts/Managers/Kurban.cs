﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kurban : MonoBehaviour
{
    float ilerleTimer = 1f;
    float ilerlemeMesafesi = 1f;
    Rigidbody rb;


    public bool bekle = false;
    public bool bidahaBekleme = false;
    public Ruh ruhu;
    private Vector3 targetPosition;
    private float stoppingDistance = 0.1f;
    private float forceStrength = 1f;

    public void Initialize(Ruh ruhu)
    {
        this.ruhu = ruhu;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody> ();
        StartCoroutine(SurekliIlerle());
    }
    void FixedUpdate()
    {
        Vector3 direction = targetPosition - rb.position;

        // Eğer hedefe yaklaştıysak kuvvet uygulamayı bırak
        if (direction.magnitude > stoppingDistance & !bekle || bidahaBekleme)
        {
            rb.AddForce(direction.normalized * forceStrength);
        }
        else if (bekle) 
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    IEnumerator SurekliIlerle() 
    {
        Ilerle();
        yield return new WaitForSeconds(ilerleTimer);
        StartCoroutine(SurekliIlerle());
    }

    public void Ilerle() 
    {
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

    public override string ToString()
    {
        return "Kurban" + ruhu;
    }

    void OnMouseOver()
    {
        GameManager.Instance.FareKurbanınÜzerineGeldi(this);
        Debug.Log("OnMouseOver: " + gameObject.name);
    }
    void OnMouseExit()
    {
        GameManager.Instance.FareKurbaninUzerindenCikti();
        Debug.Log("OnMouseExit: " + gameObject.name);
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
