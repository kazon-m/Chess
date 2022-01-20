using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SquareTableValue", menuName = "Chess", order = 0)]
    public class SquareTableValues : ScriptableObject
    {
        public TextAsset jsonData;

        public int[,] squareValues = new int[8, 8];

        public void Init()
        {
            squareValues = new int[8, 8];
            var wrapper = JsonUtility.FromJson<SquareTableJson>(jsonData.text);
            for(var i = 0; i < 8; i++) squareValues[0, i] = wrapper.firstRow[i];


            for(var i = 0; i < 8; i++) squareValues[1, i] = wrapper.secondRow[i];


            for(var i = 0; i < 8; i++) squareValues[2, i] = wrapper.thirdRow[i];


            for(var i = 0; i < 8; i++) squareValues[3, i] = wrapper.fourthRow[i];


            for(var i = 0; i < 8; i++) squareValues[4, i] = wrapper.fifthRow[i];


            for(var i = 0; i < 8; i++) squareValues[5, i] = wrapper.sixthRow[i];


            for(var i = 0; i < 8; i++) squareValues[6, i] = wrapper.seventhRow[i];


            for(var i = 0; i < 8; i++) squareValues[7, i] = wrapper.eigthRow[i];
        }
    }

    //Blame the Unity Serialization module, not me. 
    [Serializable]
    public class SquareTableJson
    {
        public int[] firstRow;
        public int[] secondRow;
        public int[] thirdRow;
        public int[] fourthRow;
        public int[] fifthRow;
        public int[] sixthRow;
        public int[] seventhRow;
        public int[] eigthRow;
    }
}