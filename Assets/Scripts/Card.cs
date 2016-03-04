using System.Text;
using UnityEngine;

namespace Pyramid
{
    public class Card
    {
        public Suit Suit;
        public Level Level;
        public bool IsCapstone;
        public bool IsFaceUp;
        public bool IsSelected;

        public string CardDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (IsCapstone)
                    return "Capstone";

                switch (Suit)
                {
                    case Suit.Purple:
                        sb.Append("Purple ");
                        break;
                    case Suit.Green:
                        sb.Append("Green ");
                        break;
                    case Suit.Orange:
                        sb.Append("Orange ");
                        break;
                }

                switch (Level)
                {
                    case Level.First:
                        sb.Append("First Level");
                        break;
                    case Level.Second:
                        sb.Append("Second Level");
                        break;
                    case Level.Third:
                        sb.Append("Third Level");
                        break;
                }

                return sb.ToString();
            }
        }

        // Use this for initialization
        void Start()
        {
            SetupCard();
        }

        public void SetupCard()
        {

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
                    }
                    else
                    {
                        IsSelected = true;
                    }
                }
            }
        }
    }

    public enum Suit
    {
        Purple,
        Green,
        Orange
    }

    public enum Level
    {
        First,
        Second,
        Third
    }
}

