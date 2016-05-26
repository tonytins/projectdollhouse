﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Files.AudioFiles;

namespace Files.AudioLogic
{
    /// <summary>
    /// Called upon by HIT subroutines to play sounds.
    /// </summary>
    public class SoundPlayer //TODO: Move this class into different library...
    {
        /// <summary>
        /// Starts playing a sound.
        /// </summary>
        /// <param name="WavData">The wav data for this sound.</param>
        /// <param name="SampleRate">The sample rate of the data.</param>
        /// <param name="LoopIt">Wether or not to loop the sound.</param>
        public static SoundEffectInstance PlaySound(ISoundCodec Sound, bool LoopIt = false)
        {
            SoundEffect Efx = new SoundEffect(Sound.DecompressedWav(), (int)Sound.GetSampleRate(), 
                Sound.IsMono() == true ? AudioChannels.Mono : AudioChannels.Stereo);
            SoundEffectInstance Inst = Efx.CreateInstance();

            if (LoopIt)
                Inst.IsLooped = true;

            Inst.Play();

            return Inst;
        }
    }

    /// <summary>
    /// Holds a SoundEffectInstance and SoundEffect that is used by the SoundPlayer.
    /// </summary>
    /*public class SFX
    {
        public SoundEffectInstance Instance;
        public SoundEffect Efx;

        /// <summary>
        /// Creates a new SFX instance.
        /// </summary>
        /// <param name="Sound">The SoundEffect instance used to store the sound.</param>
        /// <param name="ID">The InstanceID (from a DBPF) of the sound.</param>
        public SFX(SoundEffect Sound, SoundEffectInstance Inst)
        {
            Instance = Inst;
            Efx = Sound;
        }
    }*/
}
