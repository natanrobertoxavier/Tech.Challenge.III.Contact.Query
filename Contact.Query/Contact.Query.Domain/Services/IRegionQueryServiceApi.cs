﻿
using Contact.Query.Domain.ResultServices;

namespace Contact.Query.Domain.Services;
public interface IRegionQueryServiceApi
{
    Task<Result<RegionResult>> RecoverByIdAsync(Guid id, string token);
}