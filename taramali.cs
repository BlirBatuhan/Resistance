using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class taramali : MonoBehaviour
{
    [Header("AYARLAR")]
    float AtesEtmeSikligi_1;
    public float AtesEtmeSikligi_2 = .2f;
    public float menzil;
    int ToplamMermiSayısı = 100;
    int SarjorKapasitesi = 20;
    int KalanMermi;
    float DarbeGucu = 20;
    bool sesCalabilir = true;
    public TextMeshProUGUI ToplamMermitxt;
    public TextMeshProUGUI KalanMermitxt;
    [Header("SESLER")]
    public AudioSource[] Sesler;
    [Header("EFEKTLER")]
    public ParticleSystem[] Efektler;

    [Header("GENEL")]
    public Camera BenimKameram;
    public Animator KarakterinAnimatoru;

    void Start()
    {
        AtesEtmeSikligi_1 = Time.time;  
        KalanMermi = SarjorKapasitesi;
        ToplamMermitxt.text = ToplamMermiSayısı.ToString();
        KalanMermitxt.text = KalanMermi.ToString();
    }

   
    void Update()
    {
        if (Input.GetKey(KeyCode.R)) {
            
            if(KalanMermi<SarjorKapasitesi && ToplamMermiSayısı !=0)
            {
                KarakterinAnimatoru.Play("reload");
                if (!Sesler[2].isPlaying)                  
                    Sesler[2].Play();
                    
            }           
        }

        if (KarakterinAnimatoru.GetBool("Reload")) { ReloadTeknikİslemler();}

    void ReloadTeknikİslemler()
        {
            if (KalanMermi == 0)
            {
                if (ToplamMermiSayısı <= SarjorKapasitesi)
                {
                    KalanMermi = ToplamMermiSayısı;
                    ToplamMermiSayısı = 0;
                }
                else
                {
                    KalanMermi = SarjorKapasitesi;
                    ToplamMermiSayısı -= SarjorKapasitesi;
                }


            }
            else
            {
                if (ToplamMermiSayısı <= SarjorKapasitesi)
                {
                    int OlusanDeger = KalanMermi + ToplamMermiSayısı;

                    if (OlusanDeger > SarjorKapasitesi)
                    {
                        KalanMermi = SarjorKapasitesi;
                        ToplamMermiSayısı = OlusanDeger - SarjorKapasitesi;
                    }
                    else
                    {
                        KalanMermi += ToplamMermiSayısı;
                        ToplamMermiSayısı = 0;
                    }
                }
                else
                {
                    int MevcutDegisim = SarjorKapasitesi - KalanMermi;
                    ToplamMermiSayısı -= MevcutDegisim;
                    KalanMermi = SarjorKapasitesi;
                }
            }
            ToplamMermitxt.text = ToplamMermiSayısı.ToString();
            KalanMermitxt.text = KalanMermi.ToString();

            KarakterinAnimatoru.SetBool("Reload", false);
        }




        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(Time.time > AtesEtmeSikligi_1 && KalanMermi != 0)
            {
                AtesEt();
                AtesEtmeSikligi_1 = Time.time + AtesEtmeSikligi_2;
            }
            if (KalanMermi == 0 && sesCalabilir) {

                StartCoroutine(SesCooldown());
            }
            
        }

    void AtesEt()
        {
            KalanMermi--;
            KalanMermitxt.text = KalanMermi.ToString();
            Efektler[0].Play();
            Sesler[0].Play();
            KarakterinAnimatoru.Play("Egilerek_ates");

            RaycastHit hit;
            if(Physics.Raycast(BenimKameram.transform.position,BenimKameram.transform.forward,out hit, menzil))
            {
                if (hit.transform.gameObject.CompareTag("Dusman"))
                {
                    Dusman dusman = hit.transform.root.GetComponent<Dusman>();
                    Debug.Log("Düşmana çarptı!");
                    dusman.SaglikDurumu(DarbeGucu);
                    Instantiate(Efektler[2], hit.point, Quaternion.LookRotation(hit.normal));
                }
                else
                {
                    Instantiate(Efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

    }

    IEnumerator SesCooldown()
    {
        sesCalabilir = false;
        Sesler[1].Play();
        yield return new WaitForSeconds(1); // Cooldown süresi
        sesCalabilir = true;
    }
}
