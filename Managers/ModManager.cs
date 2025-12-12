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

    internal static void CreateTextObjects(GameObject baseGo, Transform parent)
    {
        var isHighestActive = baseGo.GetComponent<Text>().enabled;

        Ranges.ForEach(range => range.CreateTextObject(baseGo, parent, isHighestActive));
    }

    internal static bool UpdateNoteJudge(ref MusicData musicData)
    {
        var noteData = musicData.noteData;

        if (!AcceptedEnemies.Contains(noteData.type))
            return false;

        noteData.left_great_range = GreatLeftRange.Value;
        noteData.left_perfect_range = PerfectLeftRange.Value;
        noteData.right_perfect_range = PerfectRightRange.Value;
        noteData.right_great_range = GreatRightRange.Value;

        musicData.noteData = noteData;
        return true;
    }

    private static void CreateObjectFromRange(
        JudgementRange rangeObject,
        GameObject baseGo,
        Transform parent,
        bool isHighestActive
    )
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

        transform.localPosition =
            parent.localPosition + rangeObject.GetOffset(isHighestActive) * 10;
        transform.localRotation = parent.localRotation;
        transform.localScale *= 0.5f;

        transform.SetParent(parent);
    }

    private static void CreateTextObject(
        this JudgementRange rangeClass,
        GameObject baseGo,
        Transform parent,
        bool isHighestActive
    ) => CreateObjectFromRange(rangeClass, baseGo, parent, isHighestActive);
}
