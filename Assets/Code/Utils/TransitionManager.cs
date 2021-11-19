using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public AudioSource PoisonedSound;
    public AudioSource NauseaedSound;
    public AudioSource DrinkingSound;
    public AudioSource DeathSound;
    public AudioSource DoorOpening;
    public AudioSource AlertSound;

    public AudioSource RegularMusic;
    public AudioSource CombatMusic;
    public AudioSource AlertMusic;
    public AudioSource ScaryMusic;

    private GameObject _player;

    private void Awake()
    {
        if (PublicVars.TransitionManager != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        PublicVars.TransitionManager = this;

        RegularMusic.Play();
    }

    private void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
            return;
        }

        transform.position = _player.transform.position;
    }
}
