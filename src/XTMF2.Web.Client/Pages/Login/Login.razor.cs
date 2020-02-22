using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace XTMF2.Web.Client.Pages
{
    /// <summary>
    ///     Single project view (page).
    /// </summary>
    public partial class Login : ComponentBase
    {

        [Inject]
        protected ILogger<Login> Logger { get; set; }

    }
}