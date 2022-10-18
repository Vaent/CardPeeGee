using UnityEngine;

public class KeyboardInputManager : MonoBehaviour
{
    public static KeyCode KeybindingForDeck { get; private set; } = KeyCode.D;

    void Update()
    {
        if (Input.GetKeyDown(KeybindingForDeck)) GameController.RegisterInteractionWithDeck();
    }
}
