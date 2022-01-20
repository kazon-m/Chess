using System;
using System.Collections.Generic;
using Data.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    [CreateAssetMenu(fileName = "ChessPreset", menuName = "Chess/Chess Preset", order = 1)]
    public class ChessPreset : ScriptableObject
    {
        [Header("Square settings")]
        public Image SquarePrefab;
        public Color SquareWhiteColor;
        public Color SquareBlackColor;
        public Color SquareHoverColor;
        public Color SquareClickedColor;
        public Color SquarePossibleColor;
        
        [Header("Chess settings")]
        public Image ChessPrefab;
        
        public List<ChessItem> ChessList = new List<ChessItem>();

        [Serializable]
        public struct ChessItem
        {
            public ChessType type;
            public int value;
            public GameObject moves;
            public Sprite spriteWhite;
            public Sprite spriteBlack;
        }
    }
}