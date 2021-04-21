using System.Collections;
using UnityEngine;

public abstract class SelectedCardOption : MonoBehaviour
{
    public SelectedCardOptionsPanel panel;

    protected abstract void OnMouseDown();
}
