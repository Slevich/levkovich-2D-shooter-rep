using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    #region Поля
    [Header("Sprite image, on which current sprite changes.")]
    [SerializeField] private Sprite newSprite;
    #endregion

    #region Методы
    // При попадании объекта в триггер, его спрайт меняется на другой.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyable"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
    #endregion
}
