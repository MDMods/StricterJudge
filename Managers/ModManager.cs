using Il2Cpp;
using Il2CppGameLogic;
using Il2CppPeroPeroGames.GlobalDefines;
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

        if (!AcceptedEnemies.Contains(noteData.type)) return false;

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

        transform.localPosition = parent.localPosition + posOffset * 10;
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

        // All offset's scaled down by a factor of 10 to reduce clutter of 0's
        if (isHighestActive)
        {
            perfectLeftOffset = new Vector3(3f, -10f);
            perfectRightOffset = new Vector3(55f, 1f);
            greatLeftOffset = new Vector3(6f, -15f);
            greatRightOffset = new Vector3(58f, -4f);
        }
        else
        {
            perfectLeftOffset = Vector3.zero;
            perfectRightOffset = new Vector3(18f, 4.5f);
            greatLeftOffset = new Vector3(3f, -5f);
            greatRightOffset = new Vector3(21f, -0.5f);
        }

        CreateObjectFromRange(GreatLeftRange, baseGo, parent, greatLeftOffset);
        CreateObjectFromRange(PerfectLeftRange, baseGo, parent, perfectLeftOffset);
        CreateObjectFromRange(PerfectRightRange, baseGo, parent, perfectRightOffset);
        CreateObjectFromRange(GreatRightRange, baseGo, parent, greatRightOffset);
    }
}