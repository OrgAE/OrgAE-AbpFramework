using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class RoleManagement
    {
        protected const string PermissionProviderName = "R";

        protected PermissionManagementModal PermissionManagementModal;
        
        protected string ManagePermissionsPolicyName;
        
        protected bool HasManagePermissionsPermission { get; set; }

        public RoleManagement()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
            LocalizationResource = typeof(IdentityResource);

            CreatePolicyName = IdentityPermissions.Roles.Create;
            UpdatePolicyName = IdentityPermissions.Roles.Update;
            DeletePolicyName = IdentityPermissions.Roles.Delete;
            ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
        }

        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(IdentityPermissions.Roles.ManagePermissions);
        }

        protected override async Task DeleteEntityAsync(IdentityRoleDto entity)
        {
            //TODO: I will move try/catch to base class, doing here for test purpose 
            try
            {
                await CheckDeletePolicyAsync();

                await AppService.DeleteAsync(entity.Id);
                await GetEntitiesAsync();
            }
            catch (Exception ex)
            {
                await ShowError(ex);
                //await Message.Error("Error: " + ex.Message); //This works if I uncomment
            }
        }

        protected override string GetDeleteConfirmationMessage(IdentityRoleDto entity)
        {
            return string.Format(L["RoleDeletionConfirmationMessage"], entity.Name);
        }
    }
}
