using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace WSMCSOM {
  public class SubscriptionService {

    /// <summary>
    /// Create a new workflow association (subscription).
    /// </summary>
    public static void CreateAssociation(ref ClientContext clientConext,
                                         ref WorkflowServicesManager wfServicesManager,
                                         Guid definitionId,
                                         Guid listId,
                                         Guid historyListId,
                                         Guid taskListId) {
      WorkflowSubscriptionService subService = wfServicesManager.GetWorkflowSubscriptionService();

      Console.WriteLine();
      Console.WriteLine("Creating workflow association...");

      // create new association (aka: subscription)
      WorkflowSubscription newSubscription = new WorkflowSubscription(clientConext) {
        DefinitionId = definitionId,
        Enabled = true,
        Name = "Custom Association " + DateTime.Now
      };

      // define startup options
      //    automatic start options = ItemAdded & ItemUpdated
      //    manual start = WorkflowStart
      newSubscription.EventTypes = new List<string> { "ItemAdded", "ItemUpdated", "WorkflowStart" };

      // define the history & task associated lists
      newSubscription.SetProperty("HistoryListId", historyListId.ToString());
      newSubscription.SetProperty("TaskListId", taskListId.ToString());

      // OPTIONAL: if any values submitted by association form, add as properties here
      newSubscription.SetProperty("Prop1", "Value1");
      newSubscription.SetProperty("Prop2", "Value2");

      // create the association
      subService.PublishSubscriptionForList(newSubscription, listId); // creates association on list
      //subService.PublishSubscription(newSubscription);              // creates association on current site
      clientConext.ExecuteQuery();
      Console.WriteLine("Workflow association created!");
    }

    /// <summary>
    /// List all workflow associations for the specified workflow.
    /// </summary>
    public static void ListAllAssociations(ref ClientContext clientConext,
                                           ref WorkflowServicesManager wfServicesManager) {
      WorkflowSubscriptionService subService = wfServicesManager.GetWorkflowSubscriptionService();

      Console.WriteLine();
      Console.WriteLine("Listing all workflow associations: ");

      // get all associations
      WorkflowSubscriptionCollection wfSubscriptions = subService.EnumerateSubscriptions();
      //WorkflowSubscriptionCollection wfSubscriptions = subService.EnumerateSubscriptionsByDefinition(definitionId);
      //WorkflowSubscriptionCollection wfSubscriptions = subService.EnumerateSubscriptionsByList(listId);
      //WorkflowSubscriptionCollection wfSubscriptions = subService.EnumerateSubscriptionsByEventSource(eventSourceId);

      clientConext.Load(wfSubscriptions);
      clientConext.ExecuteQuery();

      // write all associations out
      foreach (var wfSubscription in wfSubscriptions) {
        Console.WriteLine("{0} - {1}",
          wfSubscription.Id,
          wfSubscription.Name
          );
      }
    }
    /// <summary>
    /// Retrieves a single workflow subscription.
    /// </summary>
    public static WorkflowSubscription GetOneSubscription(ref ClientContext clientContext,
                                                          ref WorkflowServicesManager wfServicesManager) {

      WorkflowSubscriptionService subService = wfServicesManager.GetWorkflowSubscriptionService();

      WorkflowSubscriptionCollection wfSubscriptions = subService.EnumerateSubscriptions();
      clientContext.Load(wfSubscriptions);
      clientContext.ExecuteQuery();

      return wfSubscriptions.FirstOrDefault();
    }
  }
}