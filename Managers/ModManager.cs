using Il2CppGameLogic;
using Il2CppPeroPeroGames.GlobalDefines;
using MuseDashMirror.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace StricterJudge.Managers;
using static SettingsManager;

internal static partial class ModManager
{
    [PnlMenuToggle("StricterJudgeToggle", "Stricter Judge", nameof(SettingsManager.IsEnabled))]
    private static GameObject EnabledToggle { get; set; }
    
    private static readonly HashSet<uint> AcceptedEnemies =
    [
        (uint)NoteType.Monster,
        (uint)NoteType.Hide,
        (uint)NoteType.Press
    ];

    internal static bool UpdateNoteJudge(ref MusicData musicData)
    {
        NoteConfigData noteData = musicData.noteData;

        if (!AcceptedEnemies.Contains(noteData.type)) return false;

        noteData.left_perfect_range = PerfectLeftRange.RangeDec;
        noteData.right_perfect_range = PerfectRightRange.RangeDec;
        noteData.left_great_range = GreatLeftRange.RangeDec;
        noteData.right_great_range = GreatRightRange.RangeDec;

        musicData.noteData = noteData;
        return true;
    }

    private static void CreateObjectFromRange(RangeClass rangeObject, GameObject baseGo, Transform parent, Vector3 posOffset)
    {
        GameObject rangeGo = Object.Instantiate(baseGo, parent);
        rangeGo.name = rangeObject.Name;

        Text rangeText = rangeGo.GetComponent<Text>();
        rangeText.text = rangeObject.DisplayText;
        rangeText.enabled = true;

        GameObject valueGo = rangeGo.transform.GetChild(0).gameObject;
        Text valueText = valueGo.GetComponent<Text>();
        valueText.text = rangeObject.GetRange();
        valueText.enabled = true;

        Transform transform = rangeGo.transform;
        transform.SetParent(parent.parent);

        transform.localPosition = parent.localPosition + posOffset;
        transform.localRotation = parent.localRotation;
        transform.localScale *= 0.5f;

        transform.SetParent(parent);
    }

    internal static void CreateTextObjects(GameObject baseGo, Transform parent)
    {
        Vector3 perfectLeftOffset;
        Vector3 perfectRightOffset;
        Vector3 greatLeftOffset;
        Vector3 greatRightOffset;
        
        var isHighestActive = baseGo.GetComponent<Text>().enabled;
        
        if (isHighestActive)
        {
            perfectLeftOffset = new Vector3(30f, -100f, 0f);
            perfectRightOffset = new Vector3(550f, 10f, 0f);
            greatLeftOffset = new Vector3(60f, -150f, 0f);
            greatRightOffset = new Vector3(580f, -40f, 0f);
        }
        else
        {
            perfectLeftOffset = new Vector3(0, 0, 0f);
            perfectRightOffset = new Vector3(180f, 45f, 0f);
            greatLeftOffset = new Vector3(30f, -50f, 0f);
            greatRightOffset = new Vector3(210f, -5f, 0f);
        }
        
        CreateObjectFromRange(GreatLeftRange, baseGo, parent, greatLeftOffset);
        CreateObjectFromRange(PerfectLeftRange, baseGo, parent, perfectLeftOffset);
        CreateObjectFromRange(PerfectRightRange, baseGo, parent, perfectRightOffset);
        CreateObjectFromRange(GreatRightRange, baseGo, parent, greatRightOffset);
    }
}