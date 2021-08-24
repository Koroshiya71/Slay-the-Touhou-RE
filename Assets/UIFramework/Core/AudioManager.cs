using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Unity3D游戏引擎一共支持4个音乐格式的文件:
//.AIFF 适用于较短的音乐文件可用作游戏打斗音效
//.WAV  适用于较短的音乐文件可用作游戏打斗音效
//.MP3 适用于较长的音乐文件可用作游戏背景音乐
//.OGG 适用于较长的音乐文件可用作游戏背景音乐
namespace GameCore
{
    public class AudioManager :UnitySingleton<AudioManager>
    {
        
        //背景音乐(1个)
        private AudioSource musicSource;
        private AudioClip musicClip;
        //音效(n个)
        private AudioSource effectSource;
        private AudioClip[] effectClip;
        private Dictionary<string, AudioClip> dicEffectClip = new Dictionary<string, AudioClip>();
        //是否打开声音
        public bool isOpenAudio = true;
        //默认音量
        public float audioValue = 0.3f;
        //void Awake()
        //{
        //    InitAudio();
        //}
        
        public void InitAudioManager()
        {
            musicSource = GameTool.GetTheChildComponent<AudioSource>(this.gameObject, "AudioSource_Music");
            musicClip = Resources.Load<AudioClip>("Audio/MusicAudio/GameMusic");
            musicSource.clip = musicClip;

            effectSource = GameTool.GetTheChildComponent<AudioSource>(this.gameObject, "AudioSource_Effect");
            effectClip = Resources.LoadAll<AudioClip>("Audio/EffectAudio");
            for (int i = 0; i < effectClip.Length; i++)
            {
                dicEffectClip.Add(effectClip[i].name, effectClip[i]);
            }

            if (!GameTool.HasKey("AudioValue"))
            {
                GameTool.SetFloat("AudioValue", audioValue);
                musicSource.volume = audioValue;
                effectSource.volume = audioValue;
               
            }
            else
            {   
                float value= GameTool.GetFloat("AudioValue");
                musicSource.volume = value;
                effectSource.volume = value;
                audioValue = value;
               // Debug.Log("音量为" + audioValue);
            }
            PlayMusic();
        }
        //启用与禁用effectSource
        public void SetEffectSourceEnable(bool enable)
        {
            if (enable)
            {
                effectSource.clip = null;
                effectSource.enabled = true;
            }
            else
            {
                effectSource.enabled = false;
            }
        }
        //对外提供,播放音效的方法
        public void PlayEffectMusic(string effectMusicName)
        {
            if (!isOpenAudio)
            {
                return;
            }
            //for (int i = 0; i < effectClip.Length; i++)
            //{
            //    if (effectClip[i].name== effectMusicName)
            //    {
            //        effectSource.clip = effectClip[i];
            //        effectSource.Play();
            //        break;
            //    }
            //}
            if (dicEffectClip.ContainsKey(effectMusicName))
            {
                effectSource.clip = dicEffectClip[effectMusicName];
                effectSource.Play();
            }
        }
        //对外提供,播放背景音乐的方法
        public void PlayMusic()
        {
            if (GameTool.HasKey("IsOpenAudio"))
            {
                isOpenAudio = bool.Parse(GameTool.GetString("IsOpenAudio"));
            }
            else
            {
                GameTool.SetString("IsOpenAudio","true");
                isOpenAudio = true;
            }
            if (isOpenAudio)
            {
                musicSource.Play();
            }
        }
        //声音开关(背景音乐与音效)
        public void OpenOrCloseAudio(bool isOn)
        {
            isOpenAudio = !isOn;
            if (isOpenAudio)//打开声音
            {
                musicSource.Play();
            }
            else//关闭声音
            {
                musicSource.Pause();
            }
            GameTool.SetString("IsOpenAudio", isOpenAudio.ToString());
        }
      
        //设置音量的大小
        public void SetVolumeValue(float value)
        {
            musicSource.volume = value;
            effectSource.volume = value;
            audioValue = value;
            GameTool.SetFloat("AudioValue",value);            
        }
    }

}
