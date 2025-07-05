using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDragandDrop : MonoBehaviour
{
    public AudioSFX audioSFX;

    [System.Serializable]
    public class KumpulanSoal
    {
        [System.Serializable]
        public class ElementSoal
        {
            public Sprite spriteSoal;

            public Sprite[] spriteJawabans;

            public int kunciJawaban;
        }

        public ElementSoal elementSoal;
    }

    public KumpulanSoal[] kumpulanSoals;

    public int soalKe;
    public int totalSoal; //total soal yang digunakan
    public int[] indexRandomSoal;
    public int[] indexRandomJawaban;
    public int kunciJawabanSekarang;

    //UI soal
    [Header("ui soal")]
    public Image imageSoal;
    public Image[] imageJawabans;

    public GameObject[] jawabans;
    Vector2[] FirstPositionJawaban;
    public GameObject dropJawaban;
    public float jarakDrop;

    [Space]
    public GameObject panelHasilJawaban;
    public Image imageCharacter;
    public Sprite[] spriteCharacter;
    public Text textHasilJawaban;
    public float timeAktifPanelHasilJawabn;

    [Space]
    public GameObject panelEndGame;

    [Space]
    public Text textScore;
    public int totalScore;
    public int increaseScore, decreaseScore;
    public Text textScoreEndGame;

    // Start is called before the first frame update
    void Start()
    {
        SaveFirstPositionJawaban();

        CreateIndexRandom();
        GenerateSoal();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClosePanelHasilJawabn()
    {
        panelHasilJawaban.SetActive(false);

        if (soalKe < totalSoal -1)
        {
            GenerateNextSoal();
        }
        else
        {
            panelEndGame.SetActive(true);

            textScoreEndGame.text = totalScore.ToString();
        }
        
        
    }

    void GenerateNextSoal()
    {
        for (int i = 0; i < jawabans.Length; i++)
        {
            jawabans[i].transform.localPosition = FirstPositionJawaban[i];
        }

        soalKe += 1;

        GenerateSoal();
        
    }

    void GenerateSoal()
    {
        

        imageSoal.sprite = kumpulanSoals[indexRandomSoal[soalKe]].elementSoal.spriteSoal; //mengganti sprite soal dari kumpulan soal

        for (int i = 0; i < imageJawabans.Length; i++)
        {
            imageJawabans[i].sprite = kumpulanSoals[indexRandomSoal[soalKe]].elementSoal.spriteJawabans[indexRandomJawaban[i]];
        }

        kunciJawabanSekarang = kumpulanSoals[indexRandomSoal[soalKe]].elementSoal.kunciJawaban; //menyimpan kunci jawaban
    }

    void CreateIndexRandom()
    {
        indexRandomSoal = new int[kumpulanSoals.Length]; //create slot

        for (int i = 0; i < indexRandomSoal.Length; i++)
        {
            indexRandomSoal[i] = i; //fill slot
        }

        IndexRandomValue(indexRandomSoal);

        indexRandomJawaban = new int[jawabans.Length]; //create slot

        for (int i = 0; i < indexRandomJawaban.Length; i++)
        {
            indexRandomJawaban[i] = i; //fill slot
        }
        

    }

    void IndexRandomValue(int[] indexRandom)
    {
        for(int i = 0; i < indexRandom.Length; i++)
        {
            int a = indexRandom[1];
            int b = Random.Range(0, indexRandom.Length);
            indexRandom[i] = indexRandom[b];
            indexRandom[b] = a;
        }
    }

    void SaveFirstPositionJawaban()
    {
        FirstPositionJawaban = new Vector2[jawabans.Length];

        for (int i = 0; i < FirstPositionJawaban.Length; i++)
        {
            FirstPositionJawaban[i] = jawabans[i].transform.localPosition;
        }
    }

    public void DragJawaban(int indexJawaban)
    {
        jawabans[indexJawaban].transform.position = Input.mousePosition;
    }

    public void EndDragJawaban(int indexJawaban)
    {
        float distance = Vector3.Distance(jawabans[indexJawaban].transform.localPosition, dropJawaban.transform.localPosition);

        if (distance < jarakDrop) // di dalam jangkauan
        {
            jawabans[indexJawaban].transform.localPosition = dropJawaban.transform.localPosition; //drop di kolom jawaban

            if (jawabans[indexJawaban].GetComponent<Image>().sprite == kumpulanSoals[indexRandomSoal[soalKe]].elementSoal.spriteJawabans[kunciJawabanSekarang])
            {
                Debug.Log("benar");

                imageCharacter.sprite = spriteCharacter[0];

                textHasilJawaban.text = "Exellent";

                totalScore += increaseScore;

                audioSFX.SoundSFXBenar();

            }
            else
            {
                Debug.Log("Salah");

                imageCharacter.sprite = spriteCharacter[1];

                textHasilJawaban.text = "Owww";

                audioSFX.SoundSFXSalah();

                if (totalScore - decreaseScore > 0)
                {
                    totalScore -= decreaseScore;
                }
                
            }
            textScore.text = totalScore.ToString();

            panelHasilJawaban.SetActive(true);

            Invoke("ClosePanelHasilJawabn", timeAktifPanelHasilJawabn);
        }
        else //di luar jangkauan
        {
            jawabans[indexJawaban].transform.localPosition = FirstPositionJawaban[indexJawaban]; //kembali ke posisi awal
        }
    }
}
