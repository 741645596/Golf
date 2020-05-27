using UnityEngine;
using IGG.Core.Data.Config;
using IGG.Logging;

namespace IGG.Core.Manger.Sound
{
    public enum SoundType {
        music, // 背景音
        voice, // 音效
        dialog,// 剧情
    }
    /// <summary>
    /// 切换方式
    /// </summary>
    public enum PlayChangeType {
        Flash,  // 立即切换
        Fade,   // 淡入淡出切换
    }
    public static class SoundManager
    {
        private static SoundNode g_SoundNode = null;
        // 当前正在播放的音乐
        private static uint g_curPlayMusicID = 0;
        public static uint PlayMusicID;
        // 当前正在播放的剧情对话
        private static uint g_curPlayDialogID = 0;
        /// <summary>
        /// 设置sound 节点
        /// </summary>
        /// <returns></returns>
        public static void SetSoundNode(SoundNode node)
        {
            g_SoundNode = node;
        }
        /// <summary>
        /// 设置音乐音量，用于系统能够设置
        /// </summary>
        public static void SetMusicVolume(float Volume)
        {
            if (g_SoundNode != null) {
                g_SoundNode.SetMusicVolume(Volume);
            }
        }
        /// <summary>
        /// 设置音效音量，用于系统能够设置
        /// </summary>
        public static void SetVoiceVolume(float Volume)
        {
            if (g_SoundNode != null) {
                g_SoundNode.SetVoiceVolume(Volume);
            }
        }
        /// <summary>
        /// 暂停音乐
        /// </summary>
        public static void PauseMusic(SoundType type = SoundType.music)
        {
            if (g_SoundNode != null) {
                g_SoundNode.Pause(type);
            }
        }
        /// <summary>
        /// 恢复播放音乐
        /// </summary>
        public static void ResumeMusic(SoundType type = SoundType.music)
        {
            if (g_SoundNode != null) {
                g_SoundNode.Resume(type);
            }
        }
        /// <summary>
        /// 停止播放音乐
        /// </summary>
        public static void StopMusic(SoundType type = SoundType.music)
        {
            if (g_SoundNode != null) {
                g_SoundNode.Stop(type);
            }
        }
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="voiceID">对应着game_voice表</param>
        public static void PlayVoice(uint voiceID)
        {
            GameVoiceConfig c = GameVoiceDao.Inst.GetCfg(voiceID);
            if (c == null) {
                return;
            }
            var volumeScale = c.Volume / 100f;
            PlayVoice(voiceID, volumeScale);
        }
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="voiceID">对应着game_voice表</param>
        public static void PlayVoice(uint voiceID, float volumeScale)
        {
            GameVoiceConfig c = GameVoiceDao.Inst.GetCfg(voiceID);
            if (c != null) {
                ResourceManger.LoadVoice(c.VoiceName, false, (g) => {
                    if (g != null) {
                        if (g_SoundNode != null) {
                            g_SoundNode.PlayVoice(g as AudioClip, volumeScale);
                        }
                    } else {
                        Logging.Logger.LogDebug("voiceID:" + voiceID + "对应的音乐不存在");
                    }
                });
            } else {
                Logging.Logger.LogDebug("voiceID:" + voiceID + "数据不存在");
            }
        }
        
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="MusicID">对应着game_music表</param>
        /// <param name="volumeScale">默认0.0，使用系统设置音</param>
        /// <param name="ChangeStype">切换方式</param>
        /// <param name="isLoop">是否循环</param>
        public static void PlayMusic(uint MusicID, float volumeScale = 0.0f, PlayChangeType ChangeStype = PlayChangeType.Fade, bool isLoop = true)
        {
            if (g_curPlayMusicID == MusicID) {
                return;
            }
            GameMusicConfig c = GameMusicDao.Inst.GetCfg(MusicID);
            if (c != null) {
                volumeScale = c.Volume / 100f;
                ResourceManger.LoadMusic(c.MusicName, false, (g) => {
                    if (g != null) {
                        if (g_SoundNode != null) {
                            if (ChangeStype == PlayChangeType.Fade) {
                                if (GameMusicDao.Inst.GetCfg(g_curPlayMusicID) == null) {
                                    g_SoundNode.PlayMusicFadeOutIn(g as AudioClip, GetFadeinTime(), volumeScale, isLoop);
                                } else {
                                    g_SoundNode.PlayMusicFadeOutIn(g as AudioClip, GetFadeOutTime(), GetFadeinTime(), volumeScale, isLoop);
                                }
                            } else {
                                g_SoundNode.PlayMusicFlash(g as AudioClip, GetSwitchTime(), volumeScale, isLoop);
                            }
                            if (isLoop == true) {
                                g_curPlayMusicID = MusicID;
                            }
                        }
                    } else {
                        Logging.Logger.LogDebug("MusicID:" + MusicID + "对应的音乐不存在");
                    }
                });
            } else {
                Logging.Logger.LogDebug("MusicID:" + MusicID + "数据不存在");
            }
        }
        
