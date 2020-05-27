using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace IGG.Core.Manger.Sound
{

    public class SoundNode : Node
    {
        private Dictionary<uint, AudioSource> ListAs = new Dictionary<uint, AudioSource>();
        // 播放声音控件
        public AudioSource VoiceSource;
        
        // 专门播放背景音乐
        public AudioSource MusicSource;
        
        // 剧情音乐
        public AudioSource DialogSource;
        
        // 循环voice节点
        public GameObject loopVoiceNode;
        
        //音量音效
        private float m_MusicVolume = 0.5f;
        private float m_VoiceVolume = 1.0f;
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            SoundManager.SetSoundNode(this);
        }
        /// <summary>
        /// 设置音乐音量，用于系统能够设置
        /// </summary>
        public void SetMusicVolume(float Volume)
        {
            m_MusicVolume = Volume;
            if (MusicSource != null) {
                MusicSource.volume = m_MusicVolume;
            }
            if (DialogSource != null) {
                DialogSource.volume = m_MusicVolume;
            }
        }
        /// <summary>
        /// 设置音效音量，用于系统能够设置
        /// </summary>
        public void SetVoiceVolume(float Volume)
        {
            m_VoiceVolume = Volume;
            if (VoiceSource != null) {
                VoiceSource.volume = m_VoiceVolume;
            }
        }
        
        public void Pause(SoundType type)
        {
            AudioSource As = GetAudioSource(type);
            if (As != null) {
                As.Pause();
            }
            
        }
        /// <summary>
        /// 获取AudioSource组件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private AudioSource GetAudioSource(SoundType type)
        {
            if (type == SoundType.music) {
                return MusicSource;
            } else if (type == SoundType.dialog) {
                return DialogSource;
            } else {
                return VoiceSource;
            }
        }
        
        public void Resume(SoundType type)
        {
            AudioSource As = GetAudioSource(type);
            if (As != null) {
                As.UnPause();
            }
        }
        
        public void Stop(SoundType type)
        {
            AudioSource As = GetAudioSource(type);
            if (As != null) {
                As.Stop();
                As.clip = null;
                As.loop = false;
            }
        }
        /// <summary>
        ///  播放音效
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volumeScale"></param>
        public void PlayVoice(AudioClip clip, float volumeScale)
        {
            if (VoiceSource != null && clip != null) {
                VoiceSource.PlayOneShot(clip, volumeScale);
            }
        }
        
        /// <summary>
        ///  播放音效
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volumeScale"></param>
        public void PlayLoopVoice(uint voiceID, AudioClip clip)
        {
            if (loopVoiceNode == null) {
                return;
            }
            if (ListAs.ContainsKey(voiceID) == true) {
                return;
            } else {
                AudioSource As = loopVoiceNode.AddComponent<AudioSource>();
                if (As != null) {
                    As.clip = clip;
                    As.clip.LoadAudioData();
                    As.loop = true;
                    As.playOnAwake = true;
                    As.volume = m_VoiceVolume;
                    As.Play();
                    ListAs.Add(voiceID, As);
                }
            }
        }
        /// <summary>
        /// 停止播放循环音效
        /// </summary>
        /// <param name="voiceID"></param>
        public  void StoploopVoice(uint voiceID)
        {
            if (loopVoiceNode == null) {
                return;
            }
            if (ListAs.ContainsKey(voiceID) == true) {
                AudioSource As = ListAs[voiceID];
                if (As != null) {
                    Destroy(As);
                }
                ListAs.Remove(voiceID);
            }
        }
        
        
        /// <summary>
        /// 停止播放循环音效
        /// </summary>
        /// <param name="voiceID"></param>
        public void StopAllloopVoice()
        {
            if (loopVoiceNode == null) {
                return;
            }
            ListAs.Clear();
            AudioSource[] As = loopVoiceNode.GetComponents<AudioSource>();
            foreach (AudioSource s in As) {
                Destroy(s);
            }
        }
        
        /// <summary>
        ///  播放音效
        /// </summary>
        /// <param name="clip"></param>
        public void PlayDialog(AudioClip clip, float volumeScale)
        {
            if (DialogSource != null && clip != null) {
                DialogSource.PlayOneShot(clip, volumeScale);
            }
        }
        /// <summary>
        /// 淡入淡出播放音乐
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="FadeOutTime">淡出时间</param>
        /// <param name="FadeInTime">淡入时间</param>
        /// <param name="loop"></param>
        public void PlayMusicFadeOutIn(AudioClip clip, float FadeOutTime, float FadeInTime, float volumeScale, bool loop = true)
        {
            if (volumeScale == 0.0f) {
                volumeScale = m_MusicVolume;
            }
            AudioSource As = GetAudioSource(SoundType.music);
            if (As == null) {
                return;
            }
            // 通过Tween将声音淡入淡出
            DOTween.To(() => As.volume, value => As.volume = value, 0, FadeOutTime).OnComplete(() => {
                As.clip = clip;
                As.clip.LoadAudioData();
                As.loop = loop;
                As.volume = volumeScale;
                As.Play();
                DOTween.To(() => As.volume, value => As.volume = value, volumeScale, FadeInTime);
            });
        }
        
        /// <summary>
        /// 淡入播放音乐，播放第一首音乐的情况下。
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="FadeInTime">淡入时间</param>
        /// <param name="loop"></param>
        public void PlayMusicFadeOutIn(AudioClip clip, float FadeInTime, float volumeScale, bool loop = true)
        {
            if (volumeScale == 0.0f) {
                volumeScale = m_MusicVolume;
            }
            AudioSource As = GetAudioSource(SoundType.music);
            if (As == null) {
                return;
            }
            As.clip = clip;
            As.clip.LoadAudioData();
            As.loop = loop;
            As.volume = 0;
            As.Play();
            DOTween.To(() => As.volume, value => As.volume = value, volumeScale, FadeInTime);
        }
        
        /// <summary>
        /// 立即播放音乐
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="switchTime">切换间隔时间</param>
        /// <param name="loop"></param>
        public void PlayMusicFlash(AudioClip clip, float switchTime, float volumeScale, bool loop = true)
        {
            if (volumeScale == 0.0f) {
                volumeScale = m_MusicVolume;
            }
            AudioSource As = GetAudioSource(SoundType.music);
            if (As == null) {
                return;
            }
            Stop(SoundType.music);
            DOTween.To(() => As.volume, value => As.volume = value, volumeScale, switchTime).OnComplete(() => {
                As.clip = clip;
                As.clip.LoadAudioData();
                As.loop = loop;
                As.volume = volumeScale;
                As.Play();
            });
        }
    }
}
