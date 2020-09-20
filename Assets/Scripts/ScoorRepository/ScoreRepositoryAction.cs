using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Assets.Scripts.ScoorRepository
{
    public class ScoreRepositoryAction
    {
        public void SaveNewScoor(ScoreModel score)
        {

            var AllScores = GetScores();
            AllScores.Scores.Add(score);
            string data = JsonUtility.ToJson(AllScores, true);

            File.WriteAllText(Application.persistentDataPath + "/SavedScores.json", data);

        }

        public ScoreModelCollection GetScores()
        {

            try
            {
                using (StreamReader r = new StreamReader(Application.persistentDataPath + "/SavedScores.json"))
                {
                    string json = r.ReadToEnd();
                    ScoreModelCollection scores = JsonUtility.FromJson<ScoreModelCollection>(json);

                    return scores;
                }
            }
            catch (System.Exception)
            {

                return new ScoreModelCollection()
                {
                    Scores = new List<ScoreModel>()
                };
            }
        }
    }
}
