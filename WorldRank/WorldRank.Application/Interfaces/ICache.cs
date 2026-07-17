using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldRank.Application.Interfaces;
public interface ICache
{
    bool TryGet<T>(string key, out T? value);

    void Set<T>(string key, T value, TimeSpan ttl);

    void Remove(string key);
}