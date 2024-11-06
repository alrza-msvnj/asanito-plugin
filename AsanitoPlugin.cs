using Nop.Core;
using Nop.Core.Domain.Tasks;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Services.Tasks;
using Nop.Web.Framework.Menu;
using System.Collections.Generic;
using System.Linq;
using Tesla.Plugin.Widgets.CRM.Asanito.Data;

namespace Tesla.Plugin.Widgets.CRM.Asanito
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class AsanitoPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permissionService;
        private readonly AsanitoContext _context;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public AsanitoPlugin(ILocalizationService localizationService,
            IWebHelper webHelper,
            IPermissionService permissionService,
            AsanitoContext context,
            IScheduleTaskService scheduleTaskService)
        {
            _localizationService = localizationService;
            _webHelper = webHelper;
            _permissionService = permissionService;
            _context = context;
            _scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/Asanito/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsAsanito";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            _context.Install();

            var task = new ScheduleTask
            {
                Name = "Asanito Invoice Status",
                Seconds = 86400,
                Type = "Tesla.Plugin.Widgets.CRM.Asanito.Services.Tasks.InvoiceStatusTask",
                Enabled = true,
                StopOnError = false
            };

            _scheduleTaskService.InsertTask(task);

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            _context.Uninstall();

            var task = _scheduleTaskService.GetTaskByType("Tesla.Plugin.Widgets.CRM.Asanito.Services.Tasks.InvoiceStatusTask");
            if (task != null)
            {
                _scheduleTaskService.DeleteTask(task);
            }

            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            {
                return;
            }

            const string adminUrlPart = "Admin/";
            var generalSettingsNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Configuration").ChildNodes.FirstOrDefault(x => x.SystemName == "Settings");

            var AsanitoNodeMenu = new SiteMapNode
            {
                Title = _localizationService.GetResource("Tesla.Plugin.Widgets.CRM.Asanito"),
                Visible = true,
                SystemName = "Widgets.CRM.Asanito",
                IconClass = "fa fa-genderless",
                Url = $"{_webHelper.GetStoreLocation()}{adminUrlPart}Asanito/Configure",
            };

            if (!generalSettingsNode.ChildNodes.Contains(AsanitoNodeMenu))
            {
                generalSettingsNode.ChildNodes.Add(AsanitoNodeMenu);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        #endregion
    }

}