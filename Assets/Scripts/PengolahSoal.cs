using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PengolahSoal : MonoBehaviour
{
    public TextAsset asset;

    private string[] soal;

    private string[,] soalBag;

    int indexSoal;
    int maxSoal;
    bool ambilSoal;
    char kunciJ;

    //Komponen UI
    public Text txtSoal, txtOpsiA, txtOpsiB, txtOpsiC, txtOpsiD;

    bool isHasil;
    private float durasi;
    public float durasiPenilian;

    int jwbBenar, jwbSalah;
    float nilai;

    public GameObject panel;
    public GameObject imagePenilian, imageHasil;
    public Text txtHasil;

    // Start is called before the first frame update
    void Start()
    {
        durasi = durasiPenilian;

        soal = asset.ToString().Split('#');

        soalBag = new string[soal.Length, 6];
        maxSoal = soal.Length;

        OlahSoal();

        ambilSoal = true;
        TampilkanSoal();

        print(soalBag[1,3]);
    }

    private void OlahSoal()
    {
        for(int i = 0; i < soal.Length; i++)
        {
            string[] tempSoal = soal[i].Split('+');
            for(int j = 0; j < tempSoal.Length; j++)
            {
                soalBag[i, j] = tempSoal[j];
                continue;
            }
            continue;
        }
    }

    private void TampilkanSoal()
    {
        if(indexSoal < maxSoal)
        {
            if (ambilSoal)
            {
                txtSoal.text = soalBag[indexSoal, 0];
                txtOpsiA.text = soalBag[indexSoal, 1];
                txtOpsiB.text = soalBag[indexSoal, 2];
                txtOpsiC.text = soalBag[indexSoal, 3];
                txtOpsiD.text = soalBag[indexSoal, 4];
                kunciJ = soalBag[indexSoal, 5][0];

                ambilSoal = false;
            }
        }
    }

    public void Opsi(string opsiHuruf)
    {
        CheckJawaban(opsiHuruf[0]);

        if (indexSoal == maxSoal - 1)
        {
            isHasil = true;
        }
        else
        {
            indexSoal++;
            ambilSoal = true;
        }

        panel.SetActive(true);
        
    }

    private float HitungNilai()
    {
        return nilai = (float)jwbBenar / maxSoal * 100;
    }

    public Text txtPenilian;

    private void CheckJawaban(char huruf)
    {
        string penilian;

        if (huruf.Equals(kunciJ))
        {
            penilian = "Excellent";
            jwbBenar++;
        }
        else
        {
            penilian = "Owww!";
            jwbSalah++;
        }

        txtPenilian.text = penilian;
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.activeSelf)
        {
            durasiPenilian -= Time.deltaTime;

            if (isHasil)
            {
                imagePenilian.SetActive(true);
                imageHasil.SetActive(false);

                if (durasiPenilian <= 0)
                {
                    txtHasil.text = "Correct : " + jwbBenar + "\nWrong : " + jwbSalah + "\n\nResults : " + HitungNilai();

                    imagePenilian.SetActive(false);
                    imageHasil.SetActive(true);

                    durasiPenilian = 0;
                }
            }
            else
            {
                imagePenilian.SetActive(true);
                imageHasil.SetActive(false);

                if (durasiPenilian <= 0)
                {
                    panel.SetActive(false);
                    durasiPenilian = durasi;

                    TampilkanSoal();
                }
            }
        }
    }
}
