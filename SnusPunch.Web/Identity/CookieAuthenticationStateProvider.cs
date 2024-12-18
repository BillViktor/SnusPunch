﻿using Microsoft.AspNetCore.Components.Authorization;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.Clients.Snus;
using System.Security.Claims;

namespace SnusPunch.Web.Identity
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthClient mAuthClient;

        private UserInfoModel mUserInfoModel;
        private bool mAuthenticated = false;
        private readonly ClaimsPrincipal mUnauthenticated = new(new ClaimsIdentity());

        public UserInfoModel UserInfoModel { get { return mUserInfoModel; } }

        public CookieAuthenticationStateProvider(AuthClient aAuthClient)
        {
            mAuthClient = aAuthClient;
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            mAuthenticated = false;

            var sUser = mUnauthenticated;
            try
            {
                var sUserResponse = await mAuthClient.Info();

                if(sUserResponse.Success)
                {
                    mUserInfoModel = sUserResponse.ResultObject;

                    var sClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, sUserResponse.ResultObject.UserName),
                        new Claim(ClaimTypes.Email, sUserResponse.ResultObject.Email)
                    };

                    foreach (var sRole in sUserResponse.ResultObject.RoleClaims)
                    {
                        if (!string.IsNullOrEmpty(sRole.Type) && !string.IsNullOrEmpty(sRole.Value))
                        {
                            sClaims.Add(new Claim(sRole.Type, sRole.Value, sRole.ValueType, sRole.Issuer, sRole.OriginalIssuer));
                        }
                    }

                    var sId = new ClaimsIdentity(sClaims, nameof(CookieAuthenticationStateProvider));
                    sUser = new ClaimsPrincipal(sId);
                    mAuthenticated = true;
                }
            }
            catch { }

            return new AuthenticationState(sUser);
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return mAuthenticated;
        }
    }
}
