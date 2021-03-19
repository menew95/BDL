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

    public List<Image> bottomBarImage = new List<Image>();

    public Gold_UI gold_UI;

    public void CallHomeUI()
    {
        currState = LobbyState.Home;

        shop_UI.SetActive(false);
        static_UI.SetActive(false);
        newGame_UI.SetActive(false);
        gold_UI.gameObject.SetActive(true);
        //Icon
        bottomBarImage[0].color = Color.gray;
        bottomBarImage[1].color = Color.white;
        bottomBarImage[2].color = Color.gray;
        //BG
        bottomBarImage[3].color = Color.white;
        bottomBarImage[4].color = selectedBGColor;
        bottomBarImage[5].color = Color.white;

        AudioManager.Instance.CallAudioClip(1);
        home_UI.SetActive(true);
    }

    Color noneSelectedIconColor = new Color(1, 200f / 255f, 150f / 255f);
    Color selectedBGColor = new Color(200f / 255f, 200f / 255f, 1f);

    public void CallShopUI()
    {
        currState = LobbyState.Shop;

        home_UI.SetActive(false);
        static_UI.SetActive(false);
        newGame_UI.SetActive(false);
        gold_UI.gameObject.SetActive(true);
        //Icon
        bottomBarImage[0].color = Color.white;
        bottomBarImage[1].color = Color.gray;
        bottomBarImage[2].color = Color.gray;
        //BG
        bottomBarImage[3].color = selectedBGColor;
        bottomBarImage[4].color = Color.white;
        bottomBarImage[5].color = Color.white;

        AudioManager.Instance.CallAudioClip(1);
        shop_UI.SetActive(true);
    }
    
    public void CallNewGameUI()
    {
        currState = LobbyState.NewGame;

        home_UI.SetActive(false);
        shop_UI.SetActive(false);
        static_UI.SetActive(false);
        gold_UI.gameObject.SetActive(true);
        //Icon
        bottomBarImage[0].color = Color.gray;
        bottomBarImage[1].color = Color.gray;
        bottomBarImage[2].color = Color.gray;
        //BG
        bottomBarImage[3].color = Color.white;
        bottomBarImage[4].color = Color.white;
        bottomBarImage[5].color = Color.white;

        AudioManager.Instance.CallAudioClip(1);
        newGame_UI.SetActive(true);
    }

    public void CallStaticUI()
    {
        currState = LobbyState.Static;

        home_UI.SetActive(false);
        shop_UI.SetActive(false);
        newGame_UI.SetActive(false);
        gold_UI.gameObject.SetActive(false);
        //Icon
        bottomBarImage[0].color = Color.gray;
        bottomBarImage[1].color = Color.gray;
        bottomBarImage[2].color = Color.white;
        //BG
        bottomBarImage[3].color = Color.white;
        bottomBarImage[4].color = Color.white;
        bottomBarImage[5].color = selectedBGColor;

        AudioManager.Instance.CallAudioClip(1);
        static_UI.SetActive(true);
    }
}
