using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dusman : MonoBehaviour
{
    [Header("DÝÐER AYARLAR")]
    NavMeshAgent navmesh;
    Animator animatorum;
    GameObject Hedef;
    public GameObject Anahedef;

    [Header("GENEL AYARLAR")]
    public float AtesEtmeMenzilDeger;
    public float SuphelenmeMenzilDeger;
    Vector3 baslangicNoktasi;
    bool Suphelenme = false;
    bool AtesEdiliyormu = false;
    public GameObject AtesEtmeNoktasi;

    [Header("DEVRÝYE AYARLARI")]
    public GameObject[] DevriyeNoktalari_1;
    public GameObject[] DevriyeNoktalari_2;
    public GameObject[] DevriyeNoktalari_3;
    public GameObject[] DevriyeNoktalari_4;
    GameObject[] AktifOlanNoktaListeleri;

    [Header("SÝLAH AYARLAR")]
    float AtesEtmeSikligi_1;
    public float AtesEtmeSikligi_2;
    public float menzil;
    public float DarbeGucu;

    [Header("SESLER")]
    public AudioSource[] Sesler;
    [Header("EFEKTLER")]
    public ParticleSystem[] Efektler;


    bool DevriyeVarmi;
    Coroutine DevriyeAt;
    Coroutine DevriyeZaman;
    bool DevriyeKilit;
    public bool DevriyeAtabilirmi;
    float Saglik;


    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        animatorum = GetComponent<Animator>();
        baslangicNoktasi = transform.position;
        DevriyeZaman = StartCoroutine(DevriyeZamanKontrol());
        Saglik = 100;
    }

    GameObject[] DevriyeKontrol()
    {

        int deger = Random.Range(1, 5);
        switch (deger)
        {
            case 1:
                AktifOlanNoktaListeleri = DevriyeNoktalari_1;
                break;
            case 2:
                AktifOlanNoktaListeleri = DevriyeNoktalari_2;
                break;
            case 3:
                AktifOlanNoktaListeleri = DevriyeNoktalari_3;
                break;
            case 4:
                AktifOlanNoktaListeleri = DevriyeNoktalari_4;
                break;

        }

        return AktifOlanNoktaListeleri;


    }
    IEnumerator DevriyeZamanKontrol()
    {

        while (true && !DevriyeVarmi && DevriyeAtabilirmi)
        {

            yield return new WaitForSeconds(5f);

            DevriyeKilit = true;
            if (DevriyeZaman != null)
            {
                StopCoroutine(DevriyeZaman);
                DevriyeZaman = null;
            }


        }
    }
    IEnumerator DevriyeTeknikÝslem(GameObject[] GelenObjeler)
    {
        navmesh.isStopped = false;
        DevriyeKilit = false;
        DevriyeVarmi = true;
        animatorum.SetBool("devriye", true);
        int toplamnokta = GelenObjeler.Length - 1;
        int baslangicdeger = 0;
        navmesh.SetDestination(GelenObjeler[baslangicdeger].transform.position);

        while (true && DevriyeAtabilirmi)
        {

            if (Vector3.Distance(transform.position, GelenObjeler[baslangicdeger].transform.position) <= 2f)
            {

                Debug.Log("Þu anki devriye noktasý: " + baslangicdeger);
                if (toplamnokta > baslangicdeger)
                {

                    ++baslangicdeger;
                    navmesh.SetDestination(GelenObjeler[baslangicdeger].transform.position);

                    Debug.Log("Þu anki devriye noktasý: " + baslangicdeger);
                }
                else
                {
                    navmesh.stoppingDistance = 1;
                    Debug.Log("Baþlangýc Noktasýna dönülüyor " + baslangicdeger);
                    navmesh.SetDestination(baslangicNoktasi);

                }


            }
            else
            {

                if (toplamnokta > baslangicdeger)
                {
                    navmesh.SetDestination(GelenObjeler[baslangicdeger].transform.position);

                }

            }

            yield return null;

        }


    }
    private void LateUpdate()
    {
        AtesEtmeMenzil();
        if (!AtesEdiliyormu)
        {
            SuphelenmeMenzil();
        }

        if (navmesh.stoppingDistance == 1 && navmesh.remainingDistance <= 1)
        {
            animatorum.SetBool("devriye", false);
            transform.rotation = Quaternion.Euler(0, 180, 0);

            if (DevriyeAtabilirmi)
            {
                DevriyeVarmi = false;
                DevriyeZaman = StartCoroutine(DevriyeZamanKontrol());
                StopCoroutine(DevriyeAt);
            }

            navmesh.stoppingDistance = 0;
            navmesh.isStopped = true;

        }


        if (DevriyeKilit && DevriyeAtabilirmi)
        {
            Debug.Log("Devriye baþlatýlýyor..."); // Test için eklendi

            DevriyeAt = StartCoroutine(DevriyeTeknikÝslem(DevriyeKontrol()));


        }
    }

    void AtesEtmeMenzil()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AtesEtmeMenzilDeger);
        bool hedefBulundu = false;

        foreach (var objeler in hitColliders)
        {
            if (objeler.gameObject.CompareTag("Player"))
            {
                hedefBulundu = true;
                Debug.Log("Hedef ateþ etme menzilinde!"); // Test için

                navmesh.isStopped = true;
                navmesh.velocity = Vector3.zero;

                AtesEt(objeler.gameObject);
                break; // Oyuncuyu bulduk, döngüden çýkalým.
            }
        }

        if (!hedefBulundu && AtesEdiliyormu)
        {
            animatorum.SetBool("atesEtme", false);
            navmesh.isStopped = false;
            animatorum.SetBool("yuru", true);
            AtesEdiliyormu = false;
        }

    }

    void AtesEt(GameObject Hedef)
    {
        AtesEdiliyormu = true;
        Vector3 aradakifark = Hedef.gameObject.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(aradakifark, Vector3.up);
        transform.rotation = rotation;
        animatorum.SetBool("yuru", false);
        animatorum.SetBool("devriye", false);
        animatorum.SetBool("atesEtme", true);

        navmesh.isStopped = true;
        navmesh.velocity = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(AtesEtmeNoktasi.transform.position, AtesEtmeNoktasi.transform.forward, out hit, menzil))
        {
            Color color = Color.blue;
            Vector3 degisenpozisyon = new Vector3(Hedef.transform.position.x, Hedef.transform.position.y + 1.5f, Hedef.transform.position.z);
            Debug.DrawLine(AtesEtmeNoktasi.transform.position, degisenpozisyon, color);

            if (Time.time > AtesEtmeSikligi_1)
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    hit.transform.GetComponent<KarakterKontrol>().SaglýkDurumu(DarbeGucu);
                    Instantiate(Efektler[1], hit.point, Quaternion.LookRotation(hit.normal));

                }
                else
                {
                    Instantiate(Efektler[2], hit.point, Quaternion.LookRotation(hit.normal));
                }


                if (!Sesler[0].isPlaying)
                {
                    Sesler[0].Play();
                    Efektler[0].Play();
                }

                AtesEtmeSikligi_1 = Time.time + AtesEtmeSikligi_2;


            }



        }
    }

    void SuphelenmeMenzil()
    {
        if (AtesEdiliyormu) return;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SuphelenmeMenzilDeger);

        bool oyuncuBulundu = false;

        foreach (var objeler in hitColliders)
        {
            if (objeler.gameObject.CompareTag("Player"))
            {
                oyuncuBulundu = true;
                Debug.Log("Þüphelendi! Hedef: " + objeler.gameObject.name);

                if (animatorum.GetBool("alarm"))
                {
                    animatorum.SetBool("alarm", false);
                    animatorum.SetBool("yuru", true);
                }
                else if (animatorum.GetBool("devriye"))
                {
                    animatorum.SetBool("devriye", false);
                    animatorum.SetBool("yuru", true);
                }
                else
                {
                    animatorum.SetBool("yuru", true);
                }

                Hedef = objeler.gameObject;
                navmesh.isStopped = false;
                navmesh.SetDestination(Hedef.transform.position);
                Suphelenme = true;

                if (DevriyeAtabilirmi)
                {
                    if (DevriyeAt != null)
                    {
                        StopCoroutine(DevriyeAt);
                        Debug.Log("Devriye durduruldu!");
                    }
                }
                break;  // Oyuncuyu bulduðumuz anda döngüden çýkalým
            }
        }

        // Eðer hiç oyuncu bulunamazsa, devriye mantýðýný uygula
        if (!oyuncuBulundu)
        {
            if (Suphelenme)
            {
                Hedef = null;

                if (transform.position != baslangicNoktasi)
                {
                    navmesh.stoppingDistance = 1;
                    navmesh.SetDestination(baslangicNoktasi);

                    animatorum.SetBool("devriye", true);

                    if (navmesh.remainingDistance <= 1)
                    {
                        animatorum.SetBool("yuru", false);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                }

                Suphelenme = false;

                if (DevriyeAtabilirmi)
                {
                    DevriyeAt = StartCoroutine(DevriyeTeknikÝslem(DevriyeKontrol()));
                    Debug.Log("Devriye yeniden baþlatýldý!");
                }
            }
            // **Düþman artýk þüphelenmiyorsa "yuru" parametresini kapat**
            animatorum.SetBool("yuru", false);
            
        }

    }


    public void SaglikDurumu(float Darbegucu)
    {
        Saglik -= Darbegucu;
        Debug.Log("Düþmanýn saðlýk durumu: " + Saglik);

        if (DevriyeAt != null)
        {
            StopCoroutine(DevriyeAt);
            DevriyeAt = null;
            Debug.Log("Devriye durduruldu!"); // Test amaçlý
        }
        if (!Suphelenme)
        {
            animatorum.SetBool("alarm", true);
            navmesh.stoppingDistance = 1.5f;
            navmesh.SetDestination(Anahedef.transform.position);
        }
        if (Saglik <= 0)
        {

            animatorum.Play("olme");
            Destroy(gameObject, 3f);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AtesEtmeMenzilDeger);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SuphelenmeMenzilDeger);

    }


}
/* RaycastHit hit;

       if(Physics.Raycast(Kafa.transform.position,Kafa.transform.forward, out hit, 10f))
       {
           if (hit.transform.gameObject.CompareTag("Player"))
           {

               Debug.Log("Çarptý");
           }

       }

       Debug.DrawRay(Kafa.transform.position, Kafa.transform.forward * 10f, Color.blue);*/