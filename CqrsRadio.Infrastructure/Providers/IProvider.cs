﻿using System.Data.Common;

namespace CqrsRadio.Infrastructure.Providers
{
    public interface IProvider
    {
        DbConnection Create();
    }
}
