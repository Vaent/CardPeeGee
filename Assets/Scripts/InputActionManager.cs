using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionManager : MonoBehaviour
{
    InputAction deckAction;

    private void Start()
    {
        deckAction = InputSystem.actions.FindAction("Deck");
    }

    void Update()
    {
        if (deckAction.WasPerformedThisDynamicUpdate()) GameController.RegisterInteractionWithDeck();
    }
}
