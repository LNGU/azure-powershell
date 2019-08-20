﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.HealthcareApis.Models;
using Microsoft.Azure.Commands.ResourceManager.Common;
using Microsoft.Azure.Graph.RBAC.Version1_6.ActiveDirectory;
using Microsoft.Azure.Management.HealthcareApis;
using Microsoft.Azure.Management.HealthcareApis.Models;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.PowerShell.Cmdlets.HealthcareApis.Common;
using Microsoft.Rest.Azure;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.HealthcareApis.Common
{
    public abstract class HealthcareApisBaseCmdlet : AzureRMCmdlet
    {
        private HealthcareApisManagementClientWrapper _healthcareApisManagementClientWrapper;

        private ActiveDirectoryClientWrapper _activeDirectoryClientWrapper;

        protected const string HealthcareApisAccountNameAlias = "HealthcareApisName";
        protected const string FhirServiceNameAlias = "FhirServiceName";

        protected const string TagsAlias = "Tags";

        protected const string ResourceProviderName = "Microsoft.HealthcareApis";
        protected const string ResourceTypeName = "services";

        public IHealthcareApisManagementClient HealthcareApisClient
        {
            get
            {
                if (_healthcareApisManagementClientWrapper == null)
                {
                    _healthcareApisManagementClientWrapper = new HealthcareApisManagementClientWrapper(DefaultProfile.DefaultContext);
                }

                _healthcareApisManagementClientWrapper.VerboseLogger = WriteVerboseWithTimestamp;
                _healthcareApisManagementClientWrapper.ErrorLogger = WriteErrorWithTimestamp;

                return _healthcareApisManagementClientWrapper.HealthcareApisManagementClient;
            }

            set { _healthcareApisManagementClientWrapper = new HealthcareApisManagementClientWrapper(value); }
        }

        public ActiveDirectoryClient ActiveDirectoryClient
        {
            get
            {
                if (_activeDirectoryClientWrapper == null)
                {
                    _activeDirectoryClientWrapper = new ActiveDirectoryClientWrapper(DefaultProfile.DefaultContext);
                }

                return _activeDirectoryClientWrapper.ActiveDirectoryClient;
            }
        }

        public string AccessPolicyID
        {
            get
            {
                ADObjectFilterOptions _options = new ADObjectFilterOptions()
                {
                    Id = DefaultProfile.DefaultContext.Account.Id
                };
                return ActiveDirectoryClient.GetObjectId(_options).ToString();
            }
        }

        public string TenantID
        {
            get
            {
                    return DefaultProfile.DefaultContext.Tenant.Id;
            }
        }

        /// <summary>
        /// Run Cmdlet with Error Handling (report error correctly)
        /// </summary>
        /// <param name="action"></param>
        protected void RunCmdLet(Action action)
        {
            try
            {
                action();
            }
            catch (ErrorDetailsException ex)
            {
                throw new PSInvalidOperationException(ex.Message, ex);
            }
        }

        protected void WriteHealthcareApisAccount(ServicesDescription healthcareApisAccount)
        {
            if (healthcareApisAccount != null)
            {
                WriteObject(PSHealthcareApisService.Create(healthcareApisAccount));
            }
        }

        protected void WriteHealthcareApisAccountList(
          IEnumerable<ServicesDescription> healthcareApisAccounts)
        {
            List<PSHealthcareApisService> output = new List<PSHealthcareApisService>();
            if (healthcareApisAccounts != null)
            {
                healthcareApisAccounts.ForEach(
                    healthcareApisAccount => output.Add(PSHealthcareApisService.Create(healthcareApisAccount)));
            }

            WriteObject(output, true);
        }

        protected bool ValidateAndExtractName(string resourceId, out string resourceGroupName, out string resourceName)
        {
            ResourceIdentifier resourceIdentifier = new ResourceIdentifier(resourceId);

            // validate the resource provider type
            if (string.Equals(ResourceProviderName,
                              ResourceIdentifier.GetProviderFromResourceType(resourceIdentifier.ResourceType),
                              System.StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(ResourceTypeName,
                                 ResourceIdentifier.GetTypeFromResourceType(resourceIdentifier.ResourceType),
                                 System.StringComparison.InvariantCultureIgnoreCase))
            {
                resourceGroupName = resourceIdentifier.ResourceGroupName;
                resourceName = resourceIdentifier.ResourceName;
                return true;
            }
            resourceGroupName = null;
            resourceName = null;
            return false;
        }

        public static PSHealthcareApisService ToPSFhirService(ServicesDescription serviceDescription)
        {
            return new PSHealthcareApisService(serviceDescription);
        }

        public static List<PSHealthcareApisService> ToPSFhirServices(IPage<ServicesDescription> fhirServiceApps)
        {
            using (IEnumerator<ServicesDescription> sdenumerator = fhirServiceApps.GetEnumerator())
            {
                var newpne = new List<PSHealthcareApisService>();
                while (sdenumerator.MoveNext())
                {
                    PSHealthcareApisService psHealthCareFhirService = ToPSFhirService(sdenumerator.Current);
                    newpne.Add(psHealthCareFhirService);
                }

                return newpne;
            }
        }
    }
}
