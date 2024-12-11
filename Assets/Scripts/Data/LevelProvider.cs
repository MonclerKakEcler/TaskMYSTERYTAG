using System;
using System.Collections.Generic;
using System.Linq;

public interface ILevelProvider : IBaseJsonProvider<LevelHolder>
{

}

public class LevelProvider : BaseJsonProvider<LevelHolder>, ILevelProvider
{
    protected override string FileName => "LevelData";

    public LevelProvider(string dirPath) : base(dirPath)
    {
    }

}

public class LevelHolder : ICloneable
{
    public int ID { get; set; }
    public List<LevelData> LevelDataList { get; set; } = new List<LevelData>();

    public object Clone()
    {
      var clone = new LevelHolder();
        clone.ID = ID;
        clone.LevelDataList = LevelDataList.Select(x => (LevelData)x.Clone()).ToList();

      return clone;
    }
}

public class LevelData : ICloneable
{
    public int ID { get; set; }
    public int CountAsteroids { get; set; }
    public int SpeedAsteroids { get; set; }
    public int SpeedBullet { get; set; }
    public bool LevelComplete { get; set; }

    public object Clone()
    {
        var clone = new LevelData();
        clone.ID = ID;
        clone.CountAsteroids = CountAsteroids;
        clone.SpeedAsteroids = SpeedAsteroids;
        clone.SpeedBullet = SpeedBullet;
        clone.LevelComplete = LevelComplete;

        return clone;
    }
}
