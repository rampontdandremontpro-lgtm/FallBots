using UnityEngine;

public class Rocket : MonoBehaviour
{
    [System.Serializable]
    public class Reference
    {
        public Rigidbody Rigidbody;
        public ParticleSystem Fire;
        public GameObject Explosion;
    }

    [System.Serializable]
    public class Settings
    {
        public bool LaunchOnEnable = false;
        public float Speed = 20;
        public float Duration = 5;
    }

    [System.Serializable]
    public class State
    {
        public bool Launched = false;
        public float FlighTime = 0;
        public bool Disabled = false;
    }

    [SerializeField] private Reference _reference;
    [SerializeField] private Settings _settings;
    [SerializeField] private State _state;

    private void OnEnable()
    {
        if (_settings.LaunchOnEnable)
        {
            Launch();
        }
    }

    private void Update()
    {
        if (_state.Launched && _state.FlighTime < _settings.Duration)
        {
            Vector3 force = transform.up * _settings.Speed * Time.deltaTime * 100;
            _reference.Rigidbody.AddForce(force);

            _state.FlighTime += Time.deltaTime;
        }
        else if (!_state.Disabled)
        {
            _state.Disabled = true;

            _reference.Fire.Stop();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!_state.Launched && _state.FlighTime > .5f)
        {
            GameObject explosion = Instantiate(_reference.Explosion);
            explosion.transform.position = transform.position;

            Destroy(gameObject);
        }
    }

    [ContextMenu("Launch")]
    public void Launch()
    {
        _state.Launched = true;

        _state.Disabled = false;
        _state.FlighTime = 0;
        _reference.Fire.Play();
    }
}