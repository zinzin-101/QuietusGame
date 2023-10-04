using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiaryPanelScript : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Color selectedDarkness;
    [SerializeField] DiaryScript diaryScript;

    private void Start()
    {
        _image.color = Color.white;
    }

    private void OnMouseEnter()
    {
        _image.color = selectedDarkness;
    }

    private void OnMouseExit()
    {
        _image.color = Color.white;
    }

    private void OnMouseUpAsButton()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (!EventSystem.current.currentSelectedGameObject.name.Equals(gameObject.name))
        //    {
        //        diaryScript.FlipDiaryPage();
        //    }
        //}

        diaryScript.FlipDiaryPage();
    }
}
