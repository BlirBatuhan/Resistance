using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K�t�phanem
{
    public class Animasyon
    {
        public void Sol_Hareket(Animator anim, bool isRunning, string AnaParatme,
             List<float> ParametreDegerleri)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (isRunning) // E�er Shift ile birlikte W+A bas�l�ysa
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[1]);
                }
                else if (Input.GetKey(KeyCode.W)) // Sadece W+A ise
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[2]);
                }
                else if (Input.GetKey(KeyCode.S)) // Geri giderken sola bas�l�ysa
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[3]);
                }
                else // Sadece sola hareket
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[0]);
                }
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetFloat(AnaParatme, 0);
            }
        }

        public void Sag_Hareket(Animator anim, bool isRunning, string AnaParatme,
             List<float> ParametreDegerleri)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (isRunning) // E�er Shift ile birlikte W+A bas�l�ysa
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[1]);
                }
                else if (Input.GetKey(KeyCode.W)) // Sadece W+A ise
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[2]);
                }
                else if (Input.GetKey(KeyCode.S)) // Geri giderken sola bas�l�ysa
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[3]);
                }
                else // Sadece sola hareket
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[0]);
                }
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetFloat(AnaParatme, 0);
            }
        }

        public void Geri_Hareket(Animator anim, string AnaParatme)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                anim.SetFloat("speed", 0);
                anim.SetBool(AnaParatme, true);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool(AnaParatme, false);
            }
        }

        public void Egilme_Hareket(Animator anim, string AnaParatme,
             List<float> ParametreDegerleri, ref float currentSpeed)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                currentSpeed *= 0.5f;
                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[1]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[2]);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[3]);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[4]);
                }
                else
                {
                    anim.SetFloat(AnaParatme, ParametreDegerleri[0]);
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                currentSpeed *= 1f;
                anim.SetFloat(AnaParatme, 0);
            }
        }

        public List<float> ParamtereOlustur(float [] parametre)
        {
            List<float> Sol_Yon_Parametreleri = new List<float>();
            foreach (float item in parametre)
            {
                Sol_Yon_Parametreleri.Add(item);
            }
            return Sol_Yon_Parametreleri;
        }
    }
}

