
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sage.Cloud.Services.Helpers
{
    public class TaskQueue : IDisposable
    {
        #region Properties / Constructor

        static TaskQueue()
        {
            Instance = new TaskQueue();
        }

        public static TaskQueue Instance { get; private set; }

        private CountdownEvent finished = new CountdownEvent(1);

        private readonly object _syncObj = new object();
        private readonly Queue<QTask> _tasks = new Queue<QTask>();
        private int _runningTaskCount;

        public void Queue(Action task)
        {
            Queue(false, task);
        }

        public int Count
        {
            get
            {
                lock (_syncObj)
                {
                    return _tasks.Count;
                }
            }
        }

        #endregion

        public void Queue(bool isParallel, Action task)
        {
            lock (_syncObj)
                _tasks.Enqueue(new QTask { IsParallel = isParallel, Task = task });

            ProcessTaskQueue();
        }

        private void ProcessTaskQueue()
        {
            lock (_syncObj)
            {
                if (_runningTaskCount != 0) return;
                if (_tasks.Count > 0 && _runningTaskCount == 0)
                    QueueUserWorkItem( _tasks.Dequeue() );
            }
        }

        private void QueueUserWorkItem(QTask qTask)
        {
            Action completionTask = () =>
            {
                qTask.Task();
                OnTaskCompleted();
            };

            finished.AddCount();
            _runningTaskCount++;
            Thread t = new Thread(_ => completionTask()) { IsBackground = false };
            t.Start();
        }

        private void OnTaskCompleted()
        {
            lock (_syncObj)
                if (--_runningTaskCount == 0)
                {
                    ProcessTaskQueue();
                    finished.Signal();
                }
        }

        public void WaitForProcessingComplete()
        {
            if( finished.CurrentCount > 0 ) finished.Signal();
            finished.Wait();
            finished.Reset();
        }

        //~TaskQueue()
        //{
        //    WaitForProcessingComplete();
        //}

        public void Dispose() {}

        private class QTask
        {
            public Action Task { get; set; }
            public bool IsParallel { get; set; }
        }
    }
}
