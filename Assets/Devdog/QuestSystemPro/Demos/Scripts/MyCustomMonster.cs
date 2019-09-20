using System;
using System.Collections;
using Devdog.General;
using UnityEngine;

#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

namespace Devdog.QuestSystemPro.Demo
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(SetQuestProgressOnKilled))]
    public partial class MyCustomMonster : MonoBehaviour
    {
        public float walkSpeed = 4.0f;
        public float walkRadius = 10.0f;

        [NonSerialized]
        private WaitForSeconds waitTime = new WaitForSeconds(1.0f);

        private Vector3 _aimPosition;
        private NavMeshAgent _agent;
        private SetQuestProgressOnKilled _onKilled;

        public bool isDead { get; protected set; }

        private int _health = 100;
        public int health
        {
            get { return _health; }
            protected set
            {
                _health = value;
            }
        }

        protected void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = walkSpeed;
            _agent.enabled = true;

            _onKilled = GetComponent<SetQuestProgressOnKilled>();
        }

        protected void Start()
        {
            StartCoroutine(_ChooseNewLocation());
        }

        private IEnumerator _ChooseNewLocation()
        {
            while (true)
            {
                ChooseNewLocation();
                yield return waitTime;
            }
        }

        public virtual void ChooseNewLocation()
        {
            if (isDead || _agent == null || _agent.isOnNavMesh == false)
                return;

            _aimPosition = UnityEngine.Random.insideUnitCircle * walkRadius;
            _agent.SetDestination(transform.position + _aimPosition);
        }

        public void OnMouseDown()
        {
            health -= 50;

            if (health <= 0)
                Kill(); // Ah it died!
        }

        protected virtual void Kill()
        {
            if (isDead)
            {
                return;
            }

            isDead = true;
            DevdogLogger.Log("You killed it!");
            if (_onKilled != null)
            {
                _onKilled.OnKilled();
            }

            if (_agent.isOnNavMesh)
            {
#if UNITY_2017_1_OR_NEWER
                _agent.isStopped = true;
#else
                _agent.Stop();
#endif
            }

                StartCoroutine(SinkIntoGround());
        }

        protected virtual IEnumerator SinkIntoGround()
        {
            yield return waitTime;
            _agent.enabled = false; // To allow for sinking
            float timer = 0.0f;

            while (timer < 2.0f)
            {
                yield return null;

                transform.Translate(0, -1.0f * Time.deltaTime, 0.0f);
                timer += Time.deltaTime;
            }

            Destroy(gameObject);
        }
    }
}
