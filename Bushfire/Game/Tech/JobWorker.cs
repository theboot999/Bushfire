using BushFire.Game.Tech.Jobs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BushFire.Game.Tech
{
    class JobWorker
    {
        private Thread worker;
        private WorkerState workerState;
        private ConcurrentQueue<Job> jobQueue;
        private Reusables reusables;



        public JobWorker()
        {
            jobQueue = new ConcurrentQueue<Job>();
            reusables = new Reusables();

        }

        public void StartWorker()
        {
            workerState = WorkerState.Running;
            worker = new Thread(Running);
            worker.IsBackground = true;
            worker.Priority = ThreadPriority.Lowest;
            worker.Start();
        }

        public void PauseWorker()
        {
            workerState = WorkerState.Paused;
        }

        public void ResumeWorker()
        {
            workerState = WorkerState.Running;
        }

        public void SetToDestroy()
        {
            if (workerState != WorkerState.Destroying || workerState != WorkerState.Destroyed)
            {
                workerState = WorkerState.Destroying;
            }
        }

        public bool IsDestroyed()
        {
            return workerState == WorkerState.Destroyed;
        }

        public void AddJob(Job job)
        {
            jobQueue.Enqueue(job);
        }

        public void Running()
        {
            Job job;

            while (workerState == WorkerState.Running || workerState == WorkerState.Paused)
            {
                while (workerState == WorkerState.Running)
                {
                    if (jobQueue.TryDequeue(out job))
                    {
                        if (!job.isCancel)
                        {
                            job.Start(reusables);

                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }

            workerState = WorkerState.Destroyed;
        }
    }

    enum WorkerState
    {
        Running,
        Paused,
        Destroying,
        Destroyed
    }
}
