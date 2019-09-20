using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.QuestSystemPro.Demo
{
    public class OnTriggerEnterDestroy : MonoBehaviour
    {
        public AudioClipInfo onEnter;

        public void OnTriggerEnter(Collider other)
        {
            AudioManager.AudioPlayOneShot(onEnter);
            Destroy(gameObject);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            AudioManager.AudioPlayOneShot(onEnter);
            Destroy(gameObject);
        }
    }
}