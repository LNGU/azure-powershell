// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Commands.Common.Compute.Version2016_04_preview.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Describes a reference to Key Vault Key
    /// </summary>
    public partial class KeyVaultKeyReference
    {
        /// <summary>
        /// Initializes a new instance of the KeyVaultKeyReference class.
        /// </summary>
        public KeyVaultKeyReference()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the KeyVaultKeyReference class.
        /// </summary>
        /// <param name="keyUrl">The URL referencing a key encryption key in
        /// Key Vault.</param>
        /// <param name="sourceVault">The relative URL of the Key Vault
        /// containing the key.</param>
        public KeyVaultKeyReference(string keyUrl, SubResource sourceVault)
        {
            KeyUrl = keyUrl;
            SourceVault = sourceVault;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the URL referencing a key encryption key in Key Vault.
        /// </summary>
        [JsonProperty(PropertyName = "keyUrl")]
        public string KeyUrl { get; set; }

        /// <summary>
        /// Gets or sets the relative URL of the Key Vault containing the key.
        /// </summary>
        [JsonProperty(PropertyName = "sourceVault")]
        public SubResource SourceVault { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (KeyUrl == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "KeyUrl");
            }
            if (SourceVault == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "SourceVault");
            }
        }
    }
}
