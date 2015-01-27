//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace AnonReadSecureWriteWeb {
  [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
  public class Customers : DataService<CustomerContext> {
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(DataServiceConfiguration config) {
#if DEBUG
      config.UseVerboseErrors = true;
#endif
      config.SetEntitySetAccessRule("*", EntitySetRights.All);
      config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
    }

    protected override CustomerContext CreateDataSource() {
      CustomerContextFactory factory = new CustomerContextFactory();
      CustomerContext context = factory.Create();
      return context;
    }

    [ChangeInterceptor("Customers")]
    public void ValidateCustomerChange(Customer customer, UpdateOperations updateOperations) {
      // get the headers & authority of the request
      var authority = HttpContext.Current.Request.Url.Authority;
      var headers = HttpContext.Current.Request.Headers;

      // validate the bare minimum headers are provided
      if (!headers.AllKeys.Contains("X-SP-AccessToken"))
        throw new DataServiceException(401, "Access Denied: No SharePoint context token provided in HTTP POST request.");
      if (!headers.AllKeys.Contains("SpAppWebUrl"))
        throw new DataServiceException(401, "Access Denied: No AppWeb provided in HTTP POST request.");

      try {
        var contextToken = headers.GetValues("X-SP-AccessToken").FirstOrDefault();
        var spAppWebUrl = headers.GetValues("SpAppWebUrl").FirstOrDefault();

        // create a client context
        using (var spClientContext = TokenHelper.GetClientContextWithContextToken(spAppWebUrl, contextToken, authority)) {
          // get the title of the calling site, just to verify it is a valid token
          spClientContext.Load(spClientContext.Web, w => w.Title);
          spClientContext.ExecuteQuery();
          // if no exception hit at this point, it's a valid token

          customer.SpContextToken = contextToken;
        }
      } catch (Exception) {
        // if an exception was hit, likely because the token wasn't valid... send rejection
        throw new DataServiceException(401, "Access Denied: Invalid context token provided in HTTP POST request.");
      }
    }
  }
}
