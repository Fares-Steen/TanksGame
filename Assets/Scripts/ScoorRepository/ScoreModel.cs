using System;
using System.Collections.Generic;

namespace Assets.Scripts.ScoorRepository
{
    [Serializable]
    public class ScoreModel
    {
        public string Name;
        public int Level;
        public int Round;
    }

    [Serializable]
    public class ScoreModelCollection
    {
        public List<ScoreModel> Scores;
    }
}
