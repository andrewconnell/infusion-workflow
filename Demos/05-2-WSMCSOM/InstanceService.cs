using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace WSMCSOM {
  public class InstanceService {
    /// <summary>
    /// Create a new workflow instance.
    /// </summary>
    public static void CreateInstance(ref ClientContext clientConext,
                                      ref WorkflowServicesManager wfServicesManager,
                                      WorkflowSubscription subscription) {
      WorkflowInstanceService instService = wfServicesManager.GetWorkflowInstanceService();

      Console.WriteLine();
      Console.WriteLine("Creating workflow instance...");

      Dictionary<string, object> startParameters = new Dictionary<string, object>();
      // if there are any values to send to the initiation form, add them here
      startParameters.Add("Name1", "Value1");
      startParameters.Add("Name2", "Value2");

      // start an instance
      instService.StartWorkflowOnListItem(subscription, 1, startParameters);
      //instService.StartWorkflow(subscription, startParameters); // when starting on a site

      clientConext.ExecuteQuery();
      Console.WriteLine("Workflow started!");
    }

    /// <summary>
    /// Publish message to existing instance.
    /// </summary>
    public static void PublishMessageToWorkflowInstance(ref ClientContext clientConext,
                                                        ref WorkflowServicesManager wfServicesManager,
                                                        WorkflowInstance instance) {
      WorkflowInstanceService instService = wfServicesManager.GetWorkflowInstanceService();

      Console.WriteLine();
      Console.WriteLine("Publishing event to running workflow instance...");

      // publish event
      instService.PublishCustomEvent(instance, "CustomEventName", "CustomEventMessage");
      clientConext.ExecuteQuery();
    }

    /// <summary>
    /// Show all running workflow instances
    /// </summary>
    public static void ListAllInstances(ref ClientContext clientConext,
                                        ref WorkflowServicesManager wfServicesManager,
                                        Guid listId) {
      WorkflowInstanceService instService = wfServicesManager.GetWorkflowInstanceService();

      Console.WriteLine();
      Console.WriteLine("Show all running workflow instances...");

      int listItemId = 1;
      WorkflowInstanceCollection wfInstances = instService.EnumerateInstancesForListItem(listId, listItemId);
      // WorkflowInstanceCollection wfInstances = instService.EnumerateInstancesForSite(); // get instances running on the current site

      clientConext.Load(wfInstances);
      clientConext.ExecuteQuery();
      foreach (var wfInstance in wfInstances) {
        Console.WriteLine("{0} - {1} - {2}|{3}",
                          wfInstance.Id,
                          wfInstance.LastUpdated,
                          wfInstance.Status,
                          wfInstance.UserStatus);
      }
    }

    /// <summary>
    /// Return a single workflow instance.
    /// </summary>
    public static WorkflowInstance GetOneRunningInstance(ref ClientContext clientConext,
                                                         ref WorkflowServicesManager wfServicesManager,
                                                         Guid listId) {
      WorkflowInstanceService instService = wfServicesManager.GetWorkflowInstanceService();

      int listItemId = 1;
      WorkflowInstanceCollection wfInstances = instService.EnumerateInstancesForListItem(listId, listItemId);
      
      clientConext.Load(wfInstances);
      clientConext.ExecuteQuery();
      
      return wfInstances.FirstOrDefault();
    }
  }
}