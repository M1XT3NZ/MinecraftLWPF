using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;

namespace MinecraftLWPF.Minecraft;

public class AccountsManager
{
    //This is the current session, you can use this to get the username, uuid, accesstoken, etc.
    private MSession currentSession;

    // This creates the HttpClient that will be used to make requests to the Mojang API
    private readonly HttpClient httpClient = new();

    public async Task Login()
    {
        // Build with options
        var loginHandler = new JELoginHandlerBuilder()
            .WithHttpClient(httpClient)
            // This is the Path where the accounts of the user will be saved (in this case it will be saved at %appdata%/Veltrox/accounts.json)
            .WithAccountManager(MinecraftSettings.Instance.SettingsPath + "accounts.json")
            .Build();
        currentSession = await loginHandler.Authenticate();
    }
}