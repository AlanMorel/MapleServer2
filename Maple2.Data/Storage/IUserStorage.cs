using Maple2.Data.Types;
using System.Collections.Generic;
using Maple2.Data.Converter;
using Maple2.Data.Types.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Maple2Storage.Enums;

namespace Maple2.Data.Storage
{
    public interface IUserStorage
    {
        UserStorage.Request Context();
    }
}
