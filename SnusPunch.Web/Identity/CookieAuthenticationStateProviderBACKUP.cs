//using Microsoft.AspNetCore.Components.Authorization;
//using SnusPunch.Shared.Models.Auth;
//using SnusPunch.Shared.Models.ResultModel;
//using SnusPunch.Web.Clients;
//using SnusPunch.Web.Clients.Snus;
//using SnusPunch.Web.Identity.Models;
//using System.Net.Http.Json;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Json;

//namespace SnusPunch.Web.Identity
//{
//    public class CookieAuthenticationStateProvider(IHttpClientFactory aHttpClientFactory) : AuthenticationStateProvider, IAccountManagement
//    {
//        /// <summary>
//        /// Map the JavaScript-formatted properties to C#-formatted classes.
//        /// </summary>
//        private readonly JsonSerializerOptions mJsonSerializerOptions = new JsonSerializerOptions
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//        };

//        /// <summary>
//        /// Special auth client.
//        /// </summary>
//        private readonly HttpClient mHttpClient = aHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());

//        /// <summary>
//        /// Authentication state.
//        /// </summary>
//        private bool mAuthenticated = false;

//        /// <summary>
//        /// Default principal for anonymous (not authenticated) users.
//        /// </summary>
//        private readonly ClaimsPrincipal mUnauthenticated = new(new ClaimsIdentity());

//        /// <summary>
//        /// User login.
//        /// </summary>
//        /// <param name="email">The user's email address.</param>
//        /// <param name="password">The user's password.</param>
//        /// <returns>The result of the login request serialized to a <see cref="FormResult"/>.</returns>
//        public async Task<FormResult> LoginAsync(string email, string password)
//        {
//            try
//            {
//                // login with cookies
//                var result = await mHttpClient.PostAsJsonAsync(
//                    "login", new LoginRequestModel
//                    {
//                        UserName = email,
//                        Password = password
//                    });

//                var sResult = JsonSerializer.Deserialize<ResultModel>(await result.Content.ReadAsStringAsync());

//                // success?
//                if (result.IsSuccessStatusCode && sResult.Success)
//                {
//                    // need to refresh auth state
//                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

//                    // success!
//                    return new FormResult { Succeeded = true };
//                }
//            }
//            catch { }

//            // unknown error
//            return new FormResult
//            {
//                Succeeded = false,
//                ErrorList = ["Invalid email and/or password."]
//            };
//        }

//        /// <summary>
//        /// Get authentication state.
//        /// </summary>
//        /// <remarks>
//        /// Called by Blazor anytime and authentication-based decision needs to be made, then cached
//        /// until the changed state notification is raised.
//        /// </remarks>
//        /// <returns>The authentication state asynchronous request.</returns>
//        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            mAuthenticated = false;

//            // default to not authenticated
//            var user = mUnauthenticated;

//            try
//            {
//                // the user info endpoint is secured, so if the user isn't logged in this will fail
//                var userResponse = await mHttpClient.GetAsync("Info");

//                // throw if user info wasn't retrieved
//                userResponse.EnsureSuccessStatusCode();

//                // user is authenticated,so let's build their authenticated identity
//                var userJson = await userResponse.Content.ReadAsStringAsync();
//                var userInfo = JsonSerializer.Deserialize<ResultModel<string>>(userJson, mJsonSerializerOptions);

//                if (userInfo.Success && userInfo.ResultObject != null)
//                {
//                    // in this example app, name and email are the same
//                    var claims = new List<Claim>
//                    {
//                        new(ClaimTypes.Name, userInfo.ResultObject)
//                    };

//                    // request the roles endpoint for the user's roles
//                    var rolesResponse = await mHttpClient.GetAsync("Roles");

//                    // throw if request fails
//                    rolesResponse.EnsureSuccessStatusCode();

//                    // read the response into a string
//                    var rolesJson = await rolesResponse.Content.ReadAsStringAsync();

//                    // deserialize the roles string into an array
//                    var roles = JsonSerializer.Deserialize<ResultModel<List<RoleClaimModel>>>(rolesJson, mJsonSerializerOptions);

//                    // add any roles to the claims collection
//                    if (roles.Success && roles.ResultObject != null && roles.ResultObject.Count > 0)
//                    {
//                        foreach (var role in roles.ResultObject)
//                        {
//                            if (!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
//                            {
//                                claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
//                            }
//                        }
//                    }

//                    // set the principal
//                    var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
//                    user = new ClaimsPrincipal(id);
//                    mAuthenticated = true;
//                }
//            }
//            catch { }

//            // return the state
//            return new AuthenticationState(user);
//        }

//        public async Task LogoutAsync()
//        {
//            const string Empty = "{}";
//            var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");
//            await mHttpClient.PostAsync("logout", emptyContent);
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//        }

//        public async Task<bool> CheckAuthenticatedAsync()
//        {
//            await GetAuthenticationStateAsync();
//            return mAuthenticated;
//        }
//    }
//}