        /// <summary>
        /// 播放剧情语音
        /// </summary>
        /// <param name="voiceID">对应着game_voice表</param>
        /// <param name="ChangeStype">切换方式</param>
        /// <param name="isLoop">是否循环</param>
        public static void PlayDialog(uint voiceID)
        {
            GameVoiceConfig c = GameVoiceDao.Inst.GetCfg(voiceID);
            if (c != null) {
                ResourceManger.LoadVoice(c.VoiceName, false, (g) => {
                    if (g != null) {
                        if (g_SoundNode != null) {
                            g_SoundNode.PlayDialog(g as AudioClip, c.Volume / 100f);
                        }
                    } else {
                        Logging.Logger.LogDebug("voiceID:" + voiceID + "对应的音乐不存在");
                    }
                });
            } else {
                Logging.Logger.LogDebug("voiceID:" + voiceID + "数据不存在");
            }
        }
        
        
        /// <summary>
        /// FadeinTime
        /// </summary>
        /// <returns></returns>
        private static float GetFadeinTime()
        {
            float time = 0.1f;
            GlobalsConfig c = GlobalsDao.Inst.GetCfg("music_fade_in_time");
            if (c != null) {
                time = int.Parse(c.Value) * 1.0f / 1000;
            }
            return time;
        }
        
        /// <summary>
        /// FadeinTime
        /// </summary>
        /// <returns></returns>
        private static float GetFadeOutTime()
        {
            float time = 0.1f;
            GlobalsConfig c = GlobalsDao.Inst.GetCfg("music_fade_out_time");
            if (c != null) {
                time = int.Parse(c.Value) * 1.0f / 1000;
            }
            return time;
        }
        
        
        /// <summary>
        /// FadeoutTime
        /// </summary>
        /// <returns></returns>
        private static float GetSwitchTime()
        {
            float time = 0.1f;
            GlobalsConfig c = GlobalsDao.Inst.GetCfg("music_switch_wait_time");
            if (c != null) {
                time = int.Parse(c.Value) * 1.0f / 1000;
            }
            return time;
        }
        
        
        /// <summary>
        /// 播放指定循环音效
        /// </summary>
        /// <param name="voiceID">对应着game_voice表</param>
        public static void PlayloopVoice(uint voiceID)
        {
            GameVoiceConfig c = GameVoiceDao.Inst.GetCfg(voiceID);
            if (c != null) {
                ResourceManger.LoadVoice(c.VoiceName, false, (g) => {
                    if (g != null) {
                        if (g_SoundNode != null) {
                            g_SoundNode.PlayLoopVoice(voiceID, g as AudioClip);
                        }
                    } else {
                        Logging.Logger.LogDebug("voiceID:" + voiceID + "对应的音乐不存在");
                    }
                });
            } else {
                Logging.Logger.LogDebug("voiceID:" + voiceID + "数据不存在");
            }
        }
        
        /// <summary>
        /// 停止播放指定循环音效
        /// </summary>
        /// <param name="voiceID">对应着game_voice表</param>
        public static void StoploopVoice(uint voiceID)
        {
            if (g_SoundNode != null) {
                g_SoundNode.StoploopVoice(voiceID);
            }
        }
        
        
        /// <summary>
        /// 停止播放所有循环音效
        /// </summary>
        public static void StopAllloopVoice()
        {
            if (g_SoundNode != null) {
                g_SoundNode.StopAllloopVoice();
            }
        }
    }
}
