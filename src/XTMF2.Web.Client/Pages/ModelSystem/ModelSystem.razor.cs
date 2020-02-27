using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using XTMF2.Web.ApiClient;
using XTMF2.Web.Client.Services;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Client.Pages
{
    /// <summary>
    ///     Root page / component for the model system display.
    /// </summary>
    public partial class ModelSystem : ComponentBase
    {
        /// <summary>
        ///     Path parameter that specifies the project name.
        /// </summary>
        [Parameter]
        public string ProjectName { get; set; }

        /// <summary>
        ///     Path parameter that specifies the model system name.
        /// </summary>
        [Parameter]
        public string ModelSystemName { get; set; }

        [Inject] protected ModelSystemEditorClient EditingClient { get; set; }


        [Inject] protected ILogger<ModelSystem> Logger { get; set; }

        [Inject] protected NotificationService NotificationService { get; set; }

        protected ModelSystemModel ModelSystemInfo { get; private set; }

        protected ModelSystemEditingModel Model { get; private set; }

        protected bool IsLoaded { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ModelSystemInfo = new ModelSystemModel
            {
                Name = ModelSystemName
            };

            try
            {
                Model = await EditingClient.GetModelSystemAsync(ProjectName, ModelSystemName);
                IsLoaded = true;
            }
            catch (ApiException exception)
            {
                NotificationService.ErrorMessage($"Unable to load model system: {exception.Response}");
                IsLoaded = false;
            }
        }
    }
}