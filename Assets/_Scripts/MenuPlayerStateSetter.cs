using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerStateSetter : MonoBehaviour
{

    [SerializeField]private PlayerState playerStateBeforeMenuOpen;
    // Start is called before the first frame update

    private void OnEnable()
    {
        playerStateBeforeMenuOpen = PlayerManager.Instance.currentPlayerState;
        PlayerManager.Instance.currentPlayerState = PlayerState.menuOpen;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.currentPlayerState = playerStateBeforeMenuOpen;
    }
}
