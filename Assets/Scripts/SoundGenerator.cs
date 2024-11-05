using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum SOUNDTYPE
{
    xylophone,
    bell,
    cat,
    piano,
    bubble,
    trumpet,
    dog,
    flute,
    cork,
    tabletennis,
}

[System.Serializable]
public class SoundData
{
    public SOUNDTYPE type;
    public AudioClip clip;
}

public class SoundGenerator : MonoBehaviour
{

    public List<SoundData> AllSoundDatas;
    List<SoundData> usingSoundDatas;

    public GameObject soundObjectPrefab;
    List<GameObject> soundObjects;

    public GameObject numberDisabelPanel;
    public GameObject presentDisablePanel;
    public GameObject shuffleDisablePanel;
    public GameObject startDisablePanel;

    public float radius = 5;

    public int soundNumber = 2;
    public float presentInterval = 0;

    // Start is called before the first frame update
    void Start()
    {
        changeSoundNum(2);
        changePresentInterval(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSoundNum(int value)
    {
        // 音源オブジェクトの削除
        if (soundObjects != null)
        {
            removeSoundObjects();
        }

        // インスタンス変数を更新
        soundNumber = (int)value;

        // 音源オブジェクトを配置
        generateSoundObjects();

        // 使用音源の設定
        setUsingSoundDatas();


        // 音源をアタッチ
        attachSounds();
    }

    public void changePresentInterval(int value)
    {
        // インスタンス変数を更新
        presentInterval = value * 0.01f;
    }

    void setUsingSoundDatas()
    {
        // 使用音源だけ抽出
        usingSoundDatas = new List<SoundData>();
        for (int i = 0; i < soundNumber; i++)
        {
            usingSoundDatas.Add(AllSoundDatas[i]);
        }
    }

    void removeSoundObjects()
    {
        for (int i = 0; i < soundNumber; i++)
        {
            Destroy(soundObjects[i]);
        }

    }

    void generateSoundObjects()
    {
        // 音源の個数から自動的に配置を設定し、音源オブジェクトを生成
        soundObjects = new List<GameObject>();
        for(int i = 0; i < soundNumber; i++)
        {
            float angle = (180 / (soundNumber - 1)) * i;
            float x, z;
            x = -Mathf.Cos(Mathf.Deg2Rad * angle) * radius; // 左からなのでマイナス
            z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            // 音源インスタンス生成
            soundObjects.Add(Instantiate(soundObjectPrefab, new Vector3(x, 0, z), Quaternion.identity));
        }
    }

    void attachSounds()
    {
        // 音源オブジェクトに音源ファイルをアタッチ
        for (int i = 0; i < usingSoundDatas.Count; i++)
        {
            soundObjects[i].GetComponent<AudioSource>().clip = usingSoundDatas[i].clip;
        }
    }

    public void shuffleSounds()
    {
        // 音源の配置をランダム化
        usingSoundDatas = usingSoundDatas.OrderBy(a => Guid.NewGuid()).ToList();

        // 音源を再アタッチ
        attachSounds();
    }

    public void presentSounds()
    {
        // UIの無効化
        startDisablePanel.SetActive(true);
        shuffleDisablePanel.SetActive(true);
        numberDisabelPanel.SetActive(true);
        presentDisablePanel.SetActive(true);

        // 音源の提示
        StartCoroutine(PresentAllSounds());
    }



    IEnumerator PresentAllSounds()
    {
        // 音源の提示
        for(int i = 0;i < soundObjects.Count;i++)
        {
            soundObjects[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(presentInterval);
        }

        // すべての音源の提示が終了するのを待つ
        for(int i = 0; i < soundObjects.Count; i++)
        {
            if (soundObjects[i].GetComponent<AudioSource>().isPlaying)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f);

        // UIの有効化
        startDisablePanel.SetActive(false);
        shuffleDisablePanel.SetActive(false);
        numberDisabelPanel.SetActive(false);
        presentDisablePanel.SetActive(false);
    }
}
