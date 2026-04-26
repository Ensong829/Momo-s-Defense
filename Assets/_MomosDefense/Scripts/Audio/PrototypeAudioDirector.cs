using UnityEngine;

namespace MomosDefense.Audio
{
    public sealed class PrototypeAudioDirector : MonoBehaviour
    {
        private static PrototypeAudioDirector instance;

        private AudioSource musicSource;
        private AudioSource sfxSource;
        private AudioClip musicLoop;
        private AudioClip selectionClip;
        private AudioClip buildClip;
        private AudioClip upgradeClip;
        private AudioClip waveStartClip;
        private AudioClip skillClip;
        private AudioClip victoryClip;
        private AudioClip defeatClip;

        public static PrototypeAudioDirector Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject audioObject = new GameObject("Prototype Audio Director");
                    instance = audioObject.AddComponent<PrototypeAudioDirector>();
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = 0.18f;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.volume = 0.45f;

            BuildClips();
            EnsureMusic();
        }

        public static void PlaySelection()
        {
            Instance.PlaySfx(Instance.selectionClip, 0.9f);
        }

        public static void PlayBuild()
        {
            Instance.PlaySfx(Instance.buildClip, 1f);
        }

        public static void PlayUpgrade()
        {
            Instance.PlaySfx(Instance.upgradeClip, 1f);
        }

        public static void PlayWaveStart()
        {
            Instance.PlaySfx(Instance.waveStartClip, 1f);
        }

        public static void PlaySkill(float pitch = 1f)
        {
            Instance.PlaySfx(Instance.skillClip, Mathf.Clamp(pitch, 0.75f, 1.25f));
        }

        public static void PlayVictory()
        {
            Instance.PlaySfx(Instance.victoryClip, 1f);
        }

        public static void PlayDefeat()
        {
            Instance.PlaySfx(Instance.defeatClip, 0.95f);
        }

        private void EnsureMusic()
        {
            if (musicSource.isPlaying || musicLoop == null)
            {
                return;
            }

            musicSource.clip = musicLoop;
            musicSource.Play();
        }

        private void PlaySfx(AudioClip clip, float pitch)
        {
            if (clip == null)
            {
                return;
            }

            EnsureMusic();
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip);
        }

        private void BuildClips()
        {
            musicLoop = CreateMusicLoop();
            selectionClip = CreateToneClip("Selection", 660f, 0.08f, 0.18f);
            buildClip = CreateToneClip("Build", 440f, 0.14f, 0.25f);
            upgradeClip = CreateToneClip("Upgrade", 550f, 0.18f, 0.28f);
            waveStartClip = CreateToneClip("WaveStart", 330f, 0.32f, 0.28f);
            skillClip = CreateToneClip("Skill", 740f, 0.16f, 0.26f);
            victoryClip = CreateToneClip("Victory", 880f, 0.34f, 0.32f);
            defeatClip = CreateToneClip("Defeat", 220f, 0.4f, 0.24f);
        }

        private AudioClip CreateMusicLoop()
        {
            const int sampleRate = 44100;
            const float duration = 4f;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] samples = new float[sampleCount];
            float[] notes = { 261.63f, 329.63f, 392f, 329.63f };

            for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                float time = sampleIndex / (float)sampleRate;
                int noteIndex = Mathf.FloorToInt((time / duration) * notes.Length) % notes.Length;
                float frequency = notes[noteIndex];
                float envelope = 0.08f + 0.02f * Mathf.Sin(time * Mathf.PI * 2f);
                samples[sampleIndex] = Mathf.Sin(time * frequency * Mathf.PI * 2f) * envelope;
            }

            AudioClip clip = AudioClip.Create("PrototypeMusicLoop", sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private AudioClip CreateToneClip(string clipName, float frequency, float duration, float amplitude)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] samples = new float[sampleCount];

            for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                float time = sampleIndex / (float)sampleRate;
                float fade = 1f - (sampleIndex / (float)sampleCount);
                samples[sampleIndex] = Mathf.Sin(time * frequency * Mathf.PI * 2f) * amplitude * fade;
            }

            AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
