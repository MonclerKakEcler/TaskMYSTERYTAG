using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public interface IBaseJsonProvider<T> where T : ICloneable, new()
{
    UniTask Init();
    void Save(T data);
    T GetData();
}

public abstract class BaseJsonProvider<T> : IBaseJsonProvider<T> where T : ICloneable, new()
{
    protected abstract string FileName { get; }

    protected T _deserializeData;

    private readonly Queue<object> _savingQueue = new Queue<object>();
    private readonly string _dirPath;
    private bool _isSaving;

    protected BaseJsonProvider(string dirPath)
    {
        if (string.IsNullOrEmpty(dirPath))
        {
            throw new ArgumentException("dirPath is null or empty");
        }

        if (string.IsNullOrEmpty(FileName))
        {
            throw new ArgumentException("FileName is null or empty");
        }

        _dirPath = dirPath;
    }

    public async UniTask Init()
    {
        _deserializeData = await JSON.JsonRead<T>(_dirPath, FileName);
    }

    public void Save(T data)
    {
        _deserializeData = (T)data.Clone();
        _savingQueue.Enqueue(data.Clone());
        _ = SaveQueueData();
    }

    public T GetData()
    {
        return (T)_deserializeData.Clone();
    }

    private async UniTaskVoid SaveQueueData()
    {
        if (_isSaving)
        {
            return;
        }

        _isSaving = true;

        while (_savingQueue.Count > 0)
        {
            if (_savingQueue.TryDequeue(out object data))
            {
                await JSON.JsonSave(data, _dirPath, FileName);
            }
        }

        _isSaving = false;
    }
}
