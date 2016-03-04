using System;
using UnityEngine;
using System.Collections;

public class PyramidCard : MonoBehaviour
{
    public PyramidSuit Suit;
    public PyramidLevel Level;
    public bool IsCapstone;
    public bool IsFaceUp;
    public bool IsSelected;

    public Sprite CardGreen1;
    public Sprite CardGreen2;
    public Sprite CardGreen3;
    public Sprite CardOrange1;
    public Sprite CardOrange2;
    public Sprite CardOrange3;
    public Sprite CardPurple1;
    public Sprite CardPurple2;
    public Sprite CardPurple3;
    public Sprite CardCapstone;
    public Sprite CardBack;

    // Use this for initialization
    void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (IsFaceUp)
        {
            if (IsCapstone)
            {
                spriteRenderer.sprite = CardCapstone;
            }
            else
            {

                switch (Suit)
                {
                    case PyramidSuit.Green:
                        switch (Level)
                        {
                            case PyramidLevel.First:
                                spriteRenderer.sprite = CardGreen1;
                                break;
                            case PyramidLevel.Second:
                                spriteRenderer.sprite = CardGreen2;
                                break;
                            case PyramidLevel.Third:
                                spriteRenderer.sprite = CardGreen3;
                                break;
                        }
                        break;
                    case PyramidSuit.Orange:
                        switch (Level)
                        {
                            case PyramidLevel.First:
                                spriteRenderer.sprite = CardOrange1;
                                break;
                            case PyramidLevel.Second:
                                spriteRenderer.sprite = CardOrange2;
                                break;
                            case PyramidLevel.Third:
                                spriteRenderer.sprite = CardOrange3;
                                break;
                        }
                        break;
                    case PyramidSuit.Purple:
                        switch (Level)
                        {
                            case PyramidLevel.First:
                                spriteRenderer.sprite = CardPurple1;
                                break;
                            case PyramidLevel.Second:
                                spriteRenderer.sprite = CardPurple2;
                                break;
                            case PyramidLevel.Third:
                                spriteRenderer.sprite = CardPurple3;
                                break;
                        }
                        break;
                }
            }
        }
        else
        {
            spriteRenderer.sprite = CardBack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float notSelectedCardYPos = -3.8f;
        float selectedCardYPos = -5.8f;

        if (IsFaceUp)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (IsSelected)
                {
                    IsSelected = false;
                    transform.position = new Vector3(transform.position.x, notSelectedCardYPos, 0);
                }
                else
                {
                    IsSelected = true;
                    transform.position = new Vector3(transform.position.x, selectedCardYPos, 0);
                }
            }
        }
    }
}

public enum PyramidSuit
{
    Purple,
    Green,
    Orange
}

public enum PyramidLevel
{
    First,
    Second,
    Third
}
