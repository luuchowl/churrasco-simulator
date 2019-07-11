using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Sound_Manager : Singleton<Sound_Manager> {
	#region Properties
	[BoxTitle("References")]
	public AudioHolder audioHolder;
	public AudioMixer mixer;
	public AudioMixerGroup musicGroup;
	public AudioMixerGroup soundFXGroup;
	[BoxTitle("Properties")]
	public int noOfChannels = 2;
	public float minPitch = .95f;
	public float maxPitch = 1.05f;
	[Tooltip("Checking this box will make that multiple channels can play the same audio clip. If unchecked, when the same audio clip is played, the channel currently playing it will stop and restart the clip.")]
	public bool canHaveSimultaneous = false;

	private AudioSource music;
	private AudioSource[] channels;
	private float lastVolume;
	#endregion

	#region MonoBehavior
	// Use this for initialization
	protected override void Awake() {
		base.Awake();

		music = gameObject.AddComponent<AudioSource>();
		music.loop = true;
		music.outputAudioMixerGroup = musicGroup;

		channels = new AudioSource[noOfChannels];
		for (int i = 0; i < noOfChannels; i++) {
			channels[i] = gameObject.AddComponent<AudioSource>();
			channels[i].playOnAwake = false;
			channels[i].outputAudioMixerGroup = soundFXGroup;
		}

		lastVolume = 1;
	}

	void Update() {
		//Mute/Unmute all channels
		if (Input.GetKeyDown(KeyCode.M)) {
			float masterVolume = 0;

			if(mixer.GetFloat("MasterVolume", out masterVolume)) {
				if (masterVolume != -80) {
					Mute();
				} else {
					Unmute();
				}
			}
		}
	}
	#endregion

	#region Getters
	public AudioSource GetMusicSource() {
		return music;
	}

	/// <summary>
	/// Looks for a free channel, returning null if there's none
	/// </summary>
	private AudioSource FindFreeChannel() {
		for (int i = 0; i < noOfChannels; i++) {
			if (!channels[i].isPlaying) {
				return channels[i];
			}
		}

		Debug.Log("There's no free audio channel!");
		return null;
	}
	#endregion

	#region Volume
	/// <summary>
	/// Set the volume of both the music and all of the EFX channels
	/// </summary>
	public void SetGlobalVolume(float volume) {
		mixer.GetFloat("MasterVolume", out lastVolume);

		volume = Mathf.Clamp01(volume);
		
		//Change the EXPOSED parameter on the Mixer (can't change it if it's not EXPOSED!)
		mixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 20, volume)); //-80 is the minimum volume the mixer can have (mute), while 0 is the default value (normal) and 20 is the the max volume
	}

	/// <summary>
	/// Set the volume of the music
	/// </summary>
	public void SetMusicVolume(float volume) {
		volume = Mathf.Clamp01(volume);
		mixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 20, volume));
	}

	/// <summary>
	/// Set the volume of all/one of the EFX channels
	/// </summary>
	public void SetSfxVolume(float volume, int channel = -1) {
		if (channel > -1) {
			channels[channel].volume = volume;
		} else {
			volume = Mathf.Clamp01(volume);
			mixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 20, volume));
		}
	}

	public void Mute() {
		mixer.SetFloat("MasterVolume", -80);
	}

	public void Unmute() {
		mixer.SetFloat("MasterVolume", lastVolume);
	}
	#endregion

	public void SetMusic(AudioClip clip) {
		music.clip = clip;
		music.Play();
	}

	/// <summary>
	/// Plays a sound with the defined paramenters
	/// </summary>
	public void PlaySingle(AudioClip clip, bool loop = false, bool randomPitch = false) {
		AudioSource channel;

		if (canHaveSimultaneous) { //If we can have simultaneous sounds of the same clip, that means more than one channel can play the same clip
			channel = FindFreeChannel();
		} else { //If not, then we have to check if a channel is already playing the clip
			channel = FindChannel(clip);
		}

		//If the channel was found, we play the clip
		if (channel && channel.isActiveAndEnabled) {
			channel.clip = clip;
			if (channel) {
				if (randomPitch) {
					channel.pitch = Random.Range(minPitch, maxPitch);
				} else {
					channel.pitch = 1;
				}

				channel.loop = loop;
				channel.Play();
			}
		}
	}

	/// <summary>
	/// Plays a random sound from the passed array with random pitch
	/// </summary>
	public AudioSource PlayRandomSFX(params AudioClip[] clips) {
		return PlayRandomSFX(false, clips);
	}

	public AudioSource PlayRandomSFX(bool loop, params AudioClip[] clips) {//"params" lets you pass more than one parameter of the same type
		if (clips.Length > 0) {
			AudioSource channel;
			int clipID = Random.Range(0, clips.Length);

			if (canHaveSimultaneous) {
				channel = FindFreeChannel();
			} else {
				channel = FindChannel(clips);
			}

			if (channel && channel.isActiveAndEnabled) {
				channel.clip = clips[clipID];
				channel.pitch = Random.Range(minPitch, maxPitch);
				channel.loop = loop;

				channel.Play();

				return channel;
			}
		} else {
			Debug.LogWarning("There's no sounds in the array");
		}

		return null;
	}

	private AudioSource FindChannel(params AudioClip[] clips) {
		//If an channel is already playing the same clip, we stop it and reuse it
		for (int i = 0; i < noOfChannels; i++) {
			for (int j = 0; j < clips.Length; j++) {
				if (channels[i].isPlaying && channels[i].clip == clips[j]) {
					channels[i].Stop();
					return channels[i];
				}
			}
		}

		//If no audio channel is playing the same clip, we look for a free channel
		return FindFreeChannel();
	}

	public void Stop(AudioClip clip) {
		if(clip == music.clip) {
			music.Stop();
			return;
		}

		AudioSource s = FindChannel(clip);

		if (s.isPlaying) {
			s.Stop();
		}
	}

	public void Stop(params AudioClip[] clips) {
		for (int i = 0; i < clips.Length; i++) {
			Stop(clips[i]);
		}
	}

	public void StopAll() {
		music.Stop();

		for (int i = 0; i < channels.Length; i++) {
			channels[i].Stop();
		}
	}
}
