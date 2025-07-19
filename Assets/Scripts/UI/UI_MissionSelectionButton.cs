using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MissionSelectionButton : UI_Button
{
    private UI_MissionSelection missionUI;
    [SerializeField] private Mission myMission;
    private TextMeshProUGUI myText;
    public override void Start()
    {
        base.Start();
        missionUI = GetComponentInParent<UI_MissionSelection>(true);
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myMission.missionName;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        missionUI.UpdateMissionDescription(myMission.missionDescription);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        missionUI.UpdateMissionDescription("Choose a Mission");
    }
}
