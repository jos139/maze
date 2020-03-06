using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MazePanel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite startSprite = null;
    [SerializeField] private Sprite goalSprite = null;

    public void SetStart()
    {
        this.spriteRenderer.sprite = startSprite;
    }

    public void SetGoal()
    {
        this.spriteRenderer.sprite = goalSprite;
    }

    public void ChangeColor()
    {
        this.spriteRenderer.color = new Color(0, 0.5f, 0, 0.5f);
    }
}
