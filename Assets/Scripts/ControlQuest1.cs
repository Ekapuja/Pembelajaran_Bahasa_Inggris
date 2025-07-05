using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlQuest1 : MonoBehaviour
{

    public AudioSFX audioSFX;

    [System.Serializable]

    public class Soals
    {
        [System.Serializable]

        public class ElementSoals
        {
            public Sprite spriteSoal; //soal gambar
            public string stringSoal; //soal text
            public Sprite[] spriteJawaban; //jawaban gambar
            public string[] stringJawaban; //jawaban text

            public int kunciJawaban; //kunci jawaban dalam array
        }

        public ElementSoals elementSoals;
    }

    public Soals[] soals;

    [Header("Random Index")]

    //random index
    public int[] indexRandomSoal;
    public int[] indexRandomJawaban;
    public int totalsoal; //total soal yg dipakai
    public int urutanSoal; //0 - 1
    int jawabanBenar;

    [Header("UI Soal dan Jawaban")]
    public Image imageSoal; //soal image
    public Text textSoal; //soal text

    public Image[] imageJawaban; //jawaban gambar
    public Text[] textJawaban; //jawaban text

    [Header("Voice Over")]
    public AudioSource audioSourceVO;
    public AudioClip[] audioClipVOs;
    public Button buttonPlayVO;

    [Header("Score")]
    public Text textScore;
    public Text textScoreAkhir;
    public int increaseScore; // tambah score
    public int decreaseScore; // kurang score
    public int totalScoreAkhir;
    public GameObject panelEndGame;

    [Header("kondisi next soal")]
    public bool isJawabanHarusBenar; //setting
    public bool isJawabanBenar; // untuk debug

    [Header("sistem hasil jawabn atau jeda")]
    public bool isHasilJawab;
    public GameObject panelHasilJawab;
    public Image imageCharacterHasilJawab;
    public Sprite[] spritesCharacterHasilJawab; //0 benar - 1 salah
    public Text textHasilJawab;
    public string[] stringHasilJawab;
    int indexHasilJawab;
    public float waktuTungguHasilJawab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateIndexRandomSoal();
        GenerateIndexRandomJawabn();

        GenerateSoal();
    }

    void GenerateIndexRandomSoal()
    {
        indexRandomSoal = new int[soals.Length]; //buat stot array
        for(int i = 0; i < indexRandomSoal.Length; i++) //fill slot array dgn int
        {
            indexRandomSoal[i] = i;
        }

        for (int i =0; i < indexRandomSoal.Length; i++) //random index
        {
            int a = indexRandomSoal[i];
            int b = Random.Range(0, indexRandomSoal.Length);
            indexRandomSoal[i] = indexRandomSoal[b];
            indexRandomSoal[b] = a;
        }
    }

    void GenerateIndexRandomJawabn()
    {
        indexRandomJawaban = new int[2]; //2 a dan b
        for (int i = 0; i < indexRandomJawaban.Length; i++) //fill slot array dgn int
        {
            indexRandomJawaban[i] = i;
        }

        for (int i = 0; i < indexRandomJawaban.Length; i++) //random index
        {
            int a = indexRandomJawaban[i];
            int b = Random.Range(0, indexRandomJawaban.Length);
            indexRandomJawaban[i] = indexRandomJawaban[b];
            indexRandomJawaban[b] = a;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    void IncreaseScore()
    {
        totalScoreAkhir += increaseScore; //nambah scor
        textScore.text = totalScoreAkhir.ToString();
    }

    void StopVoiceOver()
    {
        if (audioSourceVO.isPlaying == true) //jika main
        {
            audioSourceVO.Stop();
            buttonPlayVO.interactable = true; //VO hidup
            CancelInvoke(); //cancel
        }
    }

    void ReactiveButtonVO()
    {
        buttonPlayVO.interactable = true; //aktif VO
    }

    public void ButtonPlayVoiceOver()
    {
        if(audioSourceVO.isPlaying == false) //jika tidak main
        {
            //audioSourceVO.clip = audioClipVOs[indexRandomSoal[urutanSoal]]; //set up audio
            audioSourceVO.Play();
            buttonPlayVO.interactable = false; //VO mati

            Invoke("ReactiveButtonVO", audioClipVOs[indexRandomSoal[urutanSoal]].length); //fuction berjalan sesui VO
        }
        
        
    }

    public void ButtonJawaban(int indexJawaban)
    {
        if (indexRandomJawaban[indexJawaban] == jawabanBenar)
        {
            Debug.Log("Benar");

            StopVoiceOver();

            IncreaseScore();

            isJawabanBenar = true;

            indexHasilJawab = 0; //Benar

            audioSFX.SoundSFXBenar();
        }
        else
        {
            Debug.Log("Salah");

            indexHasilJawab = 1; //Salah

            audioSFX.SoundSFXSalah();
        }

        if (isJawabanHarusBenar == true) //kontrol kondisi
        {
            if(isJawabanBenar == true) //cek kondisi
            {
                if(isHasilJawab == false)
                {
                    GenerateNextSoal();
                }
                else
                {
                    HasilJawab();
                }
            }
        }
        else
        {
            if (isHasilJawab == false)
            {
                GenerateNextSoal();
            }
            else
            {
                HasilJawab();
            }
        }
    }

    void TutupHasilJawab()
    {
        panelHasilJawab.SetActive(false);

        GenerateNextSoal();
    }

    void HasilJawab()
    {
        panelHasilJawab.SetActive(true);

        imageCharacterHasilJawab.sprite = spritesCharacterHasilJawab[indexHasilJawab]; //gambar

        textHasilJawab.text = stringHasilJawab[indexHasilJawab]; //text

        Invoke("TutupHasilJawab", waktuTungguHasilJawab);
    }

    void GenerateNextSoal()
    {
        if (urutanSoal < totalsoal - 1) //Check kondisi
        {
            urutanSoal += 1; //Nambah soal
            GenerateIndexRandomJawabn(); //mengacak jawaban
            GenerateSoal();

            isJawabanBenar = false;
        }
        else
        {
            //Finish
            Debug.Log("Finish Gmae");

            panelEndGame.SetActive(true);
            textScoreAkhir.text = totalScoreAkhir.ToString();
        }
    }

    void GenerateSoal()
    {
        //Update soal
        imageSoal.sprite = soals[indexRandomSoal[urutanSoal]].elementSoals.spriteSoal; //soal gambar
        textSoal.text = soals[indexRandomSoal[urutanSoal]].elementSoals.stringSoal; //soal text

        //Update Jawaban
        for (int i = 0; i < 2; i++) //2 krn AB
        {
            //imageJawaban[i].sprite = soals[indexRandomSoal[urutanSoal]].elementSoals.spriteJawaban[indexRandomJawaban[i]]; // jawaban dengan gambar
            textJawaban[i].text = soals[indexRandomSoal[urutanSoal]].elementSoals.stringJawaban[indexRandomJawaban[i]]; // jawaban dengan text
        }

        //Jawaban benar
        jawabanBenar = soals[indexRandomSoal[urutanSoal]].elementSoals.kunciJawaban; //ngambil kunci jawaban

        //ButtonPlayVoiceOver(); //VO di awal soal
    }
}
