using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace WSMCSOM {
  class Program {
    static void Main(string[] args) {
      #region login
      // target site to test this against
      const string siteCollectionUrl = "https://aconn.sharepoint.com/sites/infusion";
      Uri siteUri = new Uri(siteCollectionUrl);

      // login as an app with SharePoint Online & obtain access token
      //    this uses the client ID & secret found in the app.config
      // you must have manually created an app & granted it app only permissions manually
      //    through the browser prior to this
      var accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, 
        siteUri.Authority, 
        TokenHelper.GetRealmFromTargetUrl(siteUri));
      
      // get client context & 
      ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(siteCollectionUrl, 
        accessToken.AccessToken);
      #endregion

      // get instance of workflow service manager
      WorkflowServicesManager wfServicesManager = new WorkflowServicesManager(clientContext, clientContext.Web);

      // get Documents, Workflow history & task list IDs
      Web site = clientContext.Web;
      clientContext.Load(site, s => s.Url);

      List documentsList = site.Lists.GetByTitle("Documents");
      clientContext.Load(documentsList, list => list.Id);

      List historyList = site.Lists.GetByTitle("Workflow History");
      clientContext.Load(historyList, list => list.Id);

      List taskList = site.Lists.GetByTitle("Workflow Tasks");
      clientContext.Load(taskList, list => list.Id);

      clientContext.ExecuteQuery();
      Console.WriteLine("Target Site:                 {0}", site.Url);
      Console.WriteLine("Documents list ID:           {0}", documentsList.Id);
      Console.WriteLine("WorkflowHistoryList list ID: {0}", historyList.Id);
      Console.WriteLine("WorkflowTaskList list ID:    {0}", taskList.Id);

      #region WSM Deployment Service
      Console.WriteLine();
      Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
      Console.WriteLine();

      // display all installed workflows
      DeploymentService.ShowAllInstalledWorkflows(ref clientContext, ref wfServicesManager);

      // show a good & bad workflow validation
      DeploymentService.ValidateGoodWorkflow(ref clientContext, ref wfServicesManager);
      DeploymentService.ValidateBadWorkflow(ref clientContext, ref wfServicesManager);

      // create a new workflow
      //DeploymentService.InstallWorkflow(ref clientContext, ref wfServicesManager, documentsList.Id);
      #endregion

      #region WSM Subscription Service
      Console.WriteLine();
      Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
      Console.WriteLine();

      // get a workflow definition
      Guid workflowDefinitionId = DeploymentService.GetOneInstalledWorkflow(ref clientContext, ref wfServicesManager);

      // create a new association
      //SubscriptionService.CreateAssociation(ref clientContext,
      //                                      ref wfServicesManager,
      //                                      workflowDefinitionId,
      //                                      documentsList.Id,
      //                                      historyList.Id,
      //                                      taskList.Id);

      // list all associations
      SubscriptionService.ListAllAssociations(ref clientContext, ref wfServicesManager);
      #endregion

      #region WSM Instance Service
      Console.WriteLine();
      Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
      Console.WriteLine();

      // get a workflow subscription
      var workflowSubscription = SubscriptionService.GetOneSubscription(ref clientContext, ref wfServicesManager);

      // create a new instance
      //InstanceService.CreateInstance(ref clientContext, ref wfServicesManager, workflowSubscription);

      // list all instances
      InstanceService.ListAllInstances(ref clientContext, ref wfServicesManager, documentsList.Id);

      // publish custom event to a running instance
      //WorkflowInstance instance = InstanceService.GetOneRunningInstance(ref clientContext, ref wfServicesManager, documentsList.Id);
      //InstanceService.PublishMessageToWorkflowInstance(ref clientContext, ref wfServicesManager, instance);

      #endregion
    }
  }
}
