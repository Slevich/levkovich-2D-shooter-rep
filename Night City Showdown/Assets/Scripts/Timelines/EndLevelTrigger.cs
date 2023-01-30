using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndLevelTrigger : MonoBehaviour
{
    #region Поля
    [Header("Timeline, which plays, when player comming to trigger.")]
    [SerializeField] private PlayableDirector timeline;
    #endregion

    #region Методы
    // При вхождении в триггер игрока,проигрывается таймлайн.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timeline.Play();
        }
    }
    #endregion
}
