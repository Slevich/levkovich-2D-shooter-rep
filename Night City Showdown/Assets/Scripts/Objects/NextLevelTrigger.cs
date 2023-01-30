using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class NextLevelTrigger : MonoBehaviour
{
    #region Поля
    [Header("Timeline, which playing, when player in trigger.")]
    [SerializeField] private PlayableDirector triggerTimeline;
    #endregion

    #region Методы
    /// При вхождении игрока в триггер, проигрывается таймлайн.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            triggerTimeline.Play();
        }
    }
    #endregion
}
