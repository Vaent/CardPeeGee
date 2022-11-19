using ExtensionMethods;
using System.Collections.Generic;
using static Text.Excerpts.Treasure;
using static Text.TextManager;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : Encounter
{
    private static Canvas leaveButtonCanvas;

    private List<Card> trapsOnChest;
    private Card trapSelectedForDisarm;

    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Treasure;

    public Treasure(List<Card> cards) : base(cards)
    {
        // TODO: improve acquisition of LeaveButton reference
        leaveButtonCanvas ??= GameObject.Find("LeaveButtonCanvas").GetComponent<Canvas>();
        leaveButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(AbandonTreasure);
        trapsOnChest = props.FindAll(card => (card.Suit == Suit.Spade));
    }

    void AbandonTreasure()
    {
        leaveButtonCanvas.enabled = false;
        GameState.EndEncounter(this);
    }

    // Treasure progression (if not automatically resolved) is through clicking on trap cards
    // or the "leave" button; Advance() has no purpose
    public override void Advance() { }

    protected override void BeginImpl()
    {
        Debug.Log("Found a treasure");
        DisplayText(Announce);
        if (trapsOnChest.Count == 0)
        {
            Timer.DelayThenInvoke(2, DeliverTreasure);
        }
        else
        {
            Debug.Log("The chest is trapped: " + trapsOnChest.Print());
            DisplayTextAsExtension(AnnounceTrap, Announce);

            DisplayText(PromptPlayCards);
            DisplayTextAsExtension(PromptClickToDisarm, 1, PromptPlayCards);
            DisplayTextAsExtension(PromptAbandonEncounter, 2, PromptClickToDisarm);
            leaveButtonCanvas.enabled = true;
            // TODO: display "points text" showing player's disarm bonus
        }
    }

    public override void CardSelected(Card card)
    {
        if (trapsOnChest.Contains(card))
        {
            trapSelectedForDisarm = card;
            // TODO: deal score cards
        }
        else
        {
            player.ConfigureSelectedCardOptions(card, Suit.Spade);
        }
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            // TODO: calculate scores
            // TODO: deal damage/remove trap if applicable
            // if (trapsOnChest.Count == 0) DeliverTreasure();
            // TODO: return cards to the deck if encounter is still active
            trapSelectedForDisarm = null;
        }
    }

    public void DeliverTreasure()
    {
        Debug.Log("The treasure is claimed");
        DisplayTextAsExtension(KaChing, AnnounceTrap, Announce);
        agitator.ResetDisplayProperties();
        player.Hand.Accept(EncounterCards.Cards);
        Timer.DelayThenInvoke(1.5f, GameState.EndEncounter, this);
    }
}
