﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRole"></typeparam>
public class XtmfRoleStore<TRole> : IRoleStore<TRole> where TRole : class
{


    public XtmfRoleStore()
    {
    }

    public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // throw new NotImplementedException();
    }

    public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
