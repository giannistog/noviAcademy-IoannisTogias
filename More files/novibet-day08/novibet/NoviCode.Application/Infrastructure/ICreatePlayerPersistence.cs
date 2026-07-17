using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Infrastructure
{
    public interface ICreatePlayerPersistence
    {
        Task Persist(Player player);
    }
}
