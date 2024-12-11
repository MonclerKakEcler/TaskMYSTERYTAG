using System;
using System.Collections.Generic;
using System.Linq;

public interface IStatisticProvider : IBaseJsonProvider<StatisticHolder>
{

}

public class StatisticProvider : BaseJsonProvider<StatisticHolder>, IStatisticProvider
{
    protected override string FileName => "StatisticData";

    public StatisticProvider(string dirPath) : base(dirPath)
    {
    }

}

public class StatisticHolder : ICloneable
{
    public int ID { get; set; }
    public List<StatisticData> StatisticDataList { get; set; } = new List<StatisticData>();

    public object Clone()
    {
        var clone = new StatisticHolder();
        clone.ID = ID;
        clone.StatisticDataList = StatisticDataList.Select(x => (StatisticData)x.Clone()).ToList();

        return clone;
    }
}

public class StatisticData : ICloneable
{
    public int ID { get; set; }
    public int LevelID { get; set; }
    public string Time { get; set; }
    public int Score { get; set; }
    public bool Win { get; set; }

    public object Clone()
    {
        var clone = new StatisticData();
        clone.ID = ID;
        clone.LevelID = LevelID;
        clone.Time = Time;
        clone.Score = Score;
        clone.Win = Win;
        return clone;
    }
}
