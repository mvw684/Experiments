
using System;

namespace Fractals {
    public class Scheduler {
        public static readonly LimitedConcurrencyLevelTaskScheduler Instance = 
            new LimitedConcurrencyLevelTaskScheduler(Environment.ProcessorCount);
    }
}
