using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAuth0App.Extensions
{
    public class CachedMap<T, R> where T : notnull
    {
        private int time;
        private Func<T, Task<R>> retriveData;

        private Dictionary<T, Task<R?>> dataMap;
        private Dictionary<T, Timer> timerMap;

        public CachedMap(Func<T, Task<R>> retriveData, int time)
        {
            dataMap = new Dictionary<T, Task<R?>>();
            timerMap = new Dictionary<T, Timer>();

            this.time = time;
            this.retriveData = retriveData;
        }

        public Task<R?> this[T index]
        {
            get
            {
                if (!timerMap.ContainsKey(index))
                    timerMap.Add(index, new Timer(async _ =>
                    {
                        dataMap.Remove(index);

                        await timerMap[index].DisposeAsync();
                        timerMap.Remove(index);
                    }, null, TimeSpan.FromSeconds(time), TimeSpan.Zero));

                if (!dataMap.ContainsKey(index)) dataMap.Add(index, retriveData.Invoke(index)!);

                return dataMap[index];
            }
        }
    }
}
