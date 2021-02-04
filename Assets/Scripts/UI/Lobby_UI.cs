using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_UI : MonoBehaviour
{
    public enum LobbyState
    {
        Home,
        Shop,
        Static,
        NewGame
    }

    public LobbyState currState = LobbyState.Home;

    public GameObject home_UI;
    public GameObject shop_UI;
    public GameObject static_UI;
    public GameObject newGame_UI;

    public void CallHomeUI()
    {
        currState = LobbyState.Home;

        shop_UI.SetActive(false);
        static_UI.SetActive(false);
        newGame_UI.SetActive(false);

        home_UI.SetActive(true);
    }
    public void CallShopUI()
    {
        currState = LobbyState.Shop;

        home_UI.SetActive(false);
        static_UI.SetActive(false);
        newGame_UI.SetActive(false);

        shop_UI.SetActive(true);
    }
    public void ClickDailyBtn()
    {

    }
    public void CallStaticUI()
    {
        currState = LobbyState.Static;

        home_UI.SetActive(false);
        shop_UI.SetActive(false);
        newGame_UI.SetActive(false);

        static_UI.SetActive(true);
    }
    public void CallNewGameUI()
    {
        currState = LobbyState.NewGame;

        home_UI.SetActive(false);
        shop_UI.SetActive(false);
        static_UI.SetActive(false);

        newGame_UI.SetActive(true);
    }
}
