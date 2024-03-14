using Il2Cpp;
using Il2CppGameLogic;
using Il2CppPeroPeroGames.GlobalDefines;
using MuseDashMirror.Attributes;
using StricterJudge.Models;
using UnityEngine;
using UnityEngine.UI;

namespace StricterJudge.Managers;

internal static partial class ModManager
{
    private static readonly HashSet<uint> AcceptedEnemies =
    [
        (uint)NoteType.Monster,
        (uint)NoteType.Hide,
        (uint)NoteType.Press
    ];

    [PnlMenuToggle("StricterJudgeToggle", "Stricter Judge", nameof(IsEnabled))]
    private static GameObject EnabledToggle { get; set; }

    internal static bool UpdateNoteJudge(ref MusicData musicData)
    {
        var noteData = musicData.noteData;

        if (!AcceptedEnemies.Contains(noteData.type))
        {
            return false;
        }

        noteData.left_perfect_range = PerfectLeftRange.RangeDec;
        noteData.right_perfect_range = PerfectRightRange.RangeDec;
        noteData.left_great_range = GreatLeftRange.RangeDec;
        noteData.right_great_range = GreatRightRange.RangeDec;

        musicData.noteData = noteData;
        return true;
    }

    private static void CreateObjectFromRange(RangeClass rangeObject, GameObject baseGo, Transform parent,
        Vector3 posOffset)
    {
        var rangeGo = baseGo.FastInstantiate(parent);
        rangeGo.name = rangeObject.Name;

        var rangeText = rangeGo.GetComponent<Text>();
        rangeText.text = rangeObject.DisplayText;
        rangeText.enabled = true;

        var valueGo = rangeGo.transform.GetChild(0).gameObject;
        var valueText = valueGo.GetComponent<Text>();
        valueText.text = rangeObject.GetRange();
        valueText.enabled = true;

        var transform = rangeGo.transform;
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